using Medieval_Internship.Models;
using Medieval_Internship.Services;

namespace Medieval_Internship.Views.Modules;

public partial class EksporLaporanModulePage : ContentPage
{
    private readonly AuthService _authService = AuthService.Instance;
    private readonly PklDataService _dataService = PklDataService.Instance;
    private UserModel? _currentUser;
    private string _selectedFormat = "Excel";
    private ExportReportResult? _lastResult;

    public EksporLaporanModulePage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _currentUser = _authService.CurrentUser;
        if (_currentUser == null) return;

        UserRoleSubtitle.Text = $"{_currentUser.FullName} ({_currentUser.RoleName}) • {_currentUser.OrganizationOrSchool}";
        ReportTypePicker.SelectedIndex = 0;
    }

    private void OnFormatExcelClicked(object? sender, EventArgs e)
    {
        _selectedFormat = "Excel";
        FormatExcelBtn.BackgroundColor = Color.FromArgb("#10B981");
        FormatExcelBtn.TextColor = Colors.White;
        FormatPdfBtn.BackgroundColor = Color.FromArgb("#F1F5F9");
        FormatPdfBtn.TextColor = Color.FromArgb("#475569");
    }

    private void OnFormatPdfClicked(object? sender, EventArgs e)
    {
        _selectedFormat = "PDF";
        FormatPdfBtn.BackgroundColor = Color.FromArgb("#2563EB");
        FormatPdfBtn.TextColor = Colors.White;
        FormatExcelBtn.BackgroundColor = Color.FromArgb("#F1F5F9");
        FormatExcelBtn.TextColor = Color.FromArgb("#475569");
    }

    private async void OnGenerateReportClicked(object? sender, EventArgs e)
    {
        var type = ReportTypePicker.SelectedItem?.ToString() ?? "Rekapitulasi Kehadiran & Absensi Siswa";

        _lastResult = _dataService.GenerateExportReport(type, _selectedFormat);

        ResultCard.IsVisible = true;
        ResultFileInfoLabel.Text = $"Format: {_lastResult.Format} • Ukuran: {_lastResult.FileSizeFormatted} • {_lastResult.TotalRows} Baris Data";
        ResultPathLabel.Text = $"File: {_lastResult.FilePath}";

        if (File.Exists(_lastResult.FilePath))
        {
            var content = await File.ReadAllTextAsync(_lastResult.FilePath);
            ResultPreviewLabel.Text = content.Length > 2000 ? content[..2000] + "\n... (data dilanjutkan di dalam berkas)" : content;
        }

        await DisplayAlertAsync("Ekspor Selesai 🎉",
            $"Laporan '{_lastResult.Title}' berhasil diekspor ke dalam format {_lastResult.Format}.\n\nLokasi Berkas:\n{_lastResult.FilePath}",
            "Tutup");
    }

    private async void OnOpenGeneratedFileClicked(object? sender, EventArgs e)
    {
        if (_lastResult == null || !File.Exists(_lastResult.FilePath)) return;

        await DisplayAlertAsync("Buka Berkas Laporan",
            $"Berkas tersedia di sistem file:\n{_lastResult.FilePath}\n\nAnda dapat membuka berkas ini menggunakan Microsoft Excel, Adobe Acrobat Reader, atau aplikasi pembaca dokumen teks.",
            "OK");
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
