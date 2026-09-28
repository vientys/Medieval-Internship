using Medieval_Internship.Models;
using Medieval_Internship.Services;
using Microsoft.Maui.Controls.Shapes;

namespace Medieval_Internship.Views.Modules;

public partial class PengumumanModulePage : ContentPage
{
    private readonly AuthService _authService = AuthService.Instance;
    private readonly PklDataService _dataService = PklDataService.Instance;
    private UserModel? _currentUser;

    public PengumumanModulePage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _currentUser = _authService.CurrentUser;
        if (_currentUser == null) return;

        UserRoleSubtitle.Text = $"{_currentUser.FullName} ({_currentUser.RoleName}) • {_currentUser.OrganizationOrSchool}";

        // Show/hide create tab button according to RBAC
        bool canCreate = _currentUser.Role == UserRole.Admin || _currentUser.Role == UserRole.Operator;
        TabCreateBtn.IsVisible = canCreate;
        HeaderCreateBtn.IsVisible = canCreate;

        SwitchTab(0);
        RefreshAll();
    }

    private void RefreshAll()
    {
        var role = _currentUser?.Role ?? UserRole.Siswa;
        var announcements = _dataService.GetAnnouncementsForRole(role);
        RenderAnnouncements(announcements);
    }

    private void RenderAnnouncements(List<AnnouncementRecord> list)
    {
        AnnouncementsContainer.Children.Clear();

        if (list.Count == 0)
        {
            AnnouncementsContainer.Children.Add(new Label
            {
                Text = "Belum ada pengumuman untuk kategori peran Anda.",
                TextColor = Color.FromArgb("#64748B"),
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 20)
            });
            return;
        }

        foreach (var a in list)
        {
            var card = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(14) },
                BackgroundColor = Colors.White,
                Stroke = a.IsPinned ? Color.FromArgb("#0D9488") : Color.FromArgb("#E2E8F0"),
                StrokeThickness = a.IsPinned ? 2 : 1,
                Padding = new Thickness(18, 16)
            };

            var stack = new VerticalStackLayout { Spacing = 10 };

            var topRow = new Grid
            {
                ColumnDefinitions =
                [
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                ]
            };

            var titleStack = new VerticalStackLayout { Spacing = 3 };
            var headerRow = new HorizontalStackLayout { Spacing = 6 };
            if (a.IsPinned)
            {
                headerRow.Children.Add(new Label { Text = "📌", FontSize = 13, VerticalOptions = LayoutOptions.Center });
            }
            headerRow.Children.Add(new Label
            {
                Text = a.Title,
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#0F172A")
            });
            titleStack.Children.Add(headerRow);

            titleStack.Children.Add(new Label
            {
                Text = $"Oleh: {a.AuthorName} • {a.DateFormatted} • Sasaran: {a.TargetAudience}",
                FontSize = 11,
                TextColor = Color.FromArgb("#64748B")
            });
            Grid.SetColumn(titleStack, 0);
            topRow.Children.Add(titleStack);

            var priorityBadge = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(8) },
                BackgroundColor = Color.FromArgb(a.PriorityBadgeColor),
                Padding = new Thickness(10, 4),
                Content = new Label
                {
                    Text = a.Priority.ToString(),
                    TextColor = Colors.White,
                    FontSize = 10,
                    FontAttributes = FontAttributes.Bold
                }
            };
            Grid.SetColumn(priorityBadge, 1);
            topRow.Children.Add(priorityBadge);

            stack.Children.Add(topRow);

            stack.Children.Add(new Label
            {
                Text = a.Content,
                FontSize = 13,
                TextColor = Color.FromArgb("#334155"),
                LineBreakMode = LineBreakMode.WordWrap
            });

            if (!string.IsNullOrWhiteSpace(a.AttachmentName))
            {
                var attachBorder = new Border
                {
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(8) },
                    BackgroundColor = Color.FromArgb("#F0FDFA"),
                    Stroke = Color.FromArgb("#CCFBF1"),
                    Padding = new Thickness(12, 8)
                };

                var attachGrid = new Grid
                {
                    ColumnDefinitions =
                    [
                        new ColumnDefinition { Width = GridLength.Star },
                        new ColumnDefinition { Width = GridLength.Auto }
                    ]
                };

                var attachInfo = new HorizontalStackLayout { Spacing = 8, VerticalOptions = LayoutOptions.Center };
                attachInfo.Children.Add(new Label { Text = "📎", FontSize = 14, VerticalOptions = LayoutOptions.Center });
                attachInfo.Children.Add(new Label
                {
                    Text = a.AttachmentName,
                    FontSize = 11,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.FromArgb("#0F766E"),
                    VerticalOptions = LayoutOptions.Center
                });
                Grid.SetColumn(attachInfo, 0);
                attachGrid.Children.Add(attachInfo);

                var openBtn = new Button
                {
                    Text = "Unduh Berkas",
                    BackgroundColor = Color.FromArgb("#0D9488"),
                    TextColor = Colors.White,
                    FontSize = 10,
                    HeightRequest = 28,
                    Padding = new Thickness(10, 0),
                    CornerRadius = 6
                };
                var capturedName = a.AttachmentName;
                openBtn.Clicked += async (s, e) =>
                {
                    await DisplayAlertAsync("Unduh Lampiran Resmi",
                        $"Berkas '{capturedName}' berhasil diunduh dan tersimpan di memori perangkat.",
                        "Buka Berkas");
                };
                Grid.SetColumn(openBtn, 1);
                attachGrid.Children.Add(openBtn);

                attachBorder.Content = attachGrid;
                stack.Children.Add(attachBorder);
            }

            card.Content = stack;
            AnnouncementsContainer.Children.Add(card);
        }
    }

    private void OnSubmitAnnouncementClicked(object? sender, EventArgs e)
    {
        var title = AnnTitleEntry.Text?.Trim() ?? string.Empty;
        var content = AnnContentEditor.Text?.Trim() ?? string.Empty;
        var audienceChoice = AudiencePicker.SelectedItem?.ToString() ?? "Semua";
        var priorityChoice = PriorityPicker.SelectedItem?.ToString() ?? "Penting";

        if (string.IsNullOrWhiteSpace(title))
        {
            ShowBanner("Judul pengumuman wajib diisi.", isError: true);
            AnnTitleEntry.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(content))
        {
            ShowBanner("Isi pengumuman wajib diisi.", isError: true);
            AnnContentEditor.Focus();
            return;
        }

        var audience = audienceChoice.Contains("Semua") ? "Semua" :
                       audienceChoice.Contains("Siswa") ? "Siswa" :
                       audienceChoice.Contains("Guru") ? "Guru" :
                       audienceChoice.Contains("Industri") ? "Industri" : "Monitor";

        var priority = priorityChoice switch
        {
            "Mendesak" => AnnouncementPriority.Mendesak,
            "Pengingat" => AnnouncementPriority.Pengingat,
            "Info Biasa" => AnnouncementPriority.Info,
            _ => AnnouncementPriority.Penting
        };

        var authorName = _currentUser?.FullName ?? "Koordinator Hubin SMK";
        var authorRole = _currentUser?.RoleName ?? "Hubungan Industri";

        var newAnn = new AnnouncementRecord
        {
            Title = title,
            Content = content,
            AuthorName = authorName,
            AuthorRole = authorRole,
            TargetAudience = audience,
            Priority = priority,
            IsPinned = PinCheck.IsChecked,
            AttachmentName = AttachmentProofLabel.Text.Split(' ')[0]
        };

        var (success, msg) = _dataService.AddAnnouncement(newAnn);
        ShowBanner(msg, isError: !success);

        AnnTitleEntry.Text = string.Empty;
        AnnContentEditor.Text = string.Empty;

        SwitchTab(0);
        RefreshAll();
    }

    private async void OnChooseAttachmentClicked(object? sender, EventArgs e)
    {
        var chosen = await DisplayActionSheetAsync("Pilih Lampiran Berkas", "Batal", null,
            "Surat Edaran Resmi (PDF)",
            "Panduan Template PKL (DOCX)",
            "Jadwal Sidang & Monev (XLSX)");

        if (!string.IsNullOrWhiteSpace(chosen) && chosen != "Batal")
        {
            AttachmentProofLabel.Text = $"{chosen.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd}.pdf (1.3 MB)";
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
        SectionCreate.IsVisible = index == 1;

        TabListBtn.BackgroundColor = index == 0 ? Color.FromArgb("#1E3A5F") : Color.FromArgb("#F1F5F9");
        TabListBtn.TextColor = index == 0 ? Colors.White : Color.FromArgb("#475569");

        TabCreateBtn.BackgroundColor = index == 1 ? Color.FromArgb("#1E3A5F") : Color.FromArgb("#F1F5F9");
        TabCreateBtn.TextColor = index == 1 ? Colors.White : Color.FromArgb("#475569");
    }

    private void OnTabListClicked(object? sender, EventArgs e) => SwitchTab(0);
    private void OnTabCreateClicked(object? sender, EventArgs e) => SwitchTab(1);

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
