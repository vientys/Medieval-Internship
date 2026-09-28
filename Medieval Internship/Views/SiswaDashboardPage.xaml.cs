using Medieval_Internship.Models;
using Medieval_Internship.Services;

namespace Medieval_Internship.Views;

public partial class SiswaDashboardPage : ContentPage
{
    private readonly DashboardDataService _dataService = DashboardDataService.Instance;

    public SiswaDashboardPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var user = await DashboardUIHelper.VerifyAccessAsync(this, UserRole.Siswa);
        if (user == null) return;

        UserNameLabel.Text = user.FullName;
        UserClassLabel.Text = string.IsNullOrWhiteSpace(user.DetailInfo) ? "Siswa Magang" : user.DetailInfo;
        AvatarInitialsLabel.Text = user.Initials;
        HeaderOrgLabel.Text = string.IsNullOrWhiteSpace(user.OrganizationOrSchool) ? "SMK Negeri 1 Jakarta • Tahun Ajaran 2026/2027" : user.OrganizationOrSchool;
        HeroWelcomeTitle.Text = $"Semangat Pagi, {user.FullName}! 🚀";
        HeroSubtitle.Text = $"Siswa Magang (Intern) • {user.OrganizationOrSchool}";

        var data = _dataService.GetDashboardData(UserRole.Siswa, user);
        DashboardUIHelper.RenderStatCards(StatCardsContainer, data.StatCards);
        DashboardUIHelper.RenderQuickActions(QuickActionsContainer, data.QuickActions, this);
        DashboardUIHelper.RenderJournalsList(JournalsListContainer, data.Journals, UserRole.Siswa, this);
        DashboardUIHelper.RenderActivitiesList(ActivitiesListContainer, data.Activities);
    }

    private async void OnAddJournalClicked(object? sender, EventArgs e)
    {
        await DashboardUIHelper.HandleQuickActionAsync(this, new QuickActionModel { ActionKey = "tulis_jurnal" });
    }

    private async void OnLogoutClicked(object? sender, EventArgs e)
    {
        await DashboardUIHelper.ConfirmLogoutAsync(this);
    }
}
