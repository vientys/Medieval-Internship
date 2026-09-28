using Medieval_Internship.Models;
using Medieval_Internship.Services;

namespace Medieval_Internship.Views;

public partial class AdminDashboardPage : ContentPage
{
    private readonly DashboardDataService _dataService = DashboardDataService.Instance;

    public AdminDashboardPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var user = await DashboardUIHelper.VerifyAccessAsync(this, UserRole.Admin);
        if (user == null) return;

        UserNameLabel.Text = user.FullName;
        AvatarInitialsLabel.Text = user.Initials;
        HeaderOrgLabel.Text = string.IsNullOrWhiteSpace(user.OrganizationOrSchool) ? "SMK Negeri 1 Jakarta • Pusat Data Administrasi" : user.OrganizationOrSchool;
        HeroWelcomeTitle.Text = $"Panel Kontrol Admin, {user.FullName}! ⚙️";

        var data = _dataService.GetDashboardData(UserRole.Admin, user);
        DashboardUIHelper.RenderStatCards(StatCardsContainer, data.StatCards);
        DashboardUIHelper.RenderQuickActions(QuickActionsContainer, data.QuickActions, this);
        DashboardUIHelper.RenderActivitiesList(ActivitiesListContainer, data.Activities);
    }

    private async void OnLogoutClicked(object? sender, EventArgs e)
    {
        await DashboardUIHelper.ConfirmLogoutAsync(this);
    }
}
