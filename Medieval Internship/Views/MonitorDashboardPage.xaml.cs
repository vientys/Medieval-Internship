using Medieval_Internship.Models;
using Medieval_Internship.Services;

namespace Medieval_Internship.Views;

public partial class MonitorDashboardPage : ContentPage
{
    private readonly DashboardDataService _dataService = DashboardDataService.Instance;

    public MonitorDashboardPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var user = await DashboardUIHelper.VerifyAccessAsync(this, UserRole.Monitor);
        if (user == null) return;

        UserNameLabel.Text = user.FullName;
        AvatarInitialsLabel.Text = user.Initials;
        HeaderOrgLabel.Text = string.IsNullOrWhiteSpace(user.OrganizationOrSchool) ? "Dinas Pendidikan Provinsi • Komite Penjaminan Mutu" : user.OrganizationOrSchool;
        HeroWelcomeTitle.Text = $"Dashboard Eksekutif Monev, {user.FullName}! 📈";

        var data = _dataService.GetDashboardData(UserRole.Monitor, user);
        DashboardUIHelper.RenderStatCards(StatCardsContainer, data.StatCards);
        DashboardUIHelper.RenderQuickActions(QuickActionsContainer, data.QuickActions, this);
        DashboardUIHelper.RenderActivitiesList(ActivitiesListContainer, data.Activities);
    }

    private async void OnLogoutClicked(object? sender, EventArgs e)
    {
        await DashboardUIHelper.ConfirmLogoutAsync(this);
    }
}
