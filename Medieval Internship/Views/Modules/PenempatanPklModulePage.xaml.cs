using Medieval_Internship.Models;
using Medieval_Internship.Services;
using Microsoft.Maui.Controls.Shapes;

namespace Medieval_Internship.Views.Modules;

public partial class PenempatanPklModulePage : ContentPage
{
    private readonly AuthService _authService = AuthService.Instance;
    private readonly PklDataService _dataService = PklDataService.Instance;
    private UserModel? _currentUser;

    public PenempatanPklModulePage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _currentUser = _authService.CurrentUser;
        if (_currentUser == null) return;

        UserRoleSubtitle.Text = $"{_currentUser.FullName} ({_currentUser.RoleName}) • {_currentUser.OrganizationOrSchool}";
        SwitchTab(0);
        RefreshAll();
    }

    private void RefreshAll()
    {
        RenderPlotting();
        RenderPeriods();
        RenderTracking();
    }

    private void RenderPlotting()
    {
        PlottingContainer.Children.Clear();
        var students = _dataService.GetStudents();

        foreach (var s in students)
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
                Text = s.FullName,
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#0F172A")
            });
            stack.Children.Add(new Label
            {
                Text = $"NISN: {s.Nisn} • Kelas: {s.ClassName} ({s.Major})",
                FontSize = 11,
                TextColor = Color.FromArgb("#64748B")
            });
            stack.Children.Add(new Label
            {
                Text = $"🏢 Perusahaan DUDI: {s.CompanyName}",
                FontSize = 12,
                TextColor = Color.FromArgb("#2563EB"),
                FontAttributes = FontAttributes.Bold
            });
            stack.Children.Add(new Label
            {
                Text = $"👨‍🏫 Guru Supervisor: {s.InternalMentorName}\n💼 Mentor Industri: {s.ExternalMentorName}",
                FontSize = 11,
                TextColor = Color.FromArgb("#475569")
            });

            Grid.SetColumn(stack, 0);
            grid.Children.Add(stack);

            var assignBtn = new Button
            {
                Text = "Ubah Plotting",
                BackgroundColor = Color.FromArgb("#EFF6FF"),
                TextColor = Color.FromArgb("#2563EB"),
                FontSize = 11,
                FontAttributes = FontAttributes.Bold,
                HeightRequest = 34,
                Padding = new Thickness(12, 0),
                CornerRadius = 6,
                VerticalOptions = LayoutOptions.Center
            };
            var capturedStudent = s;
            assignBtn.Clicked += async (sender, e) =>
            {
                var companies = _dataService.GetCompanies().Select(c => c.Name).ToArray();
                var chosen = await DisplayActionSheetAsync("Pilih Perusahaan Mitra DUDI", "Batal", null, companies);
                if (!string.IsNullOrWhiteSpace(chosen) && chosen != "Batal")
                {
                    var comp = _dataService.GetCompanies().FirstOrDefault(c => c.Name == chosen);
                    if (comp != null)
                    {
                        var (success, msg) = _dataService.UpdateStudentAssignment(
                            capturedStudent.Id, comp.Id, "USR-002", capturedStudent.ExternalMentorName);
                        ShowBanner(msg, isError: !success);
                        RefreshAll();
                    }
                }
            };
            Grid.SetColumn(assignBtn, 1);
            grid.Children.Add(assignBtn);

            card.Content = grid;
            PlottingContainer.Children.Add(card);
        }
    }

    private void RenderPeriods()
    {
        PeriodsContainer.Children.Clear();
        var periods = _dataService.GetPeriods();

        foreach (var p in periods)
        {
            var card = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(12) },
                BackgroundColor = Colors.White,
                Stroke = p.IsActive ? Color.FromArgb("#10B981") : Color.FromArgb("#E2E8F0"),
                StrokeThickness = p.IsActive ? 2 : 1,
                Padding = new Thickness(16, 14)
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
                Text = p.Name,
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#0F172A")
            });
            stack.Children.Add(new Label
            {
                Text = $"🗓️ Rentang Waktu: {p.DateRangeFormatted}",
                FontSize = 12,
                TextColor = Color.FromArgb("#334155")
            });
            stack.Children.Add(new Label
            {
                Text = p.IsActive ? "Status: AKTIF BERJALAN" : "Status: Tidak Aktif / Arsip",
                FontSize = 11,
                TextColor = p.IsActive ? Color.FromArgb("#10B981") : Color.FromArgb("#64748B"),
                FontAttributes = FontAttributes.Bold
            });

            Grid.SetColumn(stack, 0);
            grid.Children.Add(stack);

            var toggleBtn = new Button
            {
                Text = p.IsActive ? "Nonaktifkan" : "Aktifkan",
                BackgroundColor = p.IsActive ? Color.FromArgb("#F1F5F9") : Color.FromArgb("#10B981"),
                TextColor = p.IsActive ? Color.FromArgb("#475569") : Colors.White,
                FontSize = 11,
                HeightRequest = 32,
                Padding = new Thickness(12, 0),
                CornerRadius = 6,
                VerticalOptions = LayoutOptions.Center
            };
            var capturedPeriod = p;
            toggleBtn.Clicked += (sender, e) =>
            {
                capturedPeriod.IsActive = !capturedPeriod.IsActive;
                ShowBanner($"Status periode '{capturedPeriod.Name}' diubah menjadi: {(capturedPeriod.IsActive ? "Aktif" : "Non-aktif")}.", isError: false);
                RefreshAll();
            };
            Grid.SetColumn(toggleBtn, 1);
            grid.Children.Add(toggleBtn);

            card.Content = grid;
            PeriodsContainer.Children.Add(card);
        }
    }

    private void RenderTracking()
    {
        TrackingContainer.Children.Clear();
        var students = _dataService.GetStudents();

        foreach (var s in students)
        {
            var card = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(12) },
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
                Text = s.FullName,
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#0F172A")
            });
            info.Children.Add(new Label
            {
                Text = $"{s.ClassName} • {s.CompanyName}",
                FontSize = 11,
                TextColor = Color.FromArgb("#64748B")
            });
            Grid.SetColumn(info, 0);
            topRow.Children.Add(info);

            var rateBadge = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(8) },
                BackgroundColor = Color.FromArgb("#ECFDF5"),
                Padding = new Thickness(10, 3),
                Content = new Label
                {
                    Text = $"Kehadiran: {s.AttendanceRate:F1}%",
                    TextColor = Color.FromArgb("#059669"),
                    FontSize = 11,
                    FontAttributes = FontAttributes.Bold
                }
            };
            Grid.SetColumn(rateBadge, 1);
            topRow.Children.Add(rateBadge);
            stack.Children.Add(topRow);

            var progGrid = new Grid
            {
                ColumnDefinitions =
                [
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                ]
            };
            progGrid.Children.Add(new ProgressBar
            {
                Progress = 0.5,
                ProgressColor = Color.FromArgb("#10B981"),
                HeightRequest = 6,
                VerticalOptions = LayoutOptions.Center
            });
            var progLabel = new Label
            {
                Text = $"{s.JournalCount} Jurnal Terverifikasi",
                FontSize = 11,
                TextColor = Color.FromArgb("#2563EB"),
                FontAttributes = FontAttributes.Bold,
                Margin = new Thickness(12, 0, 0, 0),
                VerticalOptions = LayoutOptions.Center
            };
            Grid.SetColumn(progLabel, 1);
            progGrid.Children.Add(progLabel);
            stack.Children.Add(progGrid);

            card.Content = stack;
            TrackingContainer.Children.Add(card);
        }
    }

    private async void OnAddPeriodPromptClicked(object? sender, EventArgs e)
    {
        var name = await DisplayPromptAsync("Tambah Periode PKL", "Nama Periode / Gelombang:", "Contoh: Gelombang 3 TA 2026/2027");
        if (string.IsNullOrWhiteSpace(name)) return;

        var newPeriod = new PklPeriodModel
        {
            Id = $"GEL-{Guid.NewGuid().ToString()[..4].ToUpper()}",
            Name = name,
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddMonths(3),
            IsActive = false
        };

        _dataService.AddPeriod(newPeriod);
        ShowBanner($"Periode '{name}' berhasil didaftarkan.", isError: false);
        RefreshAll();
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
        SectionPlotting.IsVisible = index == 0;
        SectionPeriods.IsVisible = index == 1;
        SectionTracking.IsVisible = index == 2;

        TabPlottingBtn.BackgroundColor = index == 0 ? Color.FromArgb("#1E3A5F") : Color.FromArgb("#F1F5F9");
        TabPlottingBtn.TextColor = index == 0 ? Colors.White : Color.FromArgb("#475569");

        TabPeriodsBtn.BackgroundColor = index == 1 ? Color.FromArgb("#1E3A5F") : Color.FromArgb("#F1F5F9");
        TabPeriodsBtn.TextColor = index == 1 ? Colors.White : Color.FromArgb("#475569");

        TabTrackingBtn.BackgroundColor = index == 2 ? Color.FromArgb("#1E3A5F") : Color.FromArgb("#F1F5F9");
        TabTrackingBtn.TextColor = index == 2 ? Colors.White : Color.FromArgb("#475569");
    }

    private void OnTabPlottingClicked(object? sender, EventArgs e) => SwitchTab(0);
    private void OnTabPeriodsClicked(object? sender, EventArgs e) => SwitchTab(1);
    private void OnTabTrackingClicked(object? sender, EventArgs e) => SwitchTab(2);

    private async void OnBackClicked(object? sender, EventArgs e)
    {
        if (Navigation.ModalStack.Count > 0)
        {
            await Navigation.PopModalAsync();
        }
        else
        {
            var role = _currentUser?.Role ?? UserRole.Operator;
            await Shell.Current.GoToAsync($"//{RoleHelper.GetDashboardRoute(role)}");
        }
    }
}
