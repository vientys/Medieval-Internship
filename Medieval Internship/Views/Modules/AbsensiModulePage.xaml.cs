using Medieval_Internship.Models;
using Medieval_Internship.Services;
using Microsoft.Maui.Controls.Shapes;

namespace Medieval_Internship.Views.Modules;

public partial class AbsensiModulePage : ContentPage
{
    private readonly AuthService _authService = AuthService.Instance;
    private readonly PklDataService _dataService = PklDataService.Instance;
    private UserModel? _currentUser;

    public AbsensiModulePage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _currentUser = _authService.CurrentUser;
        if (_currentUser == null) return;

        UserRoleSubtitle.Text = $"{_currentUser.FullName} ({_currentUser.RoleName}) • {_currentUser.OrganizationOrSchool}";
        LiveTimeLabel.Text = DateTime.Now.ToString("HH:mm") + " WIB";
        TodayDateLabel.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy");

        // Role-based default tab selection
        if (_currentUser.Role == UserRole.GuruPendamping || _currentUser.Role == UserRole.PembimbingIndustri)
        {
            SwitchTab(2); // Validation tab
        }
        else
        {
            SwitchTab(0); // Check-in tab
        }

        RefreshAllSections();
    }

    private void RefreshAllSections()
    {
        var studentId = _currentUser?.Role == UserRole.Siswa ? _currentUser.Id : "USR-001";
        var history = _dataService.GetAttendanceHistory(studentId);

        // 1. Today Status
        var today = history.FirstOrDefault(a => a.Date.Date == DateTime.Today);
        if (today != null)
        {
            TodaySummaryLabel.Text = $"Status: {today.Status} • Masuk: {today.CheckInTime} • Pulang: {today.CheckOutTime}\nLokasi: {today.LocationName}\nValidasi: {today.ValidationSummary}";
        }
        else
        {
            TodaySummaryLabel.Text = "Belum ada catatan presensi hari ini. Silakan pilih status dan klik 'Presensi Masuk'.";
        }

        // 2. History List
        RenderHistoryList(history);

        // 3. Validation List
        RenderValidationList();
    }

    private void RenderHistoryList(List<AttendanceRecord> list)
    {
        HistoryCardsContainer.Children.Clear();

        if (list.Count == 0)
        {
            HistoryCardsContainer.Children.Add(new Label
            {
                Text = "Belum ada riwayat presensi.",
                TextColor = Color.FromArgb("#64748B"),
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 20)
            });
            return;
        }

        foreach (var item in list)
        {
            var card = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(12) },
                BackgroundColor = Colors.White,
                Stroke = Color.FromArgb("#E2E8F0"),
                StrokeThickness = 1,
                Padding = new Thickness(16, 12)
            };

            var grid = new Grid
            {
                ColumnDefinitions =
                [
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                ]
            };

            var stack = new VerticalStackLayout { Spacing = 3 };
            stack.Children.Add(new Label
            {
                Text = $"{item.DateFormatted} • {item.StudentName}",
                FontSize = 13,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#0F172A")
            });
            stack.Children.Add(new Label
            {
                Text = $"Jam Masuk: {item.CheckInTime} | Jam Pulang: {item.CheckOutTime}",
                FontSize = 11,
                TextColor = Color.FromArgb("#334155")
            });
            stack.Children.Add(new Label
            {
                Text = $"📍 {item.LocationName}",
                FontSize = 11,
                TextColor = Color.FromArgb("#64748B")
            });

            if (!string.IsNullOrWhiteSpace(item.Notes))
            {
                stack.Children.Add(new Label
                {
                    Text = $"Catatan: \"{item.Notes}\"",
                    FontSize = 11,
                    TextColor = Color.FromArgb("#475569"),
                    FontAttributes = FontAttributes.Italic
                });
            }

            stack.Children.Add(new Label
            {
                Text = $"Validasi: {item.ValidationSummary}",
                FontSize = 10,
                TextColor = Color.FromArgb("#2563EB"),
                FontAttributes = FontAttributes.Bold,
                Margin = new Thickness(0, 2, 0, 0)
            });

            Grid.SetColumn(stack, 0);
            grid.Children.Add(stack);

            var badge = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(8) },
                BackgroundColor = Color.FromArgb(item.StatusBadgeColor),
                Padding = new Thickness(10, 4),
                VerticalOptions = LayoutOptions.Start,
                Content = new Label
                {
                    Text = item.Status.ToString(),
                    TextColor = Colors.White,
                    FontSize = 11,
                    FontAttributes = FontAttributes.Bold
                }
            };
            Grid.SetColumn(badge, 1);
            grid.Children.Add(badge);

            card.Content = grid;
            HistoryCardsContainer.Children.Add(card);
        }
    }

    private void RenderValidationList()
    {
        ValidationListContainer.Children.Clear();

        var pendingList = _currentUser?.Role == UserRole.PembimbingIndustri
            ? _dataService.GetPendingAttendanceForIndustri(_currentUser.OrganizationOrSchool)
            : _dataService.GetPendingAttendanceForGuru(_currentUser?.Id ?? "");

        if (pendingList.Count == 0)
        {
            ValidationListContainer.Children.Add(new Label
            {
                Text = "Semua presensi siswa bimbingan telah divalidasi. Tidak ada yang tertunda 🎉",
                TextColor = Color.FromArgb("#059669"),
                FontSize = 13,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 20)
            });
            return;
        }

        foreach (var item in pendingList)
        {
            var card = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(12) },
                BackgroundColor = Colors.White,
                Stroke = Color.FromArgb("#E2E8F0"),
                StrokeThickness = 1,
                Padding = new Thickness(16)
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

            var studentInfo = new VerticalStackLayout { Spacing = 2 };
            studentInfo.Children.Add(new Label
            {
                Text = item.StudentName,
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#0F172A")
            });
            studentInfo.Children.Add(new Label
            {
                Text = $"{item.ClassName} • {item.CompanyName} • {item.DateFormatted}",
                FontSize = 11,
                TextColor = Color.FromArgb("#64748B")
            });
            Grid.SetColumn(studentInfo, 0);
            topRow.Children.Add(studentInfo);

            var statusBadge = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(8) },
                BackgroundColor = Color.FromArgb(item.StatusBadgeColor),
                Padding = new Thickness(8, 2),
                Content = new Label
                {
                    Text = item.Status.ToString(),
                    TextColor = Colors.White,
                    FontSize = 10,
                    FontAttributes = FontAttributes.Bold
                }
            };
            Grid.SetColumn(statusBadge, 1);
            topRow.Children.Add(statusBadge);

            stack.Children.Add(topRow);

            stack.Children.Add(new Label
            {
                Text = $"Masuk: {item.CheckInTime} | Pulang: {item.CheckOutTime}\nLokasi: {item.LocationName}\nCatatan: {item.Notes}",
                FontSize = 11,
                TextColor = Color.FromArgb("#334155")
            });

            // Action Buttons
            var btnRow = new HorizontalStackLayout { Spacing = 8, Margin = new Thickness(0, 4, 0, 0) };

            var approveBtn = new Button
            {
                Text = "✓ Setujui Kehadiran",
                BackgroundColor = Color.FromArgb("#10B981"),
                TextColor = Colors.White,
                FontSize = 11,
                FontAttributes = FontAttributes.Bold,
                HeightRequest = 32,
                Padding = new Thickness(12, 0),
                CornerRadius = 6
            };
            var capturedItem = item;
            approveBtn.Clicked += async (s, e) =>
            {
                var role = _currentUser?.Role ?? UserRole.GuruPendamping;
                var validatorName = _currentUser?.FullName ?? "Pembimbing";
                var (success, msg) = _dataService.ValidateAttendance(capturedItem.Id, role, true, validatorName, "Kehadiran memenuhi kriteria & divalidasi.");
                ShowBanner(msg, isError: !success);
                RefreshAllSections();
            };
            btnRow.Children.Add(approveBtn);

            var rejectBtn = new Button
            {
                Text = "✕ Tolak",
                BackgroundColor = Color.FromArgb("#EF4444"),
                TextColor = Colors.White,
                FontSize = 11,
                HeightRequest = 32,
                Padding = new Thickness(12, 0),
                CornerRadius = 6
            };
            rejectBtn.Clicked += async (s, e) =>
            {
                var reason = await DisplayPromptAsync("Tolak Presensi", "Alasan penolakan kehadiran siswa:");
                if (!string.IsNullOrWhiteSpace(reason))
                {
                    var role = _currentUser?.Role ?? UserRole.GuruPendamping;
                    var validatorName = _currentUser?.FullName ?? "Pembimbing";
                    var (success, msg) = _dataService.ValidateAttendance(capturedItem.Id, role, false, validatorName, reason);
                    ShowBanner(msg, isError: !success);
                    RefreshAllSections();
                }
            };
            btnRow.Children.Add(rejectBtn);

            var criteriaBtn = new Button
            {
                Text = "📊 Cek Kriteria PKL",
                BackgroundColor = Color.FromArgb("#EFF6FF"),
                TextColor = Color.FromArgb("#2563EB"),
                FontSize = 11,
                HeightRequest = 32,
                Padding = new Thickness(10, 0),
                CornerRadius = 6
            };
            criteriaBtn.Clicked += async (s, e) =>
            {
                var (eligible, rate, journals, notes) = _dataService.CheckStudentCriteriaEligibility(capturedItem.StudentId);
                await DisplayAlertAsync("Evaluasi Kriteria Siswa PKL",
                    $"Siswa: {capturedItem.StudentName}\n• Tingkat Kehadiran: {rate}%\n• Jurnal Disetujui: {journals} entri\n• Status: {(eligible ? "MEMENUHI SYARAT" : "BELUM MEMENUHI")}\n\nKeterangan:\n{notes}",
                    "Tutup");
            };
            btnRow.Children.Add(criteriaBtn);

            stack.Children.Add(btnRow);
            card.Content = stack;
            ValidationListContainer.Children.Add(card);
        }
    }

    private void OnSubmitCheckInClicked(object? sender, EventArgs e)
    {
        var studentId = _currentUser?.Id ?? "USR-001";
        var studentName = _currentUser?.FullName ?? "Rizky Pratama";
        var className = "XII RPL 1";
        var companyName = _currentUser?.OrganizationOrSchool ?? "PT Telkom Indonesia";

        var selectedStatusIndex = StatusPicker.SelectedIndex;
        var status = selectedStatusIndex switch
        {
            1 => AttendanceStatus.Izin,
            2 => AttendanceStatus.Sakit,
            _ => AttendanceStatus.Hadir
        };

        var location = LocationEntry.Text?.Trim() ?? "Telkom Landmark Tower (Valid GPS)";
        var notes = NotesEditor.Text?.Trim() ?? string.Empty;

        var (success, msg, record) = _dataService.RecordCheckIn(studentId, studentName, className, companyName, status, location, notes);
        ShowBanner(msg, isError: !success);
        RefreshAllSections();
    }

    private void OnSubmitCheckOutClicked(object? sender, EventArgs e)
    {
        var studentId = _currentUser?.Id ?? "USR-001";
        var notes = NotesEditor.Text?.Trim() ?? string.Empty;

        var (success, msg) = _dataService.RecordCheckOut(studentId, notes);
        ShowBanner(msg, isError: !success);
        RefreshAllSections();
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
        SectionCheckIn.IsVisible = index == 0;
        SectionHistory.IsVisible = index == 1;
        SectionValidation.IsVisible = index == 2;

        TabCheckInBtn.BackgroundColor = index == 0 ? Color.FromArgb("#1E3A5F") : Color.FromArgb("#F1F5F9");
        TabCheckInBtn.TextColor = index == 0 ? Colors.White : Color.FromArgb("#475569");

        TabHistoryBtn.BackgroundColor = index == 1 ? Color.FromArgb("#1E3A5F") : Color.FromArgb("#F1F5F9");
        TabHistoryBtn.TextColor = index == 1 ? Colors.White : Color.FromArgb("#475569");

        TabValidationBtn.BackgroundColor = index == 2 ? Color.FromArgb("#1E3A5F") : Color.FromArgb("#F1F5F9");
        TabValidationBtn.TextColor = index == 2 ? Colors.White : Color.FromArgb("#475569");
    }

    private void OnTabCheckInClicked(object? sender, EventArgs e) => SwitchTab(0);
    private void OnTabHistoryClicked(object? sender, EventArgs e) => SwitchTab(1);
    private void OnTabValidationClicked(object? sender, EventArgs e) => SwitchTab(2);

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
