using Medieval_Internship.Models;
using Medieval_Internship.Services;

namespace Medieval_Internship.Views;

public partial class GuruDashboardPage : ContentPage
{
    private readonly DashboardDataService _dataService = DashboardDataService.Instance;

    public GuruDashboardPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var user = await DashboardUIHelper.VerifyAccessAsync(this, UserRole.GuruPendamping);
        if (user == null) return;

        UserNameLabel.Text = user.FullName;
        UserNipLabel.Text = string.IsNullOrWhiteSpace(user.DetailInfo) ? "Guru Pendamping" : user.DetailInfo;
        AvatarInitialsLabel.Text = user.Initials;
        HeaderOrgLabel.Text = string.IsNullOrWhiteSpace(user.OrganizationOrSchool) ? "SMK Negeri 1 Jakarta • Tim Pembimbing Lapangan" : user.OrganizationOrSchool;
        HeroWelcomeTitle.Text = $"Selamat Bertugas, {user.FullName}! 👨‍🏫";

        var data = _dataService.GetDashboardData(UserRole.GuruPendamping, user);
        DashboardUIHelper.RenderStatCards(StatCardsContainer, data.StatCards);
        DashboardUIHelper.RenderQuickActions(QuickActionsContainer, data.QuickActions, this);
        DashboardUIHelper.RenderStudentsList(StudentsListContainer, data.Students, this);
        DashboardUIHelper.RenderJournalsList(JournalsListContainer, data.Journals, UserRole.GuruPendamping, this);
    }

    private async void OnViewAllStudentsTapped(object? sender, TappedEventArgs e)
    {
        await DisplayAlertAsync("Daftar Siswa", "Menampilkan seluruh 28 siswa bimbingan PKL di wilayah Jakarta Pusat & Selatan.", "OK");
    }

    private async void OnLogoutClicked(object? sender, EventArgs e)
    {
        await DashboardUIHelper.ConfirmLogoutAsync(this);
    }
}
