using Medieval_Internship.Models;
using Medieval_Internship.Services;

namespace Medieval_Internship.Views.Modules;

public partial class LaporanAkhirModulePage : ContentPage
{
    private readonly AuthService _authService = AuthService.Instance;
    private readonly PklDataService _dataService = PklDataService.Instance;
    private UserModel? _currentUser;
    private FinalReportRecord? _activeReport;

    public LaporanAkhirModulePage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _currentUser = _authService.CurrentUser;
        if (_currentUser == null) return;

        UserRoleSubtitle.Text = $"{_currentUser.FullName} ({_currentUser.RoleName}) • {_currentUser.OrganizationOrSchool}";

        // Role-based default tab selection
        if (_currentUser.Role == UserRole.PembimbingIndustri)
        {
            SwitchTab(1); // Nilai Industri
        }
        else if (_currentUser.Role == UserRole.GuruPendamping)
        {
            SwitchTab(2); // Nilai Guru
        }
        else
        {
            SwitchTab(0); // Laporan & Unggah
        }

        RefreshAll();
    }

    private void RefreshAll()
    {
        var studentId = _currentUser?.Role == UserRole.Siswa ? _currentUser.Id : "USR-001";
        _activeReport = _dataService.GetFinalReportByStudent(studentId) ?? _dataService.GetAllFinalReports().FirstOrDefault();

        if (_activeReport != null)
        {
            ReportStatusTitle.Text = _activeReport.ReportTitle;
            ReportStatusDesc.Text = $"Status: {_activeReport.StatusText} • Diunggah: {_activeReport.UploadDate:dd MMM yyyy}\nBerkas: {_activeReport.FileName}";
            ReportStatusBadgeText.Text = _activeReport.StatusText;
            ReportStatusBadge.BackgroundColor = Color.FromArgb(_activeReport.StatusColor);

            bool hasRevision = !string.IsNullOrWhiteSpace(_activeReport.RevisionNotes);
            RevisionNoticeBorder.IsVisible = hasRevision;
            RevisionNotesLabel.Text = _activeReport.RevisionNotes;

            // Form Fields
            UploadTitleEntry.Text = _activeReport.ReportTitle;
            UploadAbstractEditor.Text = _activeReport.Abstract;
            SelectedReportFileLabel.Text = $"{_activeReport.FileName} (Tersimpan di Cloud)";

            // Industri Scores
            ScoreTeknisEntry.Text = _activeReport.NilaiTeknisIndustri.ToString("F1");
            ScoreSoftSkillEntry.Text = _activeReport.NilaiSoftSkillIndustri.ToString("F1");
            ScoreDisiplinEntry.Text = _activeReport.NilaiDisiplinIndustri.ToString("F1");
            IndustriNotesEditor.Text = _activeReport.CatatanIndustri;

            // Guru Scores
            ScoreLaporanEntry.Text = _activeReport.NilaiLaporanGuru.ToString("F1");
            ScoreSidangEntry.Text = _activeReport.NilaiSidangGuru.ToString("F1");
            GuruNotesEditor.Text = _activeReport.CatatanGuru;

            // Transcript
            FinalStudentNameLabel.Text = $"{_activeReport.StudentName} • {_activeReport.ClassName}";
            FinalCompanyLabel.Text = $"Tempat Praktik: {_activeReport.CompanyName}";
            TranscriptDudiLabel.Text = _activeReport.RataRataIndustri.ToString("F1");
            TranscriptGuruLabel.Text = _activeReport.RataRataGuru.ToString("F1");
            TranscriptFinalScoreLabel.Text = _activeReport.NilaiAkhir.ToString("F1");
            TranscriptGradeLabel.Text = $"Predikat {_activeReport.Predikat}";
        }
    }

    private void OnSubmitReportClicked(object? sender, EventArgs e)
    {
        var title = UploadTitleEntry.Text?.Trim() ?? string.Empty;
        var abstractText = UploadAbstractEditor.Text?.Trim() ?? string.Empty;
        var fileName = SelectedReportFileLabel.Text.Split(' ')[0];

        if (string.IsNullOrWhiteSpace(title))
        {
            ShowBanner("Judul laporan akhir PKL wajib diisi.", isError: true);
            UploadTitleEntry.Focus();
            return;
        }

        var studentId = _currentUser?.Id ?? "USR-001";
        var studentName = _currentUser?.FullName ?? "Rizky Pratama";
        var className = "XII RPL 1";
        var company = _currentUser?.OrganizationOrSchool ?? "PT Telkom Indonesia";

        var (success, msg, report) = _dataService.SubmitFinalReport(
            studentId, studentName, className, company, title, abstractText, fileName);

        ShowBanner(msg, isError: !success);
        RefreshAll();
    }

    private void OnSaveIndustriScoreClicked(object? sender, EventArgs e)
    {
        if (_activeReport == null) return;

        double.TryParse(ScoreTeknisEntry.Text, out double teknis);
        double.TryParse(ScoreSoftSkillEntry.Text, out double softSkill);
        double.TryParse(ScoreDisiplinEntry.Text, out double disiplin);
        var notes = IndustriNotesEditor.Text?.Trim() ?? string.Empty;

        var (success, msg) = _dataService.GradeReportByIndustri(
            _activeReport.Id, teknis, softSkill, disiplin, notes);

        ShowBanner(msg, isError: !success);
        RefreshAll();
    }

    private void OnSaveGuruScoreClicked(object? sender, EventArgs e)
    {
        if (_activeReport == null) return;

        double.TryParse(ScoreLaporanEntry.Text, out double laporan);
        double.TryParse(ScoreSidangEntry.Text, out double sidang);
        var notes = GuruNotesEditor.Text?.Trim() ?? string.Empty;

        var (success, msg) = _dataService.GradeReportByGuru(
            _activeReport.Id, laporan, sidang, notes);

        ShowBanner(msg, isError: !success);
        RefreshAll();
        SwitchTab(3); // Go to transcript
    }

    private async void OnRequestRevisionClicked(object? sender, EventArgs e)
    {
        if (_activeReport == null) return;

        var revNotes = await DisplayPromptAsync("Catatan Revisi Laporan Bab",
            "Tuliskan rincian perbaikan laporan untuk siswa:",
            initialValue: "Perbaiki Bab 4 pada bagian diagram alur sistem dan lampirkan hasil testing.");

        if (!string.IsNullOrWhiteSpace(revNotes))
        {
            var (success, msg) = _dataService.ReviseReport(_activeReport.Id, revNotes);
            ShowBanner(msg, isError: !success);
            RefreshAll();
        }
    }

    private async void OnChooseReportFileClicked(object? sender, EventArgs e)
    {
        var chosen = await DisplayActionSheetAsync("Pilih File Laporan Akhir", "Batal", null,
            "Laporan_Akhir_Lengkap_Revisi_Final.pdf (12.4 MB)",
            "Draft_Laporan_PKL_Bab1_sd_Bab5.pdf (8.1 MB)",
            "Naskah_Publikasi_Ilmiah_Vokasi.pdf (3.2 MB)");

        if (!string.IsNullOrWhiteSpace(chosen) && chosen != "Batal")
        {
            SelectedReportFileLabel.Text = chosen;
        }
    }

    private async void OnPrintCertificateClicked(object? sender, EventArgs e)
    {
        if (_activeReport == null) return;

        await DisplayAlertAsync("Penerbitan Sertifikat PKL Resmi 📜",
            $"Sertifikat Kelulusan PKL diterbitkan untuk:\n• Nama: {_activeReport.StudentName}\n• Nilai Akhir: {_activeReport.NilaiAkhir:F1} ({_activeReport.Predikat})\n• Tempat Praktik: {_activeReport.CompanyName}\n• Nomor Seri: SERT-PKL-2026-{_activeReport.Id}\n\nDokumen sertifikat siap dicetak atau diunduh sebagai PDF resmi ber-barcode.",
            "Buka Sertifikat");
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

    private void SwitchTab(int index)
    {
        SectionStatus.IsVisible = index == 0;
        SectionIndustri.IsVisible = index == 1;
        SectionGuru.IsVisible = index == 2;
        SectionTranscript.IsVisible = index == 3;

        TabStatusBtn.BackgroundColor = index == 0 ? Color.FromArgb("#1E3A5F") : Color.FromArgb("#F1F5F9");
        TabStatusBtn.TextColor = index == 0 ? Colors.White : Color.FromArgb("#475569");

        TabIndustriBtn.BackgroundColor = index == 1 ? Color.FromArgb("#1E3A5F") : Color.FromArgb("#F1F5F9");
        TabIndustriBtn.TextColor = index == 1 ? Colors.White : Color.FromArgb("#475569");

        TabGuruBtn.BackgroundColor = index == 2 ? Color.FromArgb("#1E3A5F") : Color.FromArgb("#F1F5F9");
        TabGuruBtn.TextColor = index == 2 ? Colors.White : Color.FromArgb("#475569");

        TabTranscriptBtn.BackgroundColor = index == 3 ? Color.FromArgb("#1E3A5F") : Color.FromArgb("#F1F5F9");
        TabTranscriptBtn.TextColor = index == 3 ? Colors.White : Color.FromArgb("#475569");
    }

    private void OnTabStatusClicked(object? sender, EventArgs e) => SwitchTab(0);
    private void OnTabIndustriClicked(object? sender, EventArgs e) => SwitchTab(1);
    private void OnTabGuruClicked(object? sender, EventArgs e) => SwitchTab(2);
    private void OnTabTranscriptClicked(object? sender, EventArgs e) => SwitchTab(3);

    private async void OnBackClicked(object? sender, EventArgs e)
    {
        if (Navigation.ModalStack.Count > 0)
        {
            await Navigation.PopModalAsync();
        }
        else
        {
            var role = _currentUser?.Role ?? UserRole.Siswa;
            await Shell.Current.GoToAsync($"//{RoleHelper.GetDashboardRoute(role)}");
        }
    }
}
