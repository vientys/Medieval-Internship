using System.Text.Json.Serialization;

namespace Medieval_Internship.Models;

public class JwtPayloadModel
{
    [JsonPropertyName("sub")]
    public string Sub { get; set; } = string.Empty;

    [JsonPropertyName("unique_name")]
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string FullName { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("role")]
    public string Role { get; set; } = string.Empty;

    [JsonPropertyName("org")]
    public string Organization { get; set; } = string.Empty;

    [JsonPropertyName("jti")]
    public string Jti { get; set; } = string.Empty;

    [JsonPropertyName("iss")]
    public string Iss { get; set; } = string.Empty;

    [JsonPropertyName("aud")]
    public string Aud { get; set; } = string.Empty;

    [JsonPropertyName("iat")]
    public long Iat { get; set; }

    [JsonPropertyName("exp")]
    public long Exp { get; set; }

    [JsonIgnore]
    public DateTimeOffset IssuedAt => DateTimeOffset.FromUnixTimeSeconds(Iat);

    [JsonIgnore]
    public DateTimeOffset ExpiresAt => DateTimeOffset.FromUnixTimeSeconds(Exp);

    [JsonIgnore]
    public bool IsExpired => DateTimeOffset.UtcNow.ToUnixTimeSeconds() >= Exp;
}
