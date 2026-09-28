using Medieval_Internship.Models;
using Medieval_Internship.Services;

namespace Medieval_Internship.Views;

public partial class OperatorDashboardPage : ContentPage
{
    private readonly DashboardDataService _dataService = DashboardDataService.Instance;

    public OperatorDashboardPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var user = await DashboardUIHelper.VerifyAccessAsync(this, UserRole.Operator);
        if (user == null) return;

        UserNameLabel.Text = user.FullName;
        AvatarInitialsLabel.Text = user.Initials;
        HeaderOrgLabel.Text = string.IsNullOrWhiteSpace(user.OrganizationOrSchool) ? "BKK & Hubungan Industri • SMK Negeri 1 Jakarta" : user.OrganizationOrSchool;
        HeroWelcomeTitle.Text = $"Halo, {user.FullName}! 📋";

        var data = _dataService.GetDashboardData(UserRole.Operator, user);
        DashboardUIHelper.RenderStatCards(StatCardsContainer, data.StatCards);
        DashboardUIHelper.RenderQuickActions(QuickActionsContainer, data.QuickActions, this);
        DashboardUIHelper.RenderDocumentsList(DocumentsListContainer, data.Documents, this);
    }

    private async void OnAddDocumentClicked(object? sender, EventArgs e)
    {
        await DisplayAlertAsync("Terbitkan Dokumen", "Pilih jenis dokumen resmi: Surat Pengantar DUDI, Surat Tugas Pembimbing, atau Berita Acara PKL.", "Lanjutkan");
    }

    private async void OnLogoutClicked(object? sender, EventArgs e)
    {
        await DashboardUIHelper.ConfirmLogoutAsync(this);
    }
}
