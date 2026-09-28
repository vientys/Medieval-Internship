using Medieval_Internship.Models;
using Medieval_Internship.Services;

namespace Medieval_Internship.Views.Modules;

public partial class ManajemenAplikasiModulePage : ContentPage
{
    private readonly AuthService _authService = AuthService.Instance;
    private readonly PklDataService _dataService = PklDataService.Instance;
    private UserModel? _currentUser;

    public ManajemenAplikasiModulePage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _currentUser = _authService.CurrentUser;
        if (_currentUser == null) return;

        UserRoleSubtitle.Text = $"{_currentUser.FullName} ({_currentUser.RoleName}) • {_currentUser.OrganizationOrSchool}";
        LoadConfig();
    }

    private void LoadConfig()
    {
        var config = _dataService.GetSchoolConfig();
        // FR-APK-01 fields
        AppNameEntry.Text = config.ApplicationName;
        AppDescEditor.Text = config.ApplicationDescription;
        LogoEntry.Text = config.Logo;
        FaviconEntry.Text = config.Favicon;
        RadiusEntry.Text = config.AttendanceRadiusMeters.ToString();
        ColorThemeEntry.Text = config.ColorTheme;
        MaintenanceEntry.Text = config.MaintenanceStatus;

        // School fields
        SchoolNameEntry.Text = config.SchoolName;
        NpsnEntry.Text = config.Npsn;
        PrincipalEntry.Text = config.PrincipalName;
        HubinLeadEntry.Text = config.HubinCoordinator;
        AddressEditor.Text = config.Address;

        AcademicYearEntry.Text = config.ActiveAcademicYear;
        MinAttendanceEntry.Text = config.MinAttendancePercent.ToString("F0");
        WorkStartTimeEntry.Text = config.WorkStartTime;
        SessionTimeoutEntry.Text = config.SessionTimeoutMinutes.ToString();

        WebsiteEntry.Text = config.Website;
        InstagramEntry.Text = config.Instagram;
        YoutubeEntry.Text = config.Youtube;
        HelpdeskEntry.Text = config.HelpdeskContact;
    }

    private void OnSaveConfigClicked(object? sender, EventArgs e)
    {
        var config = _dataService.GetSchoolConfig();
        // FR-APK-01 fields
        config.ApplicationName = AppNameEntry.Text?.Trim() ?? string.Empty;
        config.ApplicationDescription = AppDescEditor.Text?.Trim() ?? string.Empty;
        config.Logo = LogoEntry.Text?.Trim() ?? string.Empty;
        config.Favicon = FaviconEntry.Text?.Trim() ?? string.Empty;
        config.ColorTheme = ColorThemeEntry.Text?.Trim() ?? string.Empty;
        config.MaintenanceStatus = MaintenanceEntry.Text?.Trim() ?? string.Empty;
        if (int.TryParse(RadiusEntry.Text, out int rad))
            config.AttendanceRadiusMeters = rad;

        config.SchoolName = SchoolNameEntry.Text?.Trim() ?? string.Empty;
        config.Npsn = NpsnEntry.Text?.Trim() ?? string.Empty;
        config.PrincipalName = PrincipalEntry.Text?.Trim() ?? config.PrincipalName;
        config.HubinCoordinator = HubinLeadEntry.Text?.Trim() ?? config.HubinCoordinator;
        config.Address = AddressEditor.Text?.Trim() ?? config.Address;

        config.ActiveAcademicYear = AcademicYearEntry.Text?.Trim() ?? config.ActiveAcademicYear;
        if (double.TryParse(MinAttendanceEntry.Text, out double minAtt))
            config.MinAttendancePercent = minAtt;

        config.WorkStartTime = WorkStartTimeEntry.Text?.Trim() ?? config.WorkStartTime;
        if (int.TryParse(SessionTimeoutEntry.Text, out int timeout))
        {
            config.SessionTimeoutMinutes = timeout;
            _authService.SessionTimeoutMinutes = timeout;
        }

        config.Website = WebsiteEntry.Text?.Trim() ?? config.Website;
        config.Instagram = InstagramEntry.Text?.Trim() ?? config.Instagram;
        config.Youtube = YoutubeEntry.Text?.Trim() ?? config.Youtube;
        config.HelpdeskContact = HelpdeskEntry.Text?.Trim() ?? config.HelpdeskContact;

        var (success, msg) = _dataService.UpdateSchoolConfigStrict(config);
        ShowBanner(msg, isError: !success);
    }

    private void ShowBanner(string message, bool isError)
    {
        StatusBanner.IsVisible = true;
        StatusLabel.Text = message;
        if (isError)
        {
            StatusBanner.BackgroundColor = Color.FromArgb("#FEF2F2");
            StatusBanner.Stroke = Color.FromArgb("#FCA5A5");
            StatusLabel.TextColor = Color.FromArgb("#DC2626");
        }
        else
        {
            StatusBanner.BackgroundColor = Color.FromArgb("#ECFDF5");
            StatusBanner.Stroke = Color.FromArgb("#6EE7B7");
            StatusLabel.TextColor = Color.FromArgb("#065F46");
        }
    }

    private async void OnBackClicked(object? sender, EventArgs e)
    {
        if (Navigation.ModalStack.Count > 0)
        {
            await Navigation.PopModalAsync();
        }
        else
        {
            var role = _currentUser?.Role ?? UserRole.Admin;
            await Shell.Current.GoToAsync($"//{RoleHelper.GetDashboardRoute(role)}");
        }
    }
}
