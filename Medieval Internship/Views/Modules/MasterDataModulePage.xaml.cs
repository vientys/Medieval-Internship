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
        RenderClasses();
    }

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

            Grid.SetColumn(stack, 0);
            grid.Children.Add(stack);

            var editBtn = new Button
            {
                Text = "Detail",
                BackgroundColor = Color.FromArgb("#F1F5F9"),
                TextColor = Color.FromArgb("#334155"),
                FontSize = 11,
                HeightRequest = 32,
                Padding = new Thickness(12, 0),
                CornerRadius = 6,
                VerticalOptions = LayoutOptions.Center
            };
            var capturedStudent = s;
            editBtn.Clicked += async (sender, e) =>
            {
                await DisplayAlertAsync("Data Siswa PKL",
                    $"Nama: {capturedStudent.FullName}\nNISN: {capturedStudent.Nisn}\nKelas: {capturedStudent.ClassName}\nJurusan: {capturedStudent.Major}\nTempat PKL: {capturedStudent.CompanyName}\nGuru Pembimbing: {capturedStudent.InternalMentorName}\nMentor Industri: {capturedStudent.ExternalMentorName}\nStatus: {capturedStudent.StatusPkl}",
                    "Tutup");
            };
            Grid.SetColumn(editBtn, 1);
            grid.Children.Add(editBtn);

            card.Content = grid;
            StudentsListContainer.Children.Add(card);
        }
    }

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

            var stack = new VerticalStackLayout { Spacing = 3 };
            stack.Children.Add(new Label
            {
                Text = c.Name,
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#0F172A")
            });
            stack.Children.Add(new Label
            {
                Text = $"Bidang: {c.IndustrySector} • Kuota: {c.Occupied}/{c.Quota} Siswa",
                FontSize = 11,
                TextColor = Color.FromArgb("#2563EB")
            });
            stack.Children.Add(new Label
            {
                Text = $"📍 Alamat: {c.Address}",
                FontSize = 11,
                TextColor = Color.FromArgb("#64748B")
            });
            stack.Children.Add(new Label
            {
                Text = $"Kontak: {c.ContactPerson} ({c.Phone}) • MoU: {c.MouStatus}",
                FontSize = 11,
                TextColor = Color.FromArgb("#10B981")
            });

            card.Content = stack;
            CompaniesListContainer.Children.Add(card);
        }
    }

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
                Text = $"Wali Kelas: {cl.HomeroomTeacher} • Total Siswa: {cl.TotalStudents} Orang",
                FontSize = 12,
                TextColor = Color.FromArgb("#475569")
            });

            card.Content = stack;
            ClassesListContainer.Children.Add(card);
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

    private async void OnAddStudentPromptClicked(object? sender, EventArgs e)
    {
        var name = await DisplayPromptAsync("Tambah Siswa Baru", "Nama Lengkap Siswa:");
        if (string.IsNullOrWhiteSpace(name)) return;

        var nisn = await DisplayPromptAsync("Data NISN", "Nomor Induk Siswa Nasional (NISN):", initialValue: "007" + Random.Shared.Next(1000000, 9999999));
        if (string.IsNullOrWhiteSpace(nisn)) return;

        var newStu = new MasterStudentModel
        {
            FullName = name,
            Nisn = nisn,
            ClassName = "XII RPL 1",
            Major = "Rekayasa Perangkat Lunak",
            CompanyName = "PT Telkom Indonesia",
            InternalMentorName = "Drs. Bambang Hidayat, M.Kom",
            ExternalMentorName = "Hendro Wicaksono, S.T."
        };

        _dataService.AddStudent(newStu);
        ShowBanner($"Siswa {name} ({nisn}) berhasil ditambahkan ke database.", isError: false);
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
            Address = "Kawasan Industri & Bisnis Jakarta",
            ContactPerson = "HR & Internship Team",
            Quota = 10,
            Occupied = 0,
            MouStatus = "Aktif (MoU 2026/2027)"
        };

        _dataService.AddCompany(newComp);
        ShowBanner($"Mitra Perusahaan {name} berhasil didaftarkan.", isError: false);
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
        SectionImport.IsVisible = index == 2;
        SectionClasses.IsVisible = index == 3;

        TabStudentsBtn.BackgroundColor = index == 0 ? Color.FromArgb("#1E3A5F") : Color.FromArgb("#F1F5F9");
        TabStudentsBtn.TextColor = index == 0 ? Colors.White : Color.FromArgb("#475569");

        TabCompaniesBtn.BackgroundColor = index == 1 ? Color.FromArgb("#1E3A5F") : Color.FromArgb("#F1F5F9");
        TabCompaniesBtn.TextColor = index == 1 ? Colors.White : Color.FromArgb("#475569");

        TabImportBtn.BackgroundColor = index == 2 ? Color.FromArgb("#1E3A5F") : Color.FromArgb("#F1F5F9");
        TabImportBtn.TextColor = index == 2 ? Colors.White : Color.FromArgb("#475569");

        TabClassesBtn.BackgroundColor = index == 3 ? Color.FromArgb("#1E3A5F") : Color.FromArgb("#F1F5F9");
        TabClassesBtn.TextColor = index == 3 ? Colors.White : Color.FromArgb("#475569");
    }

    private void OnTabStudentsClicked(object? sender, EventArgs e) => SwitchTab(0);
    private void OnTabCompaniesClicked(object? sender, EventArgs e) => SwitchTab(1);
    private void OnTabImportClicked(object? sender, EventArgs e) => SwitchTab(2);
    private void OnTabClassesClicked(object? sender, EventArgs e) => SwitchTab(3);

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
