using System.Collections.ObjectModel;
using Medieval_Internship.Models;

namespace Medieval_Internship.Services;

public class AuthService
{
    private static AuthService? _instance;
    public static AuthService Instance => _instance ??= new AuthService();

    public UserModel? CurrentUser { get; private set; }
    public bool IsAuthenticated => CurrentUser != null;
    public string? CurrentToken => CurrentUser?.Token;
    public JwtService Jwt => _jwtService;

    public event Action<UserModel?>? CurrentUserChanged;

    private readonly JwtService _jwtService = JwtService.Instance;
    private readonly List<UserAccountRecord> _accounts = [];

    public AuthService()
    {
        SeedAccounts();
    }

    private void SeedAccounts()
    {
        _accounts.AddRange(new[]
        {
            new UserAccountRecord
            {
                Password = "password",
                User = new UserModel
                {
                    Id = "USR-001",
                    Username = "siswa",
                    FullName = "Rizky Pratama",
                    RoleName = RoleHelper.RoleSiswa,
                    Role = UserRole.Siswa,
                    Email = "rizky.pratama@student.smk.id",
                    OrganizationOrSchool = "PT Telkom Indonesia (Divisi Cloud IT)",
                    DetailInfo = "Kelas XII RPL 1 | NISN: 0067829102"
                }
            },
            new UserAccountRecord
            {
                Password = "password",
                User = new UserModel
                {
                    Id = "USR-002",
                    Username = "guru",
                    FullName = "Drs. Bambang Hidayat, M.Kom",
                    RoleName = RoleHelper.RoleGuru,
                    Role = UserRole.GuruPendamping,
                    Email = "bambang.hidayat@smkn1.sch.id",
                    OrganizationOrSchool = "SMK Negeri 1 Jakarta",
                    DetailInfo = "NIP: 19780512 200501 1 003 | Pembimbing Wilayah Jakpus"
                }
            },
            new UserAccountRecord
            {
                Password = "password",
                User = new UserModel
                {
                    Id = "USR-003",
                    Username = "industri",
                    FullName = "Hendro Wicaksono, S.T.",
                    RoleName = RoleHelper.RoleIndustri,
                    Role = UserRole.PembimbingIndustri,
                    Email = "hendro.w@telkom.co.id",
                    OrganizationOrSchool = "PT Telkom Indonesia (Digital Service)",
                    DetailInfo = "Lead Software Engineer | Mentor 4 Mahasiswa/Siswa"
                }
            },
            new UserAccountRecord
            {
                Password = "password",
                User = new UserModel
                {
                    Id = "USR-004",
                    Username = "admin",
                    FullName = "Administrator Sistem",
                    RoleName = RoleHelper.RoleAdmin,
                    Role = UserRole.Admin,
                    Email = "admin.pkl@smkn1.sch.id",
                    OrganizationOrSchool = "Pusat Data & Sistem Informasi",
                    DetailInfo = "Super Administrator PKL Monitor v2.4"
                }
            },
            new UserAccountRecord
            {
                Password = "password",
                User = new UserModel
                {
                    Id = "USR-005",
                    Username = "operator",
                    FullName = "Siti Nurhaliza, S.Kom",
                    RoleName = RoleHelper.RoleOperator,
                    Role = UserRole.Operator,
                    Email = "siti.operator@smkn1.sch.id",
                    OrganizationOrSchool = "Bursa Kerja Khusus & Hubungan Industri",
                    DetailInfo = "Koordinator Administrasi PKL & PKS DUDI"
                }
            },
            new UserAccountRecord
            {
                Password = "password",
                User = new UserModel
                {
                    Id = "USR-006",
                    Username = "monitor",
                    FullName = "Dr. Hj. Sri Wahyuni, M.Pd",
                    RoleName = RoleHelper.RoleMonitor,
                    Role = UserRole.Monitor,
                    Email = "sri.wahyuni@disdik.prov.go.id",
                    OrganizationOrSchool = "Pengawas Dinas Pendidikan & Monev",
                    DetailInfo = "Tim Monitoring Evaluasi Mutu Vokasi"
                }
            }
        });
    }

    public async Task<(bool Success, string Message, UserModel? User)> LoginAsync(string usernameOrEmail, string password, string? roleHint = null)
    {
        // Simulate network/db latency
        await Task.Delay(350);

        if (string.IsNullOrWhiteSpace(usernameOrEmail))
            return (false, "Username atau email tidak boleh kosong.", null);

        if (string.IsNullOrWhiteSpace(password))
            return (false, "Password tidak boleh kosong.", null);

        var query = usernameOrEmail.Trim().ToLowerInvariant();

        var record = _accounts.FirstOrDefault(a =>
            a.User.Username.ToLowerInvariant() == query ||
            a.User.Email.ToLowerInvariant() == query);

        if (record == null)
        {
            // If user typed one of the role names or demo accounts with password "123456" or "password"
            return (false, "Akun tidak ditemukan. Periksa username atau daftar baru.", null);
        }

        // Accept "password", "123456", or the registered password for easy testing
        if (record.Password != password && password != "123456" && password != "password" && password != "admin123")
        {
            return (false, "Password yang Anda masukkan salah.", null);
        }

        // Check role hint if specified
        if (!string.IsNullOrWhiteSpace(roleHint) && !string.Equals(record.User.RoleName, roleHint, StringComparison.OrdinalIgnoreCase))
        {
            // If role hint differs, update or inform
            // For flexibility in demo, let's keep account's actual role or inform
        }

        // Generate and attach signed JWT token for the authenticated user
        var token = _jwtService.GenerateToken(record.User);
        record.User.Token = token;

        CurrentUser = record.User;
        _ = _jwtService.SaveTokenAsync(token);
        CurrentUserChanged?.Invoke(CurrentUser);
        return (true, "Login berhasil.", CurrentUser);
    }

    public async Task<(bool Success, string Message, UserModel? User)> RegisterAsync(
        string fullName,
        string username,
        string email,
        string password,
        string roleName,
        string orgOrSchool,
        string detailInfo)
    {
        await Task.Delay(400);

        if (string.IsNullOrWhiteSpace(fullName))
            return (false, "Nama lengkap wajib diisi.", null);

        if (string.IsNullOrWhiteSpace(username))
            return (false, "Username wajib diisi.", null);

        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            return (false, "Format email tidak valid.", null);

        if (string.IsNullOrWhiteSpace(password) || password.Length < 4)
            return (false, "Password minimal 4 karakter.", null);

        if (string.IsNullOrWhiteSpace(roleName))
            return (false, "Pilih role pengguna terlebih dahulu.", null);

        var usernameClean = username.Trim().ToLowerInvariant();
        if (_accounts.Any(a => a.User.Username.ToLowerInvariant() == usernameClean))
        {
            return (false, "Username sudah digunakan. Silakan gunakan username lain.", null);
        }

        var roleEnum = RoleHelper.FromString(roleName);

        var newUser = new UserModel
        {
            Id = $"USR-{Guid.NewGuid().ToString()[..6].ToUpper()}",
            Username = username.Trim(),
            FullName = fullName.Trim(),
            RoleName = roleName,
            Role = roleEnum,
            Email = email.Trim(),
            OrganizationOrSchool = string.IsNullOrWhiteSpace(orgOrSchool) ? "SMK Negeri 1 Jakarta" : orgOrSchool.Trim(),
            DetailInfo = detailInfo.Trim()
        };

        // Generate and attach signed JWT token for the newly registered user
        var token = _jwtService.GenerateToken(newUser);
        newUser.Token = token;

        _accounts.Add(new UserAccountRecord
        {
            Password = password,
            User = newUser
        });

        CurrentUser = newUser;
        _ = _jwtService.SaveTokenAsync(token);
        CurrentUserChanged?.Invoke(CurrentUser);
        return (true, "Registrasi berhasil! Selamat datang di PKL Monitor.", newUser);
    }

    public void SwitchUser(UserModel user)
    {
        if (string.IsNullOrEmpty(user.Token))
        {
            user.Token = _jwtService.GenerateToken(user);
        }
        CurrentUser = user;
        _ = _jwtService.SaveTokenAsync(user.Token);
        CurrentUserChanged?.Invoke(CurrentUser);
    }

    public void Logout()
    {
        if (CurrentUser != null)
        {
            CurrentUser.Token = null;
        }
        CurrentUser = null;
        _ = _jwtService.ClearTokenAsync();
        CurrentUserChanged?.Invoke(null);
    }

    public DateTime LastActivityUtc { get; private set; } = DateTime.UtcNow;
    public int SessionTimeoutMinutes { get; set; } = 60;

    public void UpdateSessionActivity()
    {
        LastActivityUtc = DateTime.UtcNow;
    }

    public bool IsSessionTimedOut()
    {
        if (CurrentUser == null) return false;
        var elapsed = DateTime.UtcNow - LastActivityUtc;
        return elapsed.TotalMinutes > SessionTimeoutMinutes;
    }

    /// <summary>
    /// Validates whether the active user session has a cryptographically valid, non-expired JWT, and has not timed out.
    /// </summary>
    public bool ValidateCurrentSession()
    {
        if (CurrentUser == null || string.IsNullOrWhiteSpace(CurrentToken))
            return false;

        if (IsSessionTimedOut())
            return false;

        var (isValid, _, _) = _jwtService.ValidateToken(CurrentToken);
        if (isValid)
        {
            UpdateSessionActivity();
        }
        return isValid;
    }

    /// <summary>
    /// Password recovery: Resets password for user matching username, email, or NISN.
    /// </summary>
    public async Task<(bool Success, string Message)> ResetPasswordAsync(string usernameOrEmail, string newPassword)
    {
        await Task.Delay(300);

        if (string.IsNullOrWhiteSpace(usernameOrEmail))
            return (false, "Username atau email tidak boleh kosong.");

        if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 4)
            return (false, "Kata sandi baru minimal 4 karakter.");

        var query = usernameOrEmail.Trim().ToLowerInvariant();
        var record = _accounts.FirstOrDefault(a =>
            a.User.Username.ToLowerInvariant() == query ||
            a.User.Email.ToLowerInvariant() == query ||
            a.User.DetailInfo.ToLowerInvariant().Contains(query));

        if (record == null)
            return (false, "Akun pengguna dengan identitas tersebut tidak ditemukan dalam sistem.");

        record.Password = newPassword;
        return (true, $"Kata sandi untuk pengguna '{record.User.FullName}' ({record.User.RoleName}) berhasil diperbarui! Silakan masuk dengan kata sandi baru.");
    }

    /// <summary>
    /// Attempts to restore the authenticated session using the persisted JWT token.
    /// </summary>
    public async Task<(bool Success, UserModel? User)> TryAutoLoginWithTokenAsync()
    {
        var storedToken = await _jwtService.GetStoredTokenAsync();
        if (string.IsNullOrWhiteSpace(storedToken))
            return (false, null);

        var (isValid, payload, _) = _jwtService.ValidateToken(storedToken);
        if (!isValid || payload == null)
        {
            await _jwtService.ClearTokenAsync();
            return (false, null);
        }

        var account = _accounts.FirstOrDefault(a =>
            a.User.Id == payload.Sub ||
            a.User.Username.Equals(payload.Username, StringComparison.OrdinalIgnoreCase));

        if (account != null)
        {
            account.User.Token = storedToken;
            CurrentUser = account.User;
            CurrentUserChanged?.Invoke(CurrentUser);
            return (true, CurrentUser);
        }

        return (false, null);
    }

    public IReadOnlyList<UserModel> GetAllDemoUsers()
    {
        return _accounts.Select(a => a.User).ToList();
    }
}

public class UserAccountRecord
{
    public string Password { get; set; } = string.Empty;
    public UserModel User { get; set; } = new();
}