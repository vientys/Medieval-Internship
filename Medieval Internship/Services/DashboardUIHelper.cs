using Medieval_Internship.Models;
using Medieval_Internship.Views.Modules;
using Microsoft.Maui.Controls.Shapes;

namespace Medieval_Internship.Services;

public static class DashboardUIHelper
{
    private static readonly AuthService _authService = AuthService.Instance;

    public static async Task<UserModel?> VerifyAccessAsync(Page page, UserRole allowedRole)
    {
        var user = _authService.CurrentUser;
        if (user == null || !_authService.ValidateCurrentSession())
        {
            _authService.Logout();
            await Shell.Current.GoToAsync("//MainPage");
            return null;
        }

        if (user.Role != allowedRole)
        {
            var correctRoute = RoleHelper.GetDashboardRoute(user.Role);
            await Shell.Current.GoToAsync($"//{correctRoute}");
            return null;
        }

        return user;
    }

    public static async Task ConfirmLogoutAsync(Page page)
    {
        bool confirm = await page.DisplayAlertAsync("Konfirmasi Keluar", "Apakah Anda yakin ingin keluar dari sistem PKL Monitor?", "Ya, Keluar", "Batal");
        if (confirm)
        {
            _authService.Logout();
            await Shell.Current.GoToAsync("//MainPage");
        }
    }

    public static void RenderStatCards(FlexLayout container, List<StatCardModel> cards)
    {
        container.Children.Clear();

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

            var topRow = new Grid
            {
                ColumnDefinitions =
                [
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                ]
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
            container.Children.Add(border);
        }
    }

    public static void RenderQuickActions(FlexLayout container, List<QuickActionModel> actions, Page page)
    {
        container.Children.Clear();

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
                ColumnDefinitions =
                [
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                ]
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

            var tap = new TapGestureRecognizer();
            var capturedAction = action;
            tap.Tapped += async (s, e) => await HandleQuickActionAsync(page, capturedAction);
            btnBorder.GestureRecognizers.Add(tap);

            container.Children.Add(btnBorder);
        }
    }

    public static void RenderStudentsList(VerticalStackLayout container, List<StudentItemModel> students, Page page)
    {
        container.Children.Clear();

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
                ColumnDefinitions =
                [
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                ]
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
                await page.DisplayAlertAsync("Detail Siswa Magang",
                    $"Nama: {capturedStudent.Name}\n{capturedStudent.NisnOrId}\nKelas: {capturedStudent.ClassName}\nTempat PKL: {capturedStudent.CompanyName}\nKehadiran: {capturedStudent.AttendancePercent}\nStatus Jurnal: {capturedStudent.LastJournalStatus}",
                    "Tutup");
            };
            Grid.SetColumn(actionBtn, 2);
            grid.Children.Add(actionBtn);

            card.Content = grid;
            container.Children.Add(card);
        }
    }

    public static void RenderJournalsList(VerticalStackLayout container, List<JournalItemModel> journals, UserRole role, Page page)
    {
        container.Children.Clear();

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

            var headerGrid = new Grid
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

            stack.Children.Add(new Label
            {
                Text = journal.Summary,
                FontSize = 12,
                TextColor = Color.FromArgb("#475569"),
                LineBreakMode = LineBreakMode.WordWrap
            });

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
                    await page.DisplayAlertAsync("Verifikasi Berhasil", $"Jurnal '{journal.Title}' telah disetujui.", "OK");
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
                    var note = await page.DisplayPromptAsync("Catatan Pembimbing", "Tuliskan feedback bimbingan untuk siswa:", "Kirim", "Batal");
                    if (!string.IsNullOrWhiteSpace(note))
                    {
                        await page.DisplayAlertAsync("Feedback Terkirim", $"Catatan '{note}' berhasil disampaikan ke siswa.", "OK");
                    }
                };

                actionRow.Children.Add(approveBtn);
                actionRow.Children.Add(noteBtn);
                stack.Children.Add(actionRow);
            }

            card.Content = stack;
            container.Children.Add(card);
        }
    }

    public static void RenderDocumentsList(VerticalStackLayout container, List<DocumentItemModel> documents, Page page)
    {
        container.Children.Clear();

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
                ColumnDefinitions =
                [
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                ]
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
                await page.DisplayAlertAsync("Dokumen Resmi PKL", $"Membuka berkas: {capturedDoc.DocumentTitle}\nStatus: {capturedDoc.Status}\nDokumen siap dicetak atau diunduh sebagai PDF.", "OK");
            };
            Grid.SetColumn(printBtn, 1);
            grid.Children.Add(printBtn);

            card.Content = grid;
            container.Children.Add(card);
        }
    }

    public static void RenderActivitiesList(VerticalStackLayout container, List<ActivityLogModel> activities)
    {
        container.Children.Clear();

        foreach (var act in activities)
        {
            var row = new Grid
            {
                ColumnDefinitions =
                [
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Star }
                ]
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

            container.Children.Add(row);
        }
    }

    public static async Task NavigateToModuleAsync(Page page, Page modulePage)
    {
        try
        {
            await page.Navigation.PushModalAsync(modulePage);
        }
        catch (Exception ex)
        {
            await page.DisplayAlertAsync("Buka Modul", $"Gagal membuka modul: {ex.Message}", "OK");
        }
    }

    public static async Task HandleQuickActionAsync(Page page, QuickActionModel action)
    {
        switch (action.ActionKey)
        {
            // Modul 4: Absensi Harian (Check-in, Check-out, Validasi)
            case "presensi_siswa":
            case "validasi_presensi_guru":
            case "validasi_presensi_industri":
                await NavigateToModuleAsync(page, new AbsensiModulePage());
                break;

            // Modul 5: Jurnal Harian (Tulis Jurnal, Approval, Tugas)
            case "tulis_jurnal":
            case "verif_jurnal_guru":
            case "approve_jurnal_industri":
            case "beri_tugas":
                await NavigateToModuleAsync(page, new JurnalHarianModulePage());
                break;

            // Modul 6: Monitoring & Kunjungan Pembimbing (Monev DUDI)
            case "jadwal_monev":
            case "catatan_pembinaan":
                await NavigateToModuleAsync(page, new MonitoringKunjunganModulePage());
                break;

            // Modul 9: Laporan Akhir PKL & Penilaian (Bab 1-5, Nilai Sekolah & DUDI)
            case "upload_laporan":
            case "lihat_nilai":
            case "nilai_sekolah":
            case "nilai_industri":
                await NavigateToModuleAsync(page, new LaporanAkhirModulePage());
                break;

            // Modul 2: Master Data (Siswa, Peran, Jurusan, Kelas, DUDI, Impor Data)
            case "admin_user_mgmt":
            case "admin_dudi_mgmt":
            case "master_data":
                await NavigateToModuleAsync(page, new MasterDataModulePage());
                break;

            // Modul 3: Manajemen Pengguna & Penempatan PKL (Plotting & Periode PKL)
            case "admin_periode_mgmt":
            case "matching_siswa":
            case "penempatan_pkl":
            case "kontak_dudi":
                await NavigateToModuleAsync(page, new PenempatanPklModulePage());
                break;

            // Modul 8: Dashboard & Ekspor Pelaporan (PDF / Excel, Sertifikat, MoU)
            case "rekap_nilai":
            case "cetak_surat":
            case "cetak_sertifikat":
            case "unduh_laporan_monev":
            case "analisis_link_match":
            case "audit_mutu":
            case "evaluasi_kemitraan":
            case "ekspor_laporan":
                await NavigateToModuleAsync(page, new EksporLaporanModulePage());
                break;

            // Modul 7: Informasi dan Pengumuman (Sekolah, Hubin, DUDI)
            case "pengumuman_info":
                await NavigateToModuleAsync(page, new PengumumanModulePage());
                break;

            // Modul 10: Manajemen Aplikasi (Profil Sekolah, Konfigurasi Sistem, Kontak)
            case "admin_audit_log":
            case "manajemen_aplikasi":
            case "bantuan_aplikasi":
                await NavigateToModuleAsync(page, new ManajemenAplikasiModulePage());
                break;

            default:
                await page.DisplayAlertAsync(action.Title, action.Subtitle, "OK");
                break;
        }
    }
}
