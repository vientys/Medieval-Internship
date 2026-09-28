using Medieval_Internship.Models;
using Medieval_Internship.Services;
using Microsoft.Maui.Controls.Shapes;

namespace Medieval_Internship.Views.Modules;

public partial class MasterDataModulePage : ContentPage
{
    private readonly AuthService _authService = AuthService.Instance;
    private readonly PklDataService _dataService = PklDataService.Instance;
    private UserModel? _currentUser;

    public MasterDataModulePage()
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
        LoadDefaultStudentTemplate();
    }

    private void RefreshAll()
    {
        RenderStudents();
        RenderCompanies();
        RenderMajors();
        RenderClasses();
        RenderMentors();
    }

    // ==========================================
    // TAB 1: MASTER DATA SISWA (FR-MST-01)
    // ==========================================
    private void RenderStudents()
    {
        StudentsListContainer.Children.Clear();
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
            
            var nameRow = new HorizontalStackLayout { Spacing = 8 };
            nameRow.Children.Add(new Label
            {
                Text = s.FullName,
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#0F172A")
            });
            var statusBadge = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(6) },
                BackgroundColor = s.Status == "Active" ? Color.FromArgb("#ECFDF5") : Color.FromArgb("#FEF2F2"),
                Stroke = s.Status == "Active" ? Color.FromArgb("#6EE7B7") : Color.FromArgb("#FCA5A5"),
                Padding = new Thickness(6, 2),
                Content = new Label
                {
                    Text = s.Status,
                    FontSize = 10,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = s.Status == "Active" ? Color.FromArgb("#065F46") : Color.FromArgb("#DC2626")
                }
            };
            nameRow.Children.Add(statusBadge);
            stack.Children.Add(nameRow);

            stack.Children.Add(new Label
            {
                Text = $"NISN: {s.Nisn} • Kelas: {s.ClassName} ({s.Major}) • Telp: {s.PhoneNumber}",
                FontSize = 11,
                TextColor = Color.FromArgb("#64748B")
            });
            stack.Children.Add(new Label
            {
                Text = $"🏢 Mitra DUDI: {s.CompanyName}",
                FontSize = 11,
                TextColor = Color.FromArgb("#2563EB"),
                FontAttributes = FontAttributes.Bold
            });
            stack.Children.Add(new Label
            {
                Text = $"👨‍🏫 Guru: {s.InternalMentorName} | 💼 Mentor: {s.ExternalMentorName}",
                FontSize = 11,
                TextColor = Color.FromArgb("#475569")
            });
            stack.Children.Add(new Label
            {
                Text = $"📅 Periode PKL: {s.StartDate:dd/MM/yyyy} s/d {s.EndDate:dd/MM/yyyy}",
                FontSize = 10,
                TextColor = Color.FromArgb("#059669")
            });

            Grid.SetColumn(stack, 0);
            grid.Children.Add(stack);

            var btnRow = new HorizontalStackLayout { Spacing = 6, VerticalOptions = LayoutOptions.Center };

            var editBtn = new Button
            {
                Text = "Detail",
                BackgroundColor = Color.FromArgb("#F1F5F9"),
                TextColor = Color.FromArgb("#334155"),
                FontSize = 11,
                HeightRequest = 32,
                Padding = new Thickness(10, 0),
                CornerRadius = 6
            };
            var capturedStudent = s;
            editBtn.Clicked += async (sender, e) =>
            {
                await DisplayAlertAsync("Detail Siswa PKL (FR-MST-01)",
                    $"Nama Lengkap: {capturedStudent.FullName}\nNISN: {capturedStudent.Nisn}\nKelas: {capturedStudent.ClassName}\nJurusan: {capturedStudent.Major}\nMitra DUDI: {capturedStudent.CompanyName}\nPembimbing Internal: {capturedStudent.InternalMentorName}\nPembimbing Industri: {capturedStudent.ExternalMentorName}\nTanggal Mulai: {capturedStudent.StartDate:dd MMM yyyy}\nTanggal Selesai: {capturedStudent.EndDate:dd MMM yyyy}\nNo Telp: {capturedStudent.PhoneNumber}\nStatus: {capturedStudent.Status}",
                    "Tutup");
            };
            btnRow.Children.Add(editBtn);

            var deleteBtn = new Button
            {
                Text = "🗑️ Hapus",
                BackgroundColor = Color.FromArgb("#FEE2E2"),
                TextColor = Color.FromArgb("#DC2626"),
                FontSize = 11,
                HeightRequest = 32,
                Padding = new Thickness(10, 0),
                CornerRadius = 6
            };
            deleteBtn.Clicked += async (sender, e) =>
            {
                var confirm = await DisplayAlertAsync("Konfirmasi Hapus Siswa",
                    $"Yakin ingin menghapus siswa '{capturedStudent.FullName}'?\nSesuai FR-MST-01, jika siswa memiliki riwayat presensi/jurnal/laporan, data tidak dapat dihapus dan status akan diubah menjadi Inactive.",
                    "Hapus / Nonaktifkan", "Batal");
                if (!confirm) return;

                var (success, msg) = _dataService.DeleteStudent(capturedStudent.Id);
                ShowBanner(msg, isError: !success);
                RefreshAll();
            };
            btnRow.Children.Add(deleteBtn);

            Grid.SetColumn(btnRow, 1);
            grid.Children.Add(btnRow);

            card.Content = grid;
            StudentsListContainer.Children.Add(card);
        }
    }

    // ==========================================
    // TAB 2: MASTER DATA DUDI (FR-MST-04)
    // ==========================================
    private void RenderCompanies()
    {
        CompaniesListContainer.Children.Clear();
        var companies = _dataService.GetCompanies();

        foreach (var c in companies)
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
            
            var titleRow = new HorizontalStackLayout { Spacing = 8 };
            titleRow.Children.Add(new Label
            {
                Text = c.Name,
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#0F172A")
            });
            var statusBadge = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(6) },
                BackgroundColor = c.Status == 1 ? Color.FromArgb("#ECFDF5") : Color.FromArgb("#FEF2F2"),
                Stroke = c.Status == 1 ? Color.FromArgb("#6EE7B7") : Color.FromArgb("#FCA5A5"),
                Padding = new Thickness(6, 2),
                Content = new Label
                {
                    Text = c.StatusText,
                    FontSize = 10,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = c.Status == 1 ? Color.FromArgb("#065F46") : Color.FromArgb("#DC2626")
                }
            };
            titleRow.Children.Add(statusBadge);
            stack.Children.Add(titleRow);

            stack.Children.Add(new Label
            {
                Text = $"Bidang: {c.IndustrySector} • Kuota: {c.Occupied}/{c.Quota} Siswa • Kontak: {c.ContactPerson} ({c.Phone})",
                FontSize = 11,
                TextColor = Color.FromArgb("#2563EB")
            });
            stack.Children.Add(new Label
            {
                Text = $"📍 Alamat: {c.Address} (GPS: {c.Latitude}, {c.Longitude})",
                FontSize = 11,
                TextColor = Color.FromArgb("#64748B")
            });
            stack.Children.Add(new Label
            {
                Text = $"MoU: {c.MouStatus} • Email: {c.Email}",
                FontSize = 11,
                TextColor = Color.FromArgb("#10B981")
            });

            Grid.SetColumn(stack, 0);
            grid.Children.Add(stack);

            var deleteBtn = new Button
            {
                Text = "🗑️ Hapus",
                BackgroundColor = Color.FromArgb("#FEE2E2"),
                TextColor = Color.FromArgb("#DC2626"),
                FontSize = 11,
                HeightRequest = 32,
                Padding = new Thickness(10, 0),
                CornerRadius = 6,
                VerticalOptions = LayoutOptions.Center
            };
            var capturedComp = c;
            deleteBtn.Clicked += async (s, e) =>
            {
                var confirm = await DisplayAlertAsync("Konfirmasi Hapus Mitra",
                    $"Yakin ingin menghapus '{capturedComp.Name}'?\nPer FR-MST-04: Jika perusahaan memiliki riwayat peserta PKL, penghapusan ditolak dan status diubah menjadi Inactive.",
                    "Hapus / Nonaktifkan", "Batal");
                if (!confirm) return;

                var (success, msg) = _dataService.DeleteCompany(capturedComp.Id);
                ShowBanner(msg, isError: !success);
                RefreshAll();
            };
            Grid.SetColumn(deleteBtn, 1);
            grid.Children.Add(deleteBtn);

            card.Content = grid;
            CompaniesListContainer.Children.Add(card);
        }
    }

    // ==========================================
    // TAB 3: MASTER DATA JURUSAN (FR-MST-03)
    // ==========================================
    private void RenderMajors()
    {
        MajorsListContainer.Children.Clear();
        var majors = _dataService.GetMajors();

        foreach (var m in majors)
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
            var titleRow = new HorizontalStackLayout { Spacing = 8 };
            titleRow.Children.Add(new Label
            {
                Text = $"{m.MajorCode} - {m.MajorName}",
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#0F172A")
            });
            var statusBadge = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(6) },
                BackgroundColor = m.Status == 1 ? Color.FromArgb("#ECFDF5") : Color.FromArgb("#FEF2F2"),
                Stroke = m.Status == 1 ? Color.FromArgb("#6EE7B7") : Color.FromArgb("#FCA5A5"),
                Padding = new Thickness(6, 2),
                Content = new Label
                {
                    Text = m.StatusText,
                    FontSize = 10,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = m.Status == 1 ? Color.FromArgb("#065F46") : Color.FromArgb("#DC2626")
                }
            };
            titleRow.Children.Add(statusBadge);
            stack.Children.Add(titleRow);

            stack.Children.Add(new Label
            {
                Text = $"Deskripsi: {m.Description}",
                FontSize = 12,
                TextColor = Color.FromArgb("#475569")
            });

            Grid.SetColumn(stack, 0);
            grid.Children.Add(stack);

            var deleteBtn = new Button
            {
                Text = "🗑️ Hapus",
                BackgroundColor = Color.FromArgb("#FEE2E2"),
                TextColor = Color.FromArgb("#DC2626"),
                FontSize = 11,
                HeightRequest = 32,
                Padding = new Thickness(10, 0),
                CornerRadius = 6,
                VerticalOptions = LayoutOptions.Center
            };
            var capturedMajor = m;
            deleteBtn.Clicked += async (s, e) =>
            {
                var confirm = await DisplayAlertAsync("Konfirmasi Hapus Jurusan",
                    $"Yakin ingin menghapus jurusan '{capturedMajor.MajorName}'?\nPer FR-MST-03: Jika jurusan memiliki relasi kelas aktif, penghapusan ditolak dan status diubah menjadi Inactive.",
                    "Hapus / Nonaktifkan", "Batal");
                if (!confirm) return;

                var (success, msg) = _dataService.DeleteMajor(capturedMajor.Id);
                ShowBanner(msg, isError: !success);
                RefreshAll();
            };
            Grid.SetColumn(deleteBtn, 1);
            grid.Children.Add(deleteBtn);

            card.Content = grid;
            MajorsListContainer.Children.Add(card);
        }
    }

    // ==========================================
    // TAB 4: MASTER DATA KELAS (FR-MST-02)
    // ==========================================
    private void RenderClasses()
    {
        ClassesListContainer.Children.Clear();
        var classes = _dataService.GetClasses();

        foreach (var cl in classes)
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
                Text = $"{cl.ClassName} - {cl.MajorName}",
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#0F172A")
            });
            stack.Children.Add(new Label
            {
                Text = $"Wali Kelas: {cl.HomeroomTeacher} • Tahun Ajaran: {cl.AcademicYear} • Kapasitas: {cl.TotalStudents}/{cl.Capacity} Siswa",
                FontSize = 12,
                TextColor = Color.FromArgb("#475569")
            });

            Grid.SetColumn(stack, 0);
            grid.Children.Add(stack);

            var deleteBtn = new Button
            {
                Text = "🗑️ Hapus",
                BackgroundColor = Color.FromArgb("#FEE2E2"),
                TextColor = Color.FromArgb("#DC2626"),
                FontSize = 11,
                HeightRequest = 32,
                Padding = new Thickness(10, 0),
                CornerRadius = 6,
                VerticalOptions = LayoutOptions.Center
            };
            var capturedClass = cl;
            deleteBtn.Clicked += async (s, e) =>
            {
                var confirm = await DisplayAlertAsync("Konfirmasi Hapus Kelas",
                    $"Yakin ingin menghapus kelas '{capturedClass.ClassName}'?\nPer FR-MST-02: Jika memiliki >= 1 siswa, penghapusan DITOLAK.",
                    "Hapus", "Batal");
                if (!confirm) return;

                var (success, msg) = _dataService.DeleteClass(capturedClass.Id);
                ShowBanner(msg, isError: !success);
                RefreshAll();
            };
            Grid.SetColumn(deleteBtn, 1);
            grid.Children.Add(deleteBtn);

            card.Content = grid;
            ClassesListContainer.Children.Add(card);
        }
    }

    // ==========================================
    // TAB 5: MASTER DATA PEMBIMBING (FR-MST-05)
    // ==========================================
    private void RenderMentors()
    {
        MentorsListContainer.Children.Clear();
        var mentors = _dataService.GetMentors();

        foreach (var m in mentors)
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
            var titleRow = new HorizontalStackLayout { Spacing = 8 };
            titleRow.Children.Add(new Label
            {
                Text = m.MentorName,
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#0F172A")
            });
            var typeBadge = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(6) },
                BackgroundColor = m.MentorType == "Internal" ? Color.FromArgb("#EFF6FF") : Color.FromArgb("#F5F3FF"),
                Stroke = m.MentorType == "Internal" ? Color.FromArgb("#93C5FD") : Color.FromArgb("#DDD6FE"),
                Padding = new Thickness(6, 2),
                Content = new Label
                {
                    Text = m.MentorType == "Internal" ? "Internal (Guru)" : "External (Industri)",
                    FontSize = 10,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = m.MentorType == "Internal" ? Color.FromArgb("#1E40AF") : Color.FromArgb("#6D28D9")
                }
            };
            titleRow.Children.Add(typeBadge);
            stack.Children.Add(titleRow);

            if (m.MentorType == "Internal")
            {
                stack.Children.Add(new Label
                {
                    Text = $"NIP: {m.Nip} • Rombel Bimbingan: {m.AssignedClassName} ({m.AssignedMajorCode})",
                    FontSize = 11,
                    TextColor = Color.FromArgb("#475569")
                });
            }
            else
            {
                stack.Children.Add(new Label
                {
                    Text = $"Perusahaan Mitra: {m.CompanyName} • Siswa Bimbingan: {m.AssignedStudentName} (NISN: {m.AssignedStudentNisn})",
                    FontSize = 11,
                    TextColor = Color.FromArgb("#2563EB")
                });
            }

            Grid.SetColumn(stack, 0);
            grid.Children.Add(stack);

            var deleteBtn = new Button
            {
                Text = "🗑️ Hapus",
                BackgroundColor = Color.FromArgb("#FEE2E2"),
                TextColor = Color.FromArgb("#DC2626"),
                FontSize = 11,
                HeightRequest = 32,
                Padding = new Thickness(10, 0),
                CornerRadius = 6,
                VerticalOptions = LayoutOptions.Center
            };
            var capturedMentor = m;
            deleteBtn.Clicked += async (s, e) =>
            {
                var confirm = await DisplayAlertAsync("Konfirmasi Hapus Pembimbing",
                    $"Yakin ingin menghapus pembimbing '{capturedMentor.MentorName}'?\nPer FR-MST-05: Jika masih membimbing siswa aktif, penghapusan ditolak.",
                    "Hapus", "Batal");
                if (!confirm) return;

                var (success, msg) = _dataService.DeleteMentor(capturedMentor.Id);
                ShowBanner(msg, isError: !success);
                RefreshAll();
            };
            Grid.SetColumn(deleteBtn, 1);
            grid.Children.Add(deleteBtn);

            card.Content = grid;
            MentorsListContainer.Children.Add(card);
        }
    }

    private void LoadDefaultStudentTemplate()
    {
        CsvEditor.Text = "NISN,Nama Siswa,Kelas,Jurusan,Perusahaan Mitra\n" +
                         "0078912345,Muhammad Hafizh,XII RPL 1,Rekayasa Perangkat Lunak,PT Telkom Indonesia\n" +
                         "0076543210,Zahra Aulia,XII RPL 2,Rekayasa Perangkat Lunak,PT Bank Mandiri\n" +
                         "0071122334,Raffi Pratama,XII TKJ 1,Teknik Komputer dan Jaringan,PT Astra International";
    }

    private void OnLoadSampleCsvClicked(object? sender, EventArgs e)
    {
        if (ImportTypePicker.SelectedIndex == 1)
        {
            CsvEditor.Text = "Nama Perusahaan,Bidang Industri,Alamat,Kontak Person,Kuota\n" +
                             "PT Tokopedia Indonesia,E-Commerce & Digital Tech,Tokopedia Tower Jakarta,Andi Wijaya,10\n" +
                             "PT GoTo Gojek Tokopedia,SuperApp & Mobility,Pasaraya Blok M,Citra Kirana,8\n" +
                             "PT Bukalapak.com,Marketplace & B2B,Metropolitan Tower Cilandak,Dedi Prasetyo,6";
        }
        else
        {
            LoadDefaultStudentTemplate();
        }
    }

    private void OnExecuteImportClicked(object? sender, EventArgs e)
    {
        var text = CsvEditor.Text?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(text))
        {
            ShowBanner("Konten CSV kosong. Masukkan data terlebih dahulu.", isError: true);
            return;
        }

        if (ImportTypePicker.SelectedIndex == 1) // Companies
        {
            var (success, errors, msg) = _dataService.ImportCompaniesFromCsv(text);
            ShowBanner(msg, isError: success == 0);
        }
        else // Students
        {
            var (success, errors, msg) = _dataService.ImportStudentsFromCsv(text);
            ShowBanner(msg, isError: success == 0);
        }

        RefreshAll();
    }

    // Prompts for adding data
    private async void OnAddStudentPromptClicked(object? sender, EventArgs e)
    {
        var name = await DisplayPromptAsync("Tambah Siswa Baru", "Nama Lengkap Siswa:");
        if (string.IsNullOrWhiteSpace(name)) return;

        var nisn = await DisplayPromptAsync("Data NISN", "Nomor Induk Siswa Nasional (NISN unik):", initialValue: "007" + Random.Shared.Next(1000000, 9999999));
        if (string.IsNullOrWhiteSpace(nisn)) return;

        var phone = await DisplayPromptAsync("Nomor Telepon", "Nomor Telepon/HP Siswa (unik):", initialValue: "08" + Random.Shared.Next(111111111, 999999999));
        if (string.IsNullOrWhiteSpace(phone)) return;

        var newStu = new MasterStudentModel
        {
            FullName = name,
            Nisn = nisn,
            PhoneNumber = phone,
            ClassName = "XII RPL 1",
            ClassroomId = "CLS-RPL1",
            Major = "Rekayasa Perangkat Lunak",
            CompanyId = "COMP-001",
            CompanyName = "PT Telkom Indonesia",
            InternalMentorId = "USR-002",
            InternalMentorName = "Drs. Bambang Hidayat, M.Kom",
            ExternalMentorId = "USR-003",
            ExternalMentorName = "Hendro Wicaksono, S.T.",
            StartDate = new DateTime(2026, 7, 1),
            EndDate = new DateTime(2026, 9, 30),
            Status = "Active",
            StatusPkl = "Active"
        };

        var (success, msg) = _dataService.AddStudent(newStu);
        ShowBanner(msg, isError: !success);
        RefreshAll();
    }

    private async void OnAddCompanyPromptClicked(object? sender, EventArgs e)
    {
        var name = await DisplayPromptAsync("Tambah Mitra Perusahaan", "Nama Perusahaan (DUDI):");
        if (string.IsNullOrWhiteSpace(name)) return;

        var sector = await DisplayPromptAsync("Bidang Industri", "Contoh: Software Development & IT Services");
        if (string.IsNullOrWhiteSpace(sector)) return;

        var newComp = new CompanyPartnerModel
        {
            Name = name,
            IndustrySector = sector,
            Address = "Kawasan Bisnis & Industri Jakarta",
            ContactPerson = "HR & Internship Operations",
            Phone = "+62 812-0011-2233",
            Email = "internship@partner.co.id",
            Quota = 10,
            Occupied = 0,
            Status = 1,
            MouStatus = "Aktif (MoU 2026/2027)"
        };

        var (success, msg) = _dataService.AddCompanyPartner(newComp);
        ShowBanner(msg, isError: !success);
        RefreshAll();
    }

    private async void OnAddMajorPromptClicked(object? sender, EventArgs e)
    {
        var code = await DisplayPromptAsync("Tambah Jurusan Baru", "Kode Jurusan (Contoh: ANIMASI, TKRO):");
        if (string.IsNullOrWhiteSpace(code)) return;

        var name = await DisplayPromptAsync("Nama Kompetensi Keahlian", "Nama Jurusan Lengkap:");
        if (string.IsNullOrWhiteSpace(name)) return;

        var newMajor = new VocationalMajorModel
        {
            MajorCode = code.ToUpperInvariant().Trim(),
            MajorName = name.Trim(),
            Description = "Kompetensi keahlian vokasi terpadu",
            Status = 1
        };

        var (success, msg) = _dataService.AddMajor(newMajor);
        ShowBanner(msg, isError: !success);
        RefreshAll();
    }

    private async void OnAddClassPromptClicked(object? sender, EventArgs e)
    {
        var className = await DisplayPromptAsync("Tambah Rombel Kelas Baru", "Nama Kelas (Contoh: XII RPL 3):");
        if (string.IsNullOrWhiteSpace(className)) return;

        var homeroom = await DisplayPromptAsync("Wali Kelas", "Nama Guru Wali Kelas:");
        if (string.IsNullOrWhiteSpace(homeroom)) homeroom = "Guru Wali";

        var newClass = new DepartmentClassModel
        {
            ClassName = className.Trim(),
            MajorName = "Rekayasa Perangkat Lunak (RPL)",
            AcademicYear = "2026/2027",
            HomeroomTeacher = homeroom.Trim(),
            Capacity = 36,
            TotalStudents = 0
        };

        var (success, msg) = _dataService.AddClass(newClass);
        ShowBanner(msg, isError: !success);
        RefreshAll();
    }

    private async void OnAddMentorPromptClicked(object? sender, EventArgs e)
    {
        var name = await DisplayPromptAsync("Tambah Pembimbing Baru", "Nama Lengkap Pembimbing:");
        if (string.IsNullOrWhiteSpace(name)) return;

        var type = await DisplayActionSheetAsync("Pilih Jenis Pembimbing", "Batal", null, "Internal (Guru Sekolah)", "External (Mentor Industri)");
        if (type == "Batal" || string.IsNullOrWhiteSpace(type)) return;

        bool isInternal = type.Contains("Internal");
        var nip = isInternal ? await DisplayPromptAsync("Nomor Induk Pegawai", "NIP Guru:") ?? "-" : "-";
        var company = !isInternal ? await DisplayPromptAsync("Perusahaan", "Nama Perusahaan Mitra DUDI:") ?? "Mitra DUDI" : "";

        var newMentor = new MasterMentorModel
        {
            MentorName = name.Trim(),
            MentorType = isInternal ? "Internal" : "External",
            Nip = nip,
            CompanyName = company,
            AssignedClassName = isInternal ? "XII RPL 1" : "",
            AssignedStudentName = !isInternal ? "Siswa Magang" : ""
        };

        var (success, msg) = _dataService.AddMentor(newMentor);
        ShowBanner(msg, isError: !success);
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
        SectionStudents.IsVisible = index == 0;
        SectionCompanies.IsVisible = index == 1;
        SectionMajors.IsVisible = index == 2;
        SectionClasses.IsVisible = index == 3;
        SectionMentors.IsVisible = index == 4;
        SectionImport.IsVisible = index == 5;

        TabStudentsBtn.BackgroundColor = index == 0 ? Color.FromArgb("#1E3A5F") : Color.FromArgb("#F1F5F9");
        TabStudentsBtn.TextColor = index == 0 ? Colors.White : Color.FromArgb("#475569");

        TabCompaniesBtn.BackgroundColor = index == 1 ? Color.FromArgb("#1E3A5F") : Color.FromArgb("#F1F5F9");
        TabCompaniesBtn.TextColor = index == 1 ? Colors.White : Color.FromArgb("#475569");

        TabMajorsBtn.BackgroundColor = index == 2 ? Color.FromArgb("#1E3A5F") : Color.FromArgb("#F1F5F9");
        TabMajorsBtn.TextColor = index == 2 ? Colors.White : Color.FromArgb("#475569");

        TabClassesBtn.BackgroundColor = index == 3 ? Color.FromArgb("#1E3A5F") : Color.FromArgb("#F1F5F9");
        TabClassesBtn.TextColor = index == 3 ? Colors.White : Color.FromArgb("#475569");

        TabMentorsBtn.BackgroundColor = index == 4 ? Color.FromArgb("#1E3A5F") : Color.FromArgb("#F1F5F9");
        TabMentorsBtn.TextColor = index == 4 ? Colors.White : Color.FromArgb("#475569");

        TabImportBtn.BackgroundColor = index == 5 ? Color.FromArgb("#1E3A5F") : Color.FromArgb("#F1F5F9");
        TabImportBtn.TextColor = index == 5 ? Colors.White : Color.FromArgb("#475569");
    }

    private void OnTabStudentsClicked(object? sender, EventArgs e) => SwitchTab(0);
    private void OnTabCompaniesClicked(object? sender, EventArgs e) => SwitchTab(1);
    private void OnTabMajorsClicked(object? sender, EventArgs e) => SwitchTab(2);
    private void OnTabClassesClicked(object? sender, EventArgs e) => SwitchTab(3);
    private void OnTabMentorsClicked(object? sender, EventArgs e) => SwitchTab(4);
    private void OnTabImportClicked(object? sender, EventArgs e) => SwitchTab(5);

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
