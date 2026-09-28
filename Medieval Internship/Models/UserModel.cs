namespace Medieval_Internship.Models;

public class UserModel
{
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public string Email { get; set; } = string.Empty;
    public string OrganizationOrSchool { get; set; } = string.Empty;
    public string DetailInfo { get; set; } = string.Empty;
    public string? Token { get; set; }
    public string Initials => string.Join("", FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                      .Take(2)
                                      .Select(s => s[0]))
                                      .ToUpper();
    public string RoleBadgeColor => RoleHelper.GetRoleBadgeColor(RoleName);
    public string RoleLightBgColor => RoleHelper.GetRoleLightBgColor(RoleName);
}
