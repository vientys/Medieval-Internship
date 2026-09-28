using Medieval_Internship.Models;
using Medieval_Internship.Services;
using Microsoft.Maui.Controls.Shapes;

namespace Medieval_Internship.Views.Modules;

public partial class JurnalHarianModulePage : ContentPage
{
    private readonly AuthService _authService = AuthService.Instance;
    private readonly PklDataService _dataService = PklDataService.Instance;
    private UserModel? _currentUser;

    public JurnalHarianModulePage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _currentUser = _authService.CurrentUser;
        if (_currentUser == null) return;

        UserRoleSubtitle.Text = $"{_currentUser.FullName} ({_currentUser.RoleName}) • {_currentUser.OrganizationOrSchool}";

        if (_currentUser.Role == UserRole.GuruPendamping || _currentUser.Role == UserRole.PembimbingIndustri)
        {
            SwitchTab(2); // Review tab
        }
        else
        {
            SwitchTab(0); // List tab
        }

        RefreshAll();
    }

    private void RefreshAll()
    {
        var studentId = _currentUser?.Role == UserRole.Siswa ? _currentUser.Id : "";
        var journals = string.IsNullOrEmpty(studentId)
            ? _dataService.GetAllJournals()
            : _dataService.GetJournalsByStudent(studentId);

        RenderJournalsList(journals);
        RenderReviewList();
    }

    private void RenderJournalsList(List<DailyJournalRecord> list)
    {
        JournalsContainer.Children.Clear();

        if (list.Count == 0)
        {
            JournalsContainer.Children.Add(new Label
            {
                Text = "Belum ada entri jurnal yang dibuat.",
                TextColor = Color.FromArgb("#64748B"),
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 20)
            });
            return;
        }

        foreach (var j in list)
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

            var titleStack = new VerticalStackLayout { Spacing = 2 };
            titleStack.Children.Add(new Label
            {
                Text = j.ActivityTitle,
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#0F172A")
            });
            titleStack.Children.Add(new Label
            {
                Text = $"{j.StudentName} • {j.CompanyName} • {j.DateFormatted}",
                FontSize = 11,
                TextColor = Color.FromArgb("#64748B")
            });
            Grid.SetColumn(titleStack, 0);
            topRow.Children.Add(titleStack);

            var badge = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(8) },
                BackgroundColor = Color.FromArgb(j.StatusBadgeColor),
                Padding = new Thickness(10, 4),
                Content = new Label
                {
                    Text = j.StatusText,
                    TextColor = Colors.White,
                    FontSize = 10,
                    FontAttributes = FontAttributes.Bold
                }
            };
            Grid.SetColumn(badge, 1);
            topRow.Children.Add(badge);
            stack.Children.Add(topRow);

            stack.Children.Add(new Label
            {
                Text = j.WorkDescription,
                FontSize = 12,
                TextColor = Color.FromArgb("#334155")
            });

            if (!string.IsNullOrWhiteSpace(j.ToolsUsed))
            {
                stack.Children.Add(new Label
                {
                    Text = $"🛠️ Teknologi: {j.ToolsUsed}",
                    FontSize = 11,
                    TextColor = Color.FromArgb("#2563EB")
                });
            }

            if (!string.IsNullOrWhiteSpace(j.Obstacles))
            {
                stack.Children.Add(new Label
                {
                    Text = $"⚠️ Kendala: {j.Obstacles}\n💡 Solusi: {j.Solutions}",
                    FontSize = 11,
                    TextColor = Color.FromArgb("#475569")
                });
            }

            // Bukti kegiatan & feedback mentor
            var footerGrid = new Grid
            {
                ColumnDefinitions =
                [
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                ]
            };

            var feedbackLabel = new Label
            {
                Text = !string.IsNullOrWhiteSpace(j.MentorFeedback)
                    ? $"💬 Catatan Mentor: \"{j.MentorFeedback}\""
                    : "Belum ada catatan mentor",
                FontSize = 11,
                TextColor = Color.FromArgb("#10B981"),
                FontAttributes = FontAttributes.Italic
            };
            Grid.SetColumn(feedbackLabel, 0);
            footerGrid.Children.Add(feedbackLabel);

            var proofBorder = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(6) },
                BackgroundColor = Color.FromArgb("#F1F5F9"),
                Padding = new Thickness(8, 2),
                Content = new Label
                {
                    Text = $"📎 {j.AttachmentProof}",
                    FontSize = 10,
                    TextColor = Color.FromArgb("#64748B")
                }
            };
            Grid.SetColumn(proofBorder, 1);
            footerGrid.Children.Add(proofBorder);

            stack.Children.Add(footerGrid);

            // FR-DJL-01: Siswa dapat menghapus jurnal harian yang telah dibuat dengan dialog konfirmasi
            if (_currentUser?.Role == UserRole.Siswa || _currentUser?.Role == UserRole.Admin)
            {
                var actionRow = new HorizontalStackLayout { HorizontalOptions = LayoutOptions.End, Margin = new Thickness(0, 4, 0, 0) };
                var delBtn = new Button
                {
                    Text = "🗑️ Hapus Jurnal",
                    BackgroundColor = Color.FromArgb("#FEE2E2"),
                    TextColor = Color.FromArgb("#DC2626"),
                    FontSize = 11,
                    HeightRequest = 30,
                    Padding = new Thickness(10, 0),
                    CornerRadius = 6
                };
                var capturedJournal = j;
                delBtn.Clicked += async (s, e) =>
                {
                    var confirm = await DisplayAlertAsync("Konfirmasi Hapus Jurnal (FR-DJL-01)",
                        $"Apakah Anda yakin ingin menghapus catatan jurnal \"{capturedJournal.ActivityTitle}\"?",
                        "Ya, Hapus", "Batal");
                    if (!confirm) return;

                    var (success, msg) = _dataService.DeleteJournal(capturedJournal.Id, _currentUser?.Id ?? "", _currentUser?.Role ?? UserRole.Siswa);
                    ShowBanner(msg, isError: !success);
                    RefreshAll();
                };
                actionRow.Children.Add(delBtn);
                stack.Children.Add(actionRow);
            }

            card.Content = stack;
            JournalsContainer.Children.Add(card);
        }
    }

    private void RenderReviewList()
    {
        ReviewListContainer.Children.Clear();
        var pending = _dataService.GetAllJournals()
            .Where(j => j.Status == JournalStatus.MenungguReview || j.Status == JournalStatus.PerluRevisi)
            .ToList();

        if (pending.Count == 0)
        {
            ReviewListContainer.Children.Add(new Label
            {
                Text = "Seluruh jurnal bimbingan telah diverifikasi! 🎉",
                TextColor = Color.FromArgb("#059669"),
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 20)
            });
            return;
        }

        foreach (var j in pending)
        {
            var card = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(14) },
                BackgroundColor = Colors.White,
                Stroke = Color.FromArgb("#CBD5E1"),
                StrokeThickness = 1,
                Padding = new Thickness(16)
            };

            var stack = new VerticalStackLayout { Spacing = 8 };

            stack.Children.Add(new Label
            {
                Text = j.ActivityTitle,
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#0F172A")
            });

            stack.Children.Add(new Label
            {
                Text = $"Siswa: {j.StudentName} • {j.CompanyName} • {j.DateFormatted}",
                FontSize = 11,
                TextColor = Color.FromArgb("#64748B")
            });

            stack.Children.Add(new Label
            {
                Text = $"Rincian: {j.WorkDescription}\nTeknologi: {j.ToolsUsed}\nBukti Lampiran: {j.AttachmentProof}",
                FontSize = 12,
                TextColor = Color.FromArgb("#334155")
            });

            // Action Buttons
            var btnRow = new HorizontalStackLayout { Spacing = 8, Margin = new Thickness(0, 4, 0, 0) };

            var approveBtn = new Button
            {
                Text = "✓ Setujui Jurnal",
                BackgroundColor = Color.FromArgb("#10B981"),
                TextColor = Colors.White,
                FontSize = 11,
                FontAttributes = FontAttributes.Bold,
                HeightRequest = 32,
                Padding = new Thickness(12, 0),
                CornerRadius = 6
            };
            var capturedJournal = j;
            approveBtn.Clicked += async (s, e) =>
            {
                var role = _currentUser?.Role ?? UserRole.PembimbingIndustri;
                var targetStatus = role == UserRole.PembimbingIndustri
                    ? JournalStatus.DisetujuiIndustri
                    : JournalStatus.DisetujuiPenuh;

                var (success, msg) = _dataService.ReviewJournal(
                    capturedJournal.Id, role, targetStatus, "Pekerjaan terdokumentasi dengan baik dan sesuai target sprint.");
                ShowBanner(msg, isError: !success);
                RefreshAll();
            };
            btnRow.Children.Add(approveBtn);

            var reviseBtn = new Button
            {
                Text = "✏️ Minta Revisi",
                BackgroundColor = Color.FromArgb("#6366F1"),
                TextColor = Colors.White,
                FontSize = 11,
                HeightRequest = 32,
                Padding = new Thickness(10, 0),
                CornerRadius = 6
            };
            reviseBtn.Clicked += async (s, e) =>
            {
                var feedback = await DisplayPromptAsync("Catatan Revisi Jurnal", "Tuliskan bagian yang perlu diperbaiki / dilengkapi:");
                if (!string.IsNullOrWhiteSpace(feedback))
                {
                    var role = _currentUser?.Role ?? UserRole.PembimbingIndustri;
                    var (success, msg) = _dataService.ReviewJournal(
                        capturedJournal.Id, role, JournalStatus.PerluRevisi, feedback);
                    ShowBanner(msg, isError: !success);
                    RefreshAll();
                }
            };
            btnRow.Children.Add(reviseBtn);

            stack.Children.Add(btnRow);
            card.Content = stack;
            ReviewListContainer.Children.Add(card);
        }
    }

    private void OnSubmitJournalClicked(object? sender, EventArgs e)
    {
        var title = JournalTitleEntry.Text?.Trim() ?? string.Empty;
        var tools = JournalToolsEntry.Text?.Trim() ?? string.Empty;
        var desc = JournalDescEditor.Text?.Trim() ?? string.Empty;
        var obst = JournalObstaclesEditor.Text?.Trim() ?? string.Empty;
        var sol = JournalSolutionEditor.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(title))
        {
            ShowBanner("Judul aktivitas pekerjaan hari ini wajib diisi.", isError: true);
            JournalTitleEntry.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(desc))
        {
            ShowBanner("Rincian dokumentasi pekerjaan wajib diisi.", isError: true);
            JournalDescEditor.Focus();
            return;
        }

        var studentId = _currentUser?.Id ?? "USR-001";
        var studentName = _currentUser?.FullName ?? "Rizky Pratama";
        var companyName = _currentUser?.OrganizationOrSchool ?? "PT Telkom Indonesia";

        var newJournal = new DailyJournalRecord
        {
            StudentId = studentId,
            StudentName = studentName,
            CompanyName = companyName,
            ActivityTitle = title,
            ToolsUsed = tools,
            WorkDescription = desc,
            Obstacles = obst,
            Solutions = sol,
            AttachmentProof = AttachmentLabel.Text.Split(' ')[0]
        };

        var (success, msg, _) = _dataService.AddJournal(newJournal);
        ShowBanner(msg, isError: !success);

        // Reset form
        JournalTitleEntry.Text = string.Empty;
        JournalToolsEntry.Text = string.Empty;
        JournalDescEditor.Text = string.Empty;
        JournalObstaclesEditor.Text = string.Empty;
        JournalSolutionEditor.Text = string.Empty;

        SwitchTab(0);
        RefreshAll();
    }

    private async void OnChooseAttachmentClicked(object? sender, EventArgs e)
    {
        var chosen = await DisplayActionSheetAsync("Pilih Bukti Kegiatan", "Batal", null,
            "Foto Kamera (Selfie Lapangan)",
            "Tangkapan Layar Kode/UI (Screenshot)",
            "Dokumen Berita Acara / Laporan PDF");

        if (!string.IsNullOrWhiteSpace(chosen) && chosen != "Batal")
        {
            AttachmentLabel.Text = $"{chosen.Replace(" ", "_").ToLower()}_{DateTime.Now:HHmmss}.png (1.4 MB)";
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
        SectionWrite.IsVisible = index == 1;
        SectionReview.IsVisible = index == 2;

        TabListBtn.BackgroundColor = index == 0 ? Color.FromArgb("#1E3A5F") : Color.FromArgb("#F1F5F9");
        TabListBtn.TextColor = index == 0 ? Colors.White : Color.FromArgb("#475569");

        TabWriteBtn.BackgroundColor = index == 1 ? Color.FromArgb("#1E3A5F") : Color.FromArgb("#F1F5F9");
        TabWriteBtn.TextColor = index == 1 ? Colors.White : Color.FromArgb("#475569");

        TabReviewBtn.BackgroundColor = index == 2 ? Color.FromArgb("#1E3A5F") : Color.FromArgb("#F1F5F9");
        TabReviewBtn.TextColor = index == 2 ? Colors.White : Color.FromArgb("#475569");
    }

    private void OnTabListClicked(object? sender, EventArgs e) => SwitchTab(0);
    private void OnTabWriteClicked(object? sender, EventArgs e) => SwitchTab(1);
    private void OnTabReviewClicked(object? sender, EventArgs e) => SwitchTab(2);

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
