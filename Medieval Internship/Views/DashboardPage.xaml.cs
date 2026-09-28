using Medieval_Internship.Models;
using Medieval_Internship.Services;
using Microsoft.Maui.Controls.Shapes;

namespace Medieval_Internship.Views;

public partial class DashboardPage : ContentPage
{
    private readonly AuthService _authService = AuthService.Instance;
    private readonly DashboardDataService _dataService = DashboardDataService.Instance;

    private UserRole _currentRole = UserRole.Siswa;
    private UserModel? _currentUser;

    public DashboardPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _currentUser = _authService.CurrentUser;
        if (_currentUser == null)
        {
            await Shell.Current.GoToAsync("//MainPage");
            return;
        }
        var targetRoute = RoleHelper.GetDashboardRoute(_currentUser.Role);
        await Shell.Current.GoToAsync($"//{targetRoute}");
    }

    private void LoadDashboard(UserRole role, UserModel? user)
    {
        _currentRole = role;
        var data = _dataService.GetDashboardData(role, user);

        // Update Top Navigation Bar
        UpdateTopBar(role, user);

        // Update Hero Banner
        HeroRoleSubtitle.Text = data.RoleSubtitle;
        HeroWelcomeTitle.Text = data.WelcomeBannerTitle;
        HeroWelcomeDesc.Text = data.WelcomeBannerDesc;

        HeroIconLabel.Text = role switch
        {
            UserRole.Siswa => "🚀",
            UserRole.GuruPendamping => "👨‍🏫",
            UserRole.PembimbingIndustri => "🏢",
            UserRole.Admin => "⚙️",
            UserRole.Operator => "📋",
            UserRole.Monitor => "📈",
            _ => "🎓"
        };

        if (data.ShowProgressBar)
        {
            ProgressSection.IsVisible = true;
            ProgressTitleLabel.Text = data.ProgressTitle;
            ProgressTextLabel.Text = data.ProgressText;
            HeroProgressBar.Progress = data.ProgressRatio;
        }
        else
        {
            ProgressSection.IsVisible = false;
        }

        // Render Stat Cards
        RenderStatCards(data.StatCards);

        // Render Quick Actions
        RenderQuickActions(data.QuickActions);

        // Render Role Specific Sections
        RenderRoleSpecificSections(role, data);
    }

    private void UpdateTopBar(UserRole role, UserModel? user)
    {
        var roleStr = RoleHelper.ToStringRole(role);
        var badgeColor = Color.FromArgb(RoleHelper.GetRoleBadgeColor(roleStr));

        RoleBadgeLabel.Text = roleStr;
        RoleHeaderBadge.BackgroundColor = badgeColor;

        if (user != null)
        {
            UserNameLabel.Text = user.FullName;
            UserSubLabel.Text = string.IsNullOrWhiteSpace(user.DetailInfo) ? user.RoleName : user.DetailInfo;
            AvatarInitialsLabel.Text = user.Initials;
            HeaderOrgLabel.Text = string.IsNullOrWhiteSpace(user.OrganizationOrSchool) ? "SMK Negeri 1 Jakarta" : user.OrganizationOrSchool;
        }
        else
        {
            UserNameLabel.Text = "Pengguna PKL";
            UserSubLabel.Text = roleStr;
            AvatarInitialsLabel.Text = "PKL";
        }

        AvatarBorder.BackgroundColor = badgeColor;
    }

    private void RenderStatCards(List<StatCardModel> cards)
    {
        StatCardsContainer.Children.Clear();

        foreach (var card in cards)
        {
            var accentColor = Color.FromArgb(card.AccentColor);
            var lightColor = Color.FromArgb(card.LightBgColor);

            var border = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(14) },
                BackgroundColor = Colors.White,
                Stroke = Color.FromArgb("#E2E8F0"),
                StrokeThickness = 1,
                Padding = new Thickness(16),
                Margin = new Thickness(0, 0, 8, 8),
                MinimumWidthRequest = 240
            };

            FlexLayout.SetBasis(border, new Microsoft.Maui.Layouts.FlexBasis(0.48f, true));
            FlexLayout.SetGrow(border, 1.0f);

            var stack = new VerticalStackLayout { Spacing = 10 };

            // Top Row: Icon container & Badge
            var topRow = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                }
            };

            var iconBorder = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(20) },
                BackgroundColor = lightColor,
                Stroke = Colors.Transparent,
                StrokeThickness = 0,
                HeightRequest = 40,
                WidthRequest = 40,
                Content = new Label
                {
                    Text = card.Icon,
                    FontSize = 18,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                }
            };
            Grid.SetColumn(iconBorder, 0);
            topRow.Children.Add(iconBorder);

            if (!string.IsNullOrWhiteSpace(card.BadgeText))
            {
                var badge = new Border
                {
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) },
                    BackgroundColor = lightColor,
                    Stroke = Colors.Transparent,
                    StrokeThickness = 0,
                    Padding = new Thickness(8, 2),
                    VerticalOptions = LayoutOptions.Center,
                    Content = new Label
                    {
                        Text = card.BadgeText,
                        TextColor = accentColor,
                        FontSize = 10,
                        FontAttributes = FontAttributes.Bold
                    }
                };
                Grid.SetColumn(badge, 2);
                topRow.Children.Add(badge);
            }

            stack.Children.Add(topRow);

            // Value & Title
            var textStack = new VerticalStackLayout { Spacing = 2 };
            textStack.Children.Add(new Label
            {
                Text = card.Value,
                FontSize = 24,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#0F172A")
            });
            textStack.Children.Add(new Label
            {
                Text = card.Title.ToUpperInvariant(),
                FontSize = 11,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#64748B")
            });
            textStack.Children.Add(new Label
            {
                Text = card.Subtext,
                FontSize = 11,
                TextColor = Color.FromArgb("#94A3B8")
            });

            stack.Children.Add(textStack);
            border.Content = stack;
            StatCardsContainer.Children.Add(border);
        }
    }

    private void RenderQuickActions(List<QuickActionModel> actions)
    {
        QuickActionsContainer.Children.Clear();

        foreach (var action in actions)
        {
            var btnBorder = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(14) },
                BackgroundColor = Colors.White,
                Stroke = Color.FromArgb("#E2E8F0"),
                StrokeThickness = 1,
                Padding = new Thickness(16, 14),
                Margin = new Thickness(0, 0, 8, 8),
                MinimumWidthRequest = 240
            };

            FlexLayout.SetBasis(btnBorder, new Microsoft.Maui.Layouts.FlexBasis(0.48f, true));
            FlexLayout.SetGrow(btnBorder, 1.0f);

            var row = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                }
            };

            var iconBox = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(12) },
                BackgroundColor = Color.FromArgb(action.LightColor),
                Stroke = Colors.Transparent,
                StrokeThickness = 0,
                HeightRequest = 42,
                WidthRequest = 42,
                VerticalOptions = LayoutOptions.Center,
                Content = new Label
                {
                    Text = action.Icon,
                    FontSize = 20,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                }
            };
            Grid.SetColumn(iconBox, 0);
            row.Children.Add(iconBox);

            var labelStack = new VerticalStackLayout
            {
                Spacing = 2,
                Margin = new Thickness(12, 0, 8, 0),
                VerticalOptions = LayoutOptions.Center
            };
            labelStack.Children.Add(new Label
            {
                Text = action.Title,
                FontSize = 13,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#0F172A")
            });
            labelStack.Children.Add(new Label
            {
                Text = action.Subtitle,
                FontSize = 11,
                TextColor = Color.FromArgb("#64748B"),
                LineBreakMode = LineBreakMode.TailTruncation
            });
            Grid.SetColumn(labelStack, 1);
            row.Children.Add(labelStack);

            var arrowLabel = new Label
            {
                Text = "→",
                TextColor = Color.FromArgb("#94A3B8"),
                FontSize = 16,
                VerticalOptions = LayoutOptions.Center
            };
            Grid.SetColumn(arrowLabel, 2);
            row.Children.Add(arrowLabel);

            btnBorder.Content = row;

            // Tap gesture
            var tap = new TapGestureRecognizer();
            var capturedAction = action;
            tap.Tapped += async (s, e) => await HandleQuickAction(capturedAction);
            btnBorder.GestureRecognizers.Add(tap);

            QuickActionsContainer.Children.Add(btnBorder);
        }
    }

    private void RenderRoleSpecificSections(UserRole role, RoleDashboardData data)
    {
        // Reset visibility
        StudentsSection.IsVisible = false;
        JournalsSection.IsVisible = false;
        DocumentsSection.IsVisible = false;
        ActivitiesSection.IsVisible = false;

        switch (role)
        {
            case UserRole.Siswa:
                JournalsSection.IsVisible = true;
                JournalsSectionTitle.Text = "Jurnal Aktivitas Harian Siswa";
                RenderJournalsList(data.Journals, role);

                ActivitiesSection.IsVisible = true;
                RenderActivitiesList(data.Activities);
                break;

            case UserRole.GuruPendamping:
                StudentsSection.IsVisible = true;
                StudentsSectionTitle.Text = "Siswa Bimbingan PKL (Wilayah Anda)";
                RenderStudentsList(data.Students);

                JournalsSection.IsVisible = true;
                JournalsSectionTitle.Text = "Jurnal Menunggu Reviu Guru";
                RenderJournalsList(data.Journals, role);
                break;

            case UserRole.PembimbingIndustri:
                StudentsSection.IsVisible = true;
                StudentsSectionTitle.Text = "Siswa Magang di Perusahaan";
                RenderStudentsList(data.Students);

                JournalsSection.IsVisible = true;
                JournalsSectionTitle.Text = "Log Jurnal Harian dari Siswa";
                RenderJournalsList(data.Journals, role);
                break;

            case UserRole.Admin:
                ActivitiesSection.IsVisible = true;
                RenderActivitiesList(data.Activities);
                break;

            case UserRole.Operator:
                DocumentsSection.IsVisible = true;
                RenderDocumentsList(data.Documents);
                break;

            case UserRole.Monitor:
                ActivitiesSection.IsVisible = true;
                RenderActivitiesList(data.Activities);
                break;
        }
    }

    private void RenderStudentsList(List<StudentItemModel> students)
    {
        StudentsListContainer.Children.Clear();

        foreach (var student in students)
        {
            var card = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(14) },
                BackgroundColor = Colors.White,
                Stroke = Color.FromArgb("#E2E8F0"),
                StrokeThickness = 1,
                Padding = new Thickness(16, 12)
            };

            var grid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                }
            };

            var avatar = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(20) },
                BackgroundColor = Color.FromArgb("#EFF6FF"),
                Stroke = Colors.Transparent,
                StrokeThickness = 0,
                HeightRequest = 40,
                WidthRequest = 40,
                VerticalOptions = LayoutOptions.Center,
                Content = new Label
                {
                    Text = "👤",
                    FontSize = 18,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                }
            };
            Grid.SetColumn(avatar, 0);
            grid.Children.Add(avatar);

            var infoStack = new VerticalStackLayout
            {
                Spacing = 2,
                Margin = new Thickness(12, 0, 8, 0),
                VerticalOptions = LayoutOptions.Center
            };
            infoStack.Children.Add(new Label
            {
                Text = student.Name,
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#0F172A")
            });
            infoStack.Children.Add(new Label
            {
                Text = $"{student.ClassName} • {student.CompanyName}",
                FontSize = 11,
                TextColor = Color.FromArgb("#64748B")
            });
            infoStack.Children.Add(new Label
            {
                Text = $"{student.NisnOrId} • Kehadiran: {student.AttendancePercent}",
                FontSize = 11,
                TextColor = Color.FromArgb("#10B981"),
                FontAttributes = FontAttributes.Bold
            });
            Grid.SetColumn(infoStack, 1);
            grid.Children.Add(infoStack);

            var actionBtn = new Button
            {
                Text = "Detail Siswa",
                BackgroundColor = Color.FromArgb("#EFF6FF"),
                TextColor = Color.FromArgb("#2563EB"),
                FontSize = 11,
                FontAttributes = FontAttributes.Bold,
                HeightRequest = 32,
                Padding = new Thickness(10, 0),
                CornerRadius = 8,
                VerticalOptions = LayoutOptions.Center
            };
            var capturedStudent = student;
            actionBtn.Clicked += async (s, e) =>
            {
                await DisplayAlertAsync("Detail Siswa Magang",
                    $"Nama: {capturedStudent.Name}\n{capturedStudent.NisnOrId}\nKelas: {capturedStudent.ClassName}\nTempat PKL: {capturedStudent.CompanyName}\nKehadiran: {capturedStudent.AttendancePercent}\nStatus Jurnal: {capturedStudent.LastJournalStatus}",
                    "Tutup");
            };
            Grid.SetColumn(actionBtn, 2);
            grid.Children.Add(actionBtn);

            card.Content = grid;
            StudentsListContainer.Children.Add(card);
        }
    }

    private void RenderJournalsList(List<JournalItemModel> journals, UserRole role)
    {
        JournalsListContainer.Children.Clear();

        foreach (var journal in journals)
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

            // Header Row: Student name, Date, Status Pill
            var headerGrid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                }
            };

            var titleStack = new VerticalStackLayout { Spacing = 2 };
            titleStack.Children.Add(new Label
            {
                Text = journal.Title,
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#0F172A")
            });
            titleStack.Children.Add(new Label
            {
                Text = $"{journal.StudentName} • {journal.DateFormatted}",
                FontSize = 11,
                TextColor = Color.FromArgb("#64748B")
            });
            Grid.SetColumn(titleStack, 0);
            headerGrid.Children.Add(titleStack);

            var statusBadge = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) },
                BackgroundColor = Color.FromArgb(journal.StatusBgColor),
                Stroke = Colors.Transparent,
                StrokeThickness = 0,
                Padding = new Thickness(10, 4),
                VerticalOptions = LayoutOptions.Start,
                Content = new Label
                {
                    Text = journal.Status,
                    TextColor = Color.FromArgb(journal.StatusColor),
                    FontSize = 11,
                    FontAttributes = FontAttributes.Bold
                }
            };
            Grid.SetColumn(statusBadge, 1);
            headerGrid.Children.Add(statusBadge);

            stack.Children.Add(headerGrid);

            // Summary text
            stack.Children.Add(new Label
            {
                Text = journal.Summary,
                FontSize = 12,
                TextColor = Color.FromArgb("#475569"),
                LineBreakMode = LineBreakMode.WordWrap
            });

            // Action row if Guru or Pembimbing Industri
            if (role == UserRole.GuruPendamping || role == UserRole.PembimbingIndustri)
            {
                var actionRow = new HorizontalStackLayout { Spacing = 8, HorizontalOptions = LayoutOptions.End };

                var approveBtn = new Button
                {
                    Text = "✓ Setujui Jurnal",
                    BackgroundColor = Color.FromArgb("#10B981"),
                    TextColor = Colors.White,
                    FontSize = 11,
                    HeightRequest = 30,
                    Padding = new Thickness(12, 0),
                    CornerRadius = 6
                };
                approveBtn.Clicked += async (s, e) =>
                {
                    journal.Status = "Disetujui";
                    journal.StatusColor = "#10B981";
                    journal.StatusBgColor = "#ECFDF5";
                    statusBadge.BackgroundColor = Color.FromArgb("#ECFDF5");
                    ((Label)statusBadge.Content).Text = "Disetujui";
                    ((Label)statusBadge.Content).TextColor = Color.FromArgb("#10B981");
                    await DisplayAlertAsync("Verifikasi Berhasil", $"Jurnal '{journal.Title}' telah disetujui.", "OK");
                };

                var noteBtn = new Button
                {
                    Text = "💬 Beri Catatan",
                    BackgroundColor = Color.FromArgb("#EFF6FF"),
                    TextColor = Color.FromArgb("#2563EB"),
                    FontSize = 11,
                    HeightRequest = 30,
                    Padding = new Thickness(12, 0),
                    CornerRadius = 6
                };
                noteBtn.Clicked += async (s, e) =>
                {
                    var note = await DisplayPromptAsync("Catatan Pembimbing", "Tuliskan feedback bimbingan untuk siswa:", "Kirim", "Batal");
                    if (!string.IsNullOrWhiteSpace(note))
                    {
                        await DisplayAlertAsync("Feedback Terkirim", $"Catatan '{note}' berhasil disampaikan ke siswa.", "OK");
                    }
                };

                actionRow.Children.Add(approveBtn);
                actionRow.Children.Add(noteBtn);
                stack.Children.Add(actionRow);
            }

            card.Content = stack;
            JournalsListContainer.Children.Add(card);
        }
    }

    private void RenderDocumentsList(List<DocumentItemModel> documents)
    {
        DocumentsListContainer.Children.Clear();

        foreach (var doc in documents)
        {
            var card = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(14) },
                BackgroundColor = Colors.White,
                Stroke = Color.FromArgb("#E2E8F0"),
                StrokeThickness = 1,
                Padding = new Thickness(16, 12)
            };

            var grid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                }
            };

            var textStack = new VerticalStackLayout { Spacing = 2 };
            textStack.Children.Add(new Label
            {
                Text = doc.DocumentTitle,
                FontSize = 13,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#0F172A")
            });
            textStack.Children.Add(new Label
            {
                Text = $"Tujuan: {doc.TargetParty} • {doc.DateFormatted}",
                FontSize = 11,
                TextColor = Color.FromArgb("#64748B")
            });

            var badge = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(8) },
                BackgroundColor = Color.FromArgb(doc.StatusBgColor),
                Stroke = Colors.Transparent,
                StrokeThickness = 0,
                Padding = new Thickness(8, 2),
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(0, 4, 0, 0),
                Content = new Label
                {
                    Text = doc.Status,
                    TextColor = Color.FromArgb(doc.StatusColor),
                    FontSize = 10,
                    FontAttributes = FontAttributes.Bold
                }
            };
            textStack.Children.Add(badge);
            Grid.SetColumn(textStack, 0);
            grid.Children.Add(textStack);

            var printBtn = new Button
            {
                Text = doc.ActionLabel,
                BackgroundColor = Color.FromArgb("#2563EB"),
                TextColor = Colors.White,
                FontSize = 11,
                FontAttributes = FontAttributes.Bold,
                HeightRequest = 32,
                Padding = new Thickness(12, 0),
                CornerRadius = 8,
                VerticalOptions = LayoutOptions.Center
            };
            var capturedDoc = doc;
            printBtn.Clicked += async (s, e) =>
            {
                await DisplayAlertAsync("Dokumen Resmi PKL", $"Membuka berkas: {capturedDoc.DocumentTitle}\nStatus: {capturedDoc.Status}\nDokumen siap dicetak atau diunduh sebagai PDF.", "OK");
            };
            Grid.SetColumn(printBtn, 1);
            grid.Children.Add(printBtn);

            card.Content = grid;
            DocumentsListContainer.Children.Add(card);
        }
    }

    private void RenderActivitiesList(List<ActivityLogModel> activities)
    {
        ActivitiesListContainer.Children.Clear();

        foreach (var act in activities)
        {
            var row = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Star }
                }
            };

            var iconBorder = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(16) },
                BackgroundColor = Color.FromArgb("#F1F5F9"),
                Stroke = Colors.Transparent,
                StrokeThickness = 0,
                HeightRequest = 32,
                WidthRequest = 32,
                VerticalOptions = LayoutOptions.Start,
                Content = new Label
                {
                    Text = act.Icon,
                    FontSize = 14,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                }
            };
            Grid.SetColumn(iconBorder, 0);
            row.Children.Add(iconBorder);

            var textStack = new VerticalStackLayout
            {
                Spacing = 2,
                Margin = new Thickness(10, 0, 0, 0),
                VerticalOptions = LayoutOptions.Center
            };

            var head = new HorizontalStackLayout { Spacing = 6 };
            head.Children.Add(new Label
            {
                Text = act.Title,
                FontSize = 13,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#0F172A")
            });
            head.Children.Add(new Label
            {
                Text = $"• {act.TimeAgo}",
                FontSize = 11,
                TextColor = Color.FromArgb("#94A3B8")
            });
            textStack.Children.Add(head);

            textStack.Children.Add(new Label
            {
                Text = act.Detail,
                FontSize = 11,
                TextColor = Color.FromArgb("#64748B")
            });

            Grid.SetColumn(textStack, 1);
            row.Children.Add(textStack);

            ActivitiesListContainer.Children.Add(row);
        }
    }

    private async Task HandleQuickAction(QuickActionModel action)
    {
        switch (action.ActionKey)
        {
            case "presensi_siswa":
                await DisplayAlertAsync("Presensi Masuk Berhasil 📍",
                    "Kehadiran Anda hari ini telah tercatat:\n• Jam: " + DateTime.Now.ToString("HH:mm") + " WIB\n• Lokasi: Kantor Telkom Landmark Tower (Valid GPS)\n• Status: Hadir Tepat Waktu",
                    "Mantap!");
                break;

            case "tulis_jurnal":
                var title = await DisplayPromptAsync("Tulis Jurnal Harian ✍️", "Judul aktivitas pekerjaan hari ini:", "Lanjut", "Batal", "Contoh: Slicing UI Dashboard");
                if (!string.IsNullOrWhiteSpace(title))
                {
                    var desc = await DisplayPromptAsync("Detail Kegiatan", "Rincian tugas dan teknologi yang digunakan:", "Kirim Jurnal", "Batal");
                    if (!string.IsNullOrWhiteSpace(desc))
                    {
                        await DisplayAlertAsync("Jurnal Tersimpan 🎉", "Jurnal harian Anda telah dikirim dan menunggu verifikasi mentor industri.", "OK");
                    }
                }
                break;

            case "upload_laporan":
                await DisplayAlertAsync("Upload Laporan Akhir 📤",
                    "Pilih berkas dokumen PDF (Bab 1 s/d Bab 5).\nFormat yang didukung: .pdf, .docx (Maksimal 25MB).\nSistem siap menerima berkas.",
                    "Pilih Berkas");
                break;

            case "lihat_nilai":
                await DisplayAlertAsync("Transkrip Nilai PKL 🏆",
                    "Penilaian Siswa Magang:\n1. Nilai Pembimbing DUDI: 94.0 (Sangat Baik)\n2. Nilai Guru Pendamping: 90.5 (Sangat Baik)\n3. Rata-rata Akhir: 92.3 (Predikat A)\nStatus: Memenuhi Syarat Kelulusan Vokasi",
                    "Tutup");
                break;

            case "verif_jurnal_guru":
                await DisplayAlertAsync("Verifikasi Jurnal Guru 📋",
                    "Menampilkan 6 jurnal siswa bimbingan yang menunggu validasi Anda. Anda dapat menyetujui langsung pada daftar jurnal di bawah.",
                    "Pahami");
                break;

            case "jadwal_monev":
                await DisplayAlertAsync("Jadwal Monitoring & Kunjungan 🚗",
                    "Agenda Monev Pekan Ini:\n• Kamis, 09:00: PT Telkom Indonesia\n• Jumat, 10:00: PT Bank Mandiri Digital Lab\n• Senin, 13:00: PT Astra International",
                    "OK");
                break;

            case "nilai_sekolah":
                await DisplayAlertAsync("Form Nilai Guru Pembimbing ⭐",
                    "Aspek Penilaian Sekolah:\n• Laporan Akhir & Logbook (30%)\n• Disiplin & Sikap Kerjasama (30%)\n• Ujian Sidang Presentasi (40%)",
                    "Buka Form");
                break;

            case "kontak_dudi":
                await DisplayAlertAsync("Kontak Mitra Industri 📞",
                    "Pembimbing Industri Aktif:\n• Hendro Wicaksono (PT Telkom): +62 812-9988-7766\n• Aris Munandar (Bank Mandiri): +62 813-2233-4455",
                    "Tutup");
                break;

            case "approve_jurnal_industri":
                await DisplayAlertAsync("Approval Jurnal Industri ✅",
                    "Ada 3 jurnal dari siswa di tim Anda yang siap divalidasi. Silakan klik tombol 'Setujui' pada entri jurnal di bawah.",
                    "Siap");
                break;

            case "beri_tugas":
                var taskName = await DisplayPromptAsync("Penugasan Sprint Magang 🎯", "Nama task / tiket pekerjaan:", "Tugaskan", "Batal");
                if (!string.IsNullOrWhiteSpace(taskName))
                {
                    await DisplayAlertAsync("Task Ditugaskan", $"Pekerjaan '{taskName}' berhasil dibagikan ke siswa magang.", "OK");
                }
                break;

            case "nilai_industri":
                await DisplayAlertAsync("Form Penilaian Kinerja Industri ⭐",
                    "Indikator Penilaian DUDI:\n• Keterampilan Teknis / Hard Skill (40%)\n• Komunikasi, Inisiatif & Disiplin (40%)\n• Ketepatan Waktu & K3 (20%)",
                    "Mulai Nilai");
                break;

            case "catatan_pembinaan":
                var note = await DisplayPromptAsync("Catatan Pembinaan 💬", "Tuliskan catatan arahan untuk siswa magang:", "Simpan", "Batal");
                if (!string.IsNullOrWhiteSpace(note))
                {
                    await DisplayAlertAsync("Tersimpan", "Catatan pembinaan berhasil disimpan dan diteruskan ke guru pembimbing.", "OK");
                }
                break;

            case "admin_user_mgmt":
                await DisplayAlertAsync("Manajemen Pengguna 👤",
                    "Pusat Data User:\n• Total Akun: 642\n• Siswa: 480\n• Guru: 45\n• Mentor Industri: 85\n• Operator: 12\n• Monitor: 20\nFitur: Tambah User, Reset Sandi, Ubah Hak Akses.",
                    "OK");
                break;

            case "admin_dudi_mgmt":
                await DisplayAlertAsync("Master Data DUDI 🏢",
                    "Terdapat 54 Mitra Perusahaan aktif dengan total kuota 210 siswa magang. Seluruh MoU tercatat dalam status aktif.",
                    "OK");
                break;

            case "admin_periode_mgmt":
                await DisplayAlertAsync("Konfigurasi Gelombang PKL ⚙️",
                    "Periode Aktif: Gelombang 1 Tahun Ajaran 2026/2027\nTanggal Mulai: 01 Juli 2026\nTanggal Selesai: 30 September 2026",
                    "OK");
                break;

            case "admin_audit_log":
                await DisplayAlertAsync("Audit Trail Keamanan 📜",
                    "Audit Log Sistem:\n• Tidak ada percobaan login mencurigakan.\n• Enkripsi database: AES-256 GCM.\n• 100% request API terotentikasi.",
                    "OK");
                break;

            case "matching_siswa":
                await DisplayAlertAsync("Plotting & Penempatan Siswa 🤝",
                    "Status Penempatan:\n• 172 Siswa sudah diplot ke 54 DUDI.\n• 13 Siswa dalam antrean verifikasi minat industri alternatif.",
                    "Buka Plotting");
                break;

            case "cetak_surat":
                await DisplayAlertAsync("Cetak Surat Pengantar PKL 🖨️",
                    "Surat pengantar resmi berkop sekolah dengan QR barcode validasi siap di-generate ke format PDF.",
                    "Generate PDF");
                break;

            case "rekap_nilai":
                await DisplayAlertAsync("Rekapitulasi Nilai PKL 📊",
                    "Ekspor rekapitulasi nilai komprehensif ke format Microsoft Excel (.xlsx) atau PDF.",
                    "Unduh Excel");
                break;

            case "cetak_sertifikat":
                await DisplayAlertAsync("Penerbitan Sertifikat PKL 📜",
                    "Modul sertifikasi otomatis dengan nomor seri unik terverifikasi sekolah dan DUDI.",
                    "Buka Modul");
                break;

            case "unduh_laporan_monev":
                await DisplayAlertAsync("Laporan Eksekutif Monev 📑",
                    "Laporan Ringkasan Eksekutif Hasil Monitoring PKL Semester Ganjil 2026 siap diunduh.",
                    "Unduh Laporan");
                break;

            case "analisis_link_match":
                await DisplayAlertAsync("Analisis Link & Match 🔗",
                    "Indeks Keselarasan Kurikulum Vokasi dengan DUDI: 94.2% (Kategori Sangat Selaras).",
                    "Tutup");
                break;

            case "audit_mutu":
                await DisplayAlertAsync("Audit Mutu Pelaksanaan PKL 📋",
                    "Indikator Standar Mutu Vokasi:\n• Kepatuhan SOP: 99%\n• Ketepatan Pembagian DUDI: 96%\n• Tingkat Kepuasan Siswa: 97.4%",
                    "OK");
                break;

            case "evaluasi_kemitraan":
                await DisplayAlertAsync("Evaluasi Kemitraan DUDI 🏢",
                    "Hasil Monev merekomendasikan perpanjangan MoU dengan seluruh 54 mitra DUDI untuk tahun ajaran berikutnya.",
                    "OK");
                break;

            default:
                await DisplayAlertAsync(action.Title, action.Subtitle, "OK");
                break;
        }
    }

    private async void OnLogoutClicked(object? sender, EventArgs e)
    {
        bool confirm = await DisplayAlertAsync("Konfirmasi Keluar", "Apakah Anda yakin ingin keluar dari sistem PKL Monitor?", "Ya, Keluar", "Batal");
        if (confirm)
        {
            _authService.Logout();
            await Shell.Current.GoToAsync("//MainPage");
        }
    }

    private async void OnViewAllStudentsTapped(object? sender, TappedEventArgs e)
    {
        await DisplayAlertAsync("Daftar Siswa", "Menampilkan seluruh daftar siswa bimbingan PKL.", "OK");
    }

    private async void OnAddJournalClicked(object? sender, EventArgs e)
    {
        await HandleQuickAction(new QuickActionModel { ActionKey = "tulis_jurnal" });
    }

    private async void OnAddDocumentClicked(object? sender, EventArgs e)
    {
        await DisplayAlertAsync("Terbitkan Dokumen", "Pilih jenis dokumen: Surat Pengantar, Surat Tugas, atau Lembar Pengesahan.", "Lanjutkan");
    }
}
