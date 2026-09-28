namespace Medieval_Internship.Models;

public enum UserRole
{
    Siswa,
    GuruPendamping,
    PembimbingIndustri,
    Admin,
    Operator,
    Monitor
}

public static class RoleHelper
{
    public const string RoleAdmin = "Admin";
    public const string RoleOperator = "Operator";
    public const string RoleSiswa = "Siswa";
    public const string RoleGuru = "Guru Pendamping";
    public const string RoleIndustri = "Pembimbing Industri";
    public const string RoleMonitor = "Monitor";

    public static readonly string[] AllRoles =
    [
        RoleSiswa,
        RoleGuru,
        RoleIndustri,
        RoleAdmin,
        RoleOperator,
        RoleMonitor
    ];

    public static UserRole FromString(string roleStr) => roleStr switch
    {
        RoleAdmin => UserRole.Admin,
        RoleOperator => UserRole.Operator,
        RoleSiswa => UserRole.Siswa,
        RoleGuru => UserRole.GuruPendamping,
        RoleIndustri => UserRole.PembimbingIndustri,
        RoleMonitor => UserRole.Monitor,
        _ => UserRole.Siswa
    };

    public static string ToStringRole(UserRole role) => role switch
    {
        UserRole.Admin => RoleAdmin,
        UserRole.Operator => RoleOperator,
        UserRole.Siswa => RoleSiswa,
        UserRole.GuruPendamping => RoleGuru,
        UserRole.PembimbingIndustri => RoleIndustri,
        UserRole.Monitor => RoleMonitor,
        _ => RoleSiswa
    };

    public static string GetRoleBadgeColor(string roleStr) => roleStr switch
    {
        RoleSiswa => "#10B981",          // Emerald Green
        RoleGuru => "#2563EB",           // Blue
        RoleIndustri => "#6366F1",       // Indigo
        RoleAdmin => "#7C3AED",          // Purple
        RoleOperator => "#D97706",       // Amber
        RoleMonitor => "#0D9488",        // Teal
        _ => "#1E3A5F"
    };

    public static string GetRoleLightBgColor(string roleStr) => roleStr switch
    {
        RoleSiswa => "#ECFDF5",
        RoleGuru => "#EFF6FF",
        RoleIndustri => "#EEF2FF",
        RoleAdmin => "#F5F3FF",
        RoleOperator => "#FFFBEB",
        RoleMonitor => "#F0FDFA",
        _ => "#F1F5F9"
    };

    public static string GetDashboardRoute(UserRole role) => role switch
    {
        UserRole.Admin => "AdminDashboardPage",
        UserRole.Siswa => "SiswaDashboardPage",
        UserRole.GuruPendamping => "GuruDashboardPage",
        UserRole.PembimbingIndustri => "IndustriDashboardPage",
        UserRole.Operator => "OperatorDashboardPage",
        UserRole.Monitor => "MonitorDashboardPage",
        _ => "SiswaDashboardPage"
    };
}
