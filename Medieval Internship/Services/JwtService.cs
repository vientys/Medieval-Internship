using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Medieval_Internship.Models;

namespace Medieval_Internship.Services;

public class JwtService
{
    private static JwtService? _instance;
    public static JwtService Instance => _instance ??= new JwtService();

    // Default configuration for PKL Monitor JWT authentication
    private const string DefaultSecretKey = "MedievalInternship_PKL_Monitor_Secure_Jwt_Secret_Key_2026_#XyZ!";
    private const string DefaultIssuer = "PKLMonitorAuthServer";
    private const string DefaultAudience = "PKLMonitorClients";
    private const string StoredTokenKey = "pkl_jwt_auth_token";

    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly TimeSpan _defaultTokenLifetime;

    public JwtService(
        string? secretKey = null,
        string? issuer = null,
        string? audience = null,
        TimeSpan? defaultTokenLifetime = null)
    {
        _secretKey = secretKey ?? DefaultSecretKey;
        _issuer = issuer ?? DefaultIssuer;
        _audience = audience ?? DefaultAudience;
        _defaultTokenLifetime = defaultTokenLifetime ?? TimeSpan.FromDays(7);
    }

    /// <summary>
    /// Generates a signed RFC 7519 compliant JSON Web Token (HS256) for the specified user.
    /// </summary>
    public string GenerateToken(UserModel user, TimeSpan? lifetime = null)
    {
        var now = DateTimeOffset.UtcNow;
        var expires = now.Add(lifetime ?? _defaultTokenLifetime);

        var header = new Dictionary<string, string>
        {
            { "alg", "HS256" },
            { "typ", "JWT" }
        };

        var payload = new JwtPayloadModel
        {
            Sub = user.Id,
            Username = user.Username,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.RoleName,
            Organization = user.OrganizationOrSchool,
            Jti = Guid.NewGuid().ToString("N"),
            Iss = _issuer,
            Aud = _audience,
            Iat = now.ToUnixTimeSeconds(),
            Exp = expires.ToUnixTimeSeconds()
        };

        var headerJson = JsonSerializer.Serialize(header);
        var payloadJson = JsonSerializer.Serialize(payload);

        var encodedHeader = Base64UrlEncode(Encoding.UTF8.GetBytes(headerJson));
        var encodedPayload = Base64UrlEncode(Encoding.UTF8.GetBytes(payloadJson));

        var unsignedToken = $"{encodedHeader}.{encodedPayload}";
        var signature = ComputeHmacSha256(unsignedToken, _secretKey);
        var encodedSignature = Base64UrlEncode(signature);

        return $"{unsignedToken}.{encodedSignature}";
    }

    /// <summary>
    /// Validates the structure, cryptographic HMAC-SHA256 signature, and expiration of a JWT.
    /// </summary>
    public (bool IsValid, JwtPayloadModel? Payload, string? ErrorMessage) ValidateToken(string? token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return (false, null, "Token tidak boleh kosong.");
        }

        var parts = token.Split('.');
        if (parts.Length != 3)
        {
            return (false, null, "Format JWT tidak valid. Token harus terdiri dari 3 segmen.");
        }

        var encodedHeader = parts[0];
        var encodedPayload = parts[1];
        var encodedSignature = parts[2];

        try
        {
            // Verify HMAC-SHA256 Signature
            var unsignedToken = $"{encodedHeader}.{encodedPayload}";
            var expectedSignature = ComputeHmacSha256(unsignedToken, _secretKey);
            var actualSignature = Base64UrlDecode(encodedSignature);

            if (!CryptographicOperations.FixedTimeEquals(expectedSignature, actualSignature))
            {
                return (false, null, "Tanda tangan (signature) token tidak valid atau telah dimodifikasi.");
            }

            // Decode and inspect payload
            var payloadBytes = Base64UrlDecode(encodedPayload);
            var payloadJson = Encoding.UTF8.GetString(payloadBytes);
            var payload = JsonSerializer.Deserialize<JwtPayloadModel>(payloadJson);

            if (payload == null)
            {
                return (false, null, "Klaim (payload) token tidak dapat diproses.");
            }

            // Check token expiration
            if (payload.IsExpired)
            {
                return (false, payload, $"Token telah kedaluwarsa pada {payload.ExpiresAt:dd/MM/yyyy HH:mm:ss} UTC.");
            }

            return (true, payload, null);
        }
        catch (Exception ex)
        {
            return (false, null, $"Gagal memvalidasi token: {ex.Message}");
        }
    }

    /// <summary>
    /// Decodes the JWT payload without verifying the signature (useful for client inspection).
    /// </summary>
    public JwtPayloadModel? DecodeToken(string? token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return null;

        var parts = token.Split('.');
        if (parts.Length < 2)
            return null;

        try
        {
            var payloadBytes = Base64UrlDecode(parts[1]);
            var payloadJson = Encoding.UTF8.GetString(payloadBytes);
            return JsonSerializer.Deserialize<JwtPayloadModel>(payloadJson);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Persists the active JWT token securely on device storage.
    /// </summary>
    public async Task SaveTokenAsync(string token)
    {
        try
        {
            await SecureStorage.Default.SetAsync(StoredTokenKey, token);
        }
        catch
        {
            // Fallback to Preferences if SecureStorage is unavailable in simulator/environment
            Preferences.Default.Set(StoredTokenKey, token);
        }
    }

    /// <summary>
    /// Retrieves the persisted JWT token from device storage.
    /// </summary>
    public async Task<string?> GetStoredTokenAsync()
    {
        try
        {
            var token = await SecureStorage.Default.GetAsync(StoredTokenKey);
            if (!string.IsNullOrWhiteSpace(token))
                return token;
        }
        catch
        {
            // Ignore SecureStorage errors and try Preferences fallback
        }

        return Preferences.Default.Get(StoredTokenKey, (string?)null);
    }

    /// <summary>
    /// Clears the stored JWT token upon user logout.
    /// </summary>
    public async Task ClearTokenAsync()
    {
        try
        {
            SecureStorage.Default.Remove(StoredTokenKey);
        }
        catch
        {
            // Ignore SecureStorage errors
        }

        Preferences.Default.Remove(StoredTokenKey);
        await Task.CompletedTask;
    }

    #region Crypto & Base64Url Utilities

    private static byte[] ComputeHmacSha256(string data, string secretKey)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));
        return hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
    }

    private static string Base64UrlEncode(byte[] input)
    {
        var base64 = Convert.ToBase64String(input);
        return base64
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }

    private static byte[] Base64UrlDecode(string input)
    {
        var base64 = input
            .Replace('-', '+')
            .Replace('_', '/');

        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }

        return Convert.FromBase64String(base64);
    }

    #endregion
}
