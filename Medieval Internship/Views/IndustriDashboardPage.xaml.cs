using Medieval_Internship.Models;
using Medieval_Internship.Services;

namespace Medieval_Internship.Views;

public partial class IndustriDashboardPage : ContentPage
{
    private readonly DashboardDataService _dataService = DashboardDataService.Instance;

    public IndustriDashboardPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var user = await DashboardUIHelper.VerifyAccessAsync(this, UserRole.PembimbingIndustri);
        if (user == null) return;

        UserNameLabel.Text = user.FullName;
        UserOrgLabel.Text = string.IsNullOrWhiteSpace(user.DetailInfo) ? "Pembimbing Industri" : user.DetailInfo;
        AvatarInitialsLabel.Text = user.Initials;
        HeaderOrgLabel.Text = string.IsNullOrWhiteSpace(user.OrganizationOrSchool) ? "PT Telkom Indonesia • Divisi Cloud & Software Lab" : user.OrganizationOrSchool;
        HeroWelcomeTitle.Text = $"Selamat Datang, {user.FullName}! 🏢";

        var data = _dataService.GetDashboardData(UserRole.PembimbingIndustri, user);
        DashboardUIHelper.RenderStatCards(StatCardsContainer, data.StatCards);
        DashboardUIHelper.RenderQuickActions(QuickActionsContainer, data.QuickActions, this);
        DashboardUIHelper.RenderStudentsList(StudentsListContainer, data.Students, this);
        DashboardUIHelper.RenderJournalsList(JournalsListContainer, data.Journals, UserRole.PembimbingIndustri, this);
    }

    private async void OnLogoutClicked(object? sender, EventArgs e)
    {
        await DashboardUIHelper.ConfirmLogoutAsync(this);
    }
}
