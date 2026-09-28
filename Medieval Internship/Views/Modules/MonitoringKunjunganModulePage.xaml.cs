using Medieval_Internship.Models;
using Medieval_Internship.Services;
using Microsoft.Maui.Controls.Shapes;

namespace Medieval_Internship.Views.Modules;

public partial class MonitoringKunjunganModulePage : ContentPage
{
    private readonly AuthService _authService = AuthService.Instance;
    private readonly PklDataService _dataService = PklDataService.Instance;
    private UserModel? _currentUser;

    public MonitoringKunjunganModulePage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _currentUser = _authService.CurrentUser;
        if (_currentUser == null) return;

        UserRoleSubtitle.Text = $"{_currentUser.FullName} ({_currentUser.RoleName}) • {_currentUser.OrganizationOrSchool}";

        if (_currentUser.Role == UserRole.Monitor)
        {
            SwitchTab(2); // Monev Analysis tab
        }
        else
        {
            SwitchTab(0); // List tab
        }

        RefreshAll();
    }

    private void RefreshAll()
    {
        var visits = _dataService.GetAllVisits();
        RenderVisitsList(visits);
    }

    private void RenderVisitsList(List<SupervisionVisitRecord> list)
    {
        VisitsContainer.Children.Clear();

        if (list.Count == 0)
        {
            VisitsContainer.Children.Add(new Label
            {
                Text = "Belum ada catatan kunjungan monitoring yang terdaftar.",
                TextColor = Color.FromArgb("#64748B"),
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 20)
            });
            return;
        }

        foreach (var v in list)
        {
            var card = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(14) },
                BackgroundColor = Colors.White,
                Stroke = Color.FromArgb("#E2E8F0"),
                StrokeThickness = 1,
                Padding = new Thickness(16, 14)
            };

            var stack = new VerticalStackLayout { Spacing = 8 };

            var topRow = new Grid
            {
                ColumnDefinitions =
                [
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                ]
            };

            var info = new VerticalStackLayout { Spacing = 2 };
            info.Children.Add(new Label
            {
                Text = v.CompanyName,
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#0F172A")
            });
            info.Children.Add(new Label
            {
                Text = $"Pembimbing: {v.GuruName} • {v.VisitDateFormatted}",
                FontSize = 11,
                TextColor = Color.FromArgb("#64748B")
            });
            Grid.SetColumn(info, 0);
            topRow.Children.Add(info);

            var badge = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(8) },
                BackgroundColor = Color.FromArgb("#F5F3FF"),
                Stroke = Color.FromArgb("#DDD6FE"),
                StrokeThickness = 1,
                Padding = new Thickness(10, 4),
                Content = new Label
                {
                    Text = v.VisitType.Split(' ')[0],
                    TextColor = Color.FromArgb("#7C3AED"),
                    FontSize = 10,
                    FontAttributes = FontAttributes.Bold
                }
            };
            Grid.SetColumn(badge, 1);
            topRow.Children.Add(badge);

            stack.Children.Add(topRow);

            stack.Children.Add(new Label
            {
                Text = $"👥 Siswa yang Dikunjungi: {v.StudentsMet}",
                FontSize = 12,
                TextColor = Color.FromArgb("#2563EB"),
                FontAttributes = FontAttributes.Bold
            });

            stack.Children.Add(new Label
            {
                Text = $"📝 Catatan Supervisi: {v.SupervisionNotes}",
                FontSize = 12,
                TextColor = Color.FromArgb("#334155")
            });

            if (!string.IsNullOrWhiteSpace(v.IndustryFeedback))
            {
                stack.Children.Add(new Label
                {
                    Text = $"🏢 Masukan DUDI: \"{v.IndustryFeedback}\"",
                    FontSize = 11,
                    TextColor = Color.FromArgb("#0D9488"),
                    FontAttributes = FontAttributes.Italic
                });
            }

            if (!string.IsNullOrWhiteSpace(v.FollowUpRecommendations))
            {
                stack.Children.Add(new Label
                {
                    Text = $"💡 Rekomendasi Tindak Lanjut: {v.FollowUpRecommendations}",
                    FontSize = 11,
                    TextColor = Color.FromArgb("#475569")
                });
            }

            var footerGrid = new Grid
            {
                ColumnDefinitions =
                [
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                ]
            };

            var evalLabel = new Label
            {
                Text = $"Hasil Evaluasi: {v.OverallAssessment}",
                FontSize = 11,
                TextColor = Color.FromArgb("#10B981"),
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.Center
            };
            Grid.SetColumn(evalLabel, 0);
            footerGrid.Children.Add(evalLabel);

            var btnStack = new HorizontalStackLayout { Spacing = 6 };

            var docBtn = new Button
            {
                Text = "Lihat Berkas",
                BackgroundColor = Color.FromArgb("#EFF6FF"),
                TextColor = Color.FromArgb("#2563EB"),
                FontSize = 10,
                FontAttributes = FontAttributes.Bold,
                HeightRequest = 30,
                Padding = new Thickness(10, 0),
                CornerRadius = 6
            };
            var capturedVisit = v;
            docBtn.Clicked += async (s, e) =>
            {
                await DisplayAlertAsync("Dokumentasi Kunjungan Monitoring",
                    $"Perusahaan: {capturedVisit.CompanyName}\nTanggal: {capturedVisit.VisitDateFormatted}\nPembimbing: {capturedVisit.GuruName}\nSiswa: {capturedVisit.StudentsMet}\nLampiran Berkas: {capturedVisit.DocumentationProof}\n\nStatus Berita Acara: Lengkap & Terverifikasi Sekolah.",
                    "Tutup");
            };
            btnStack.Children.Add(docBtn);

            // FR-VST-02: Memperbarui hasil kunjungan (Catatan diskusi & foto dokumentasi WAJIB)
            if (_currentUser?.Role == UserRole.GuruPendamping || _currentUser?.Role == UserRole.Admin)
            {
                var updateBtn = new Button
                {
                    Text = "✏️ Update Hasil",
                    BackgroundColor = Color.FromArgb("#F5F3FF"),
                    TextColor = Color.FromArgb("#7C3AED"),
                    FontSize = 10,
                    FontAttributes = FontAttributes.Bold,
                    HeightRequest = 30,
                    Padding = new Thickness(8, 0),
                    CornerRadius = 6
                };
                updateBtn.Clicked += async (s, e) =>
                {
                    var newNotes = await DisplayPromptAsync("Update Catatan Kunjungan (FR-VST-02)",
                        "Catatan diskusi hasil supervisi (WAJIB diisi):",
                        initialValue: capturedVisit.SupervisionNotes);
                    if (newNotes == null) return;
                    if (string.IsNullOrWhiteSpace(newNotes))
                    {
                        ShowBanner("Catatan diskusi supervisi WAJIB diisi!", isError: true);
                        return;
                    }

                    var proof = await DisplayActionSheetAsync("Unggah Foto Dokumentasi Kunjungan (WAJIB)", "Batal", null,
                        "foto_lapangan_supervisi.jpg",
                        "berita_acara_monev_signed.pdf",
                        "tangkapan_layar_zoom_meeting.png");
                    if (proof == null || proof == "Batal")
                    {
                        ShowBanner("Foto dokumentasi kunjungan WAJIB diunggah!", isError: true);
                        return;
                    }

                    var (success, msg) = _dataService.UpdateVisitResult(
                        capturedVisit.Id, newNotes, proof, capturedVisit.IndustryFeedback, capturedVisit.FollowUpRecommendations, capturedVisit.OverallAssessment);
                    ShowBanner(msg, isError: !success);
                    RefreshAll();
                };
                btnStack.Children.Add(updateBtn);
            }

            Grid.SetColumn(btnStack, 1);
            footerGrid.Children.Add(btnStack);

            stack.Children.Add(footerGrid);
            card.Content = stack;
            VisitsContainer.Children.Add(card);
        }
    }

    private void OnSubmitVisitClicked(object? sender, EventArgs e)
    {
        var company = CompanyPicker.SelectedItem?.ToString() ?? string.Empty;
        var visitType = VisitTypePicker.SelectedItem?.ToString() ?? "Onsite (Kunjungan Langsung ke DUDI)";
        var students = StudentsMetEntry.Text?.Trim() ?? string.Empty;
        var notes = SupervisionNotesEditor.Text?.Trim() ?? string.Empty;
        var industryFeedback = IndustryFeedbackEditor.Text?.Trim() ?? string.Empty;
        var followUp = FollowUpEditor.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(company))
        {
            ShowBanner("Pilih perusahaan mitra DUDI yang dikunjungi.", isError: true);
            return;
        }

        if (string.IsNullOrWhiteSpace(students))
        {
            ShowBanner("Daftar nama siswa yang ditemui wajib diisi.", isError: true);
            StudentsMetEntry.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(notes))
        {
            ShowBanner("Catatan hasil monitoring guru supervisi wajib diisi.", isError: true);
            SupervisionNotesEditor.Focus();
            return;
        }

        var guruName = _currentUser?.FullName ?? "Drs. Bambang Hidayat, M.Kom";
        var guruId = _currentUser?.Id ?? "USR-002";

        var newVisit = new SupervisionVisitRecord
        {
            GuruId = guruId,
            GuruName = guruName,
            CompanyName = company,
            VisitDate = DateTime.Today,
            VisitType = visitType,
            StudentsMet = students,
            SupervisionNotes = notes,
            IndustryFeedback = industryFeedback,
            FollowUpRecommendations = followUp,
            DocumentationProof = VisitProofLabel.Text.Split(' ')[0],
            OverallAssessment = "Sangat Baik (A)"
        };

        var (success, msg) = _dataService.AddVisit(newVisit);
        ShowBanner(msg, isError: !success);

        // Reset form
        StudentsMetEntry.Text = string.Empty;
        SupervisionNotesEditor.Text = string.Empty;
        IndustryFeedbackEditor.Text = string.Empty;
        FollowUpEditor.Text = string.Empty;

        SwitchTab(0);
        RefreshAll();
    }

    private async void OnChooseVisitProofClicked(object? sender, EventArgs e)
    {
        var chosen = await DisplayActionSheetAsync("Pilih Dokumentasi Kunjungan", "Batal", null,
            "Foto Lapangan Bersama Siswa & Mentor",
            "Berita Acara Kunjungan Bertandatangan (PDF)",
            "Tangkapan Layar Virtual Meeting (Daring)");

        if (!string.IsNullOrWhiteSpace(chosen) && chosen != "Batal")
        {
            VisitProofLabel.Text = $"{chosen.Replace(" ", "_").ToLower()}_{DateTime.Now:HHmmss}.jpg (2.3 MB)";
        }
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
        SectionList.IsVisible = index == 0;
        SectionAdd.IsVisible = index == 1;
        SectionMonev.IsVisible = index == 2;

        TabListBtn.BackgroundColor = index == 0 ? Color.FromArgb("#1E3A5F") : Color.FromArgb("#F1F5F9");
        TabListBtn.TextColor = index == 0 ? Colors.White : Color.FromArgb("#475569");

        TabAddBtn.BackgroundColor = index == 1 ? Color.FromArgb("#1E3A5F") : Color.FromArgb("#F1F5F9");
        TabAddBtn.TextColor = index == 1 ? Colors.White : Color.FromArgb("#475569");

        TabMonevBtn.BackgroundColor = index == 2 ? Color.FromArgb("#1E3A5F") : Color.FromArgb("#F1F5F9");
        TabMonevBtn.TextColor = index == 2 ? Colors.White : Color.FromArgb("#475569");
    }

    private void OnTabListClicked(object? sender, EventArgs e) => SwitchTab(0);
    private void OnTabAddClicked(object? sender, EventArgs e) => SwitchTab(1);
    private void OnTabMonevClicked(object? sender, EventArgs e) => SwitchTab(2);

    private async void OnBackClicked(object? sender, EventArgs e)
    {
        if (Navigation.ModalStack.Count > 0)
        {
            await Navigation.PopModalAsync();
        }
        else
        {
            var role = _currentUser?.Role ?? UserRole.GuruPendamping;
            await Shell.Current.GoToAsync($"//{RoleHelper.GetDashboardRoute(role)}");
        }
    }
}
