using Medieval_Internship.Models;
using Medieval_Internship.Services;

namespace Medieval_Internship;

public partial class MainPage : ContentPage
{
    private readonly AuthService _authService = AuthService.Instance;
    private bool _isPasswordHidden = true;

    public MainPage()
    {
        InitializeComponent();
        PreselectDefaultRole();
    }

    private void PreselectDefaultRole()
    {
        if (LoginRolePicker.Items.Count > 0)
        {
            LoginRolePicker.SelectedIndex = 0; // Default to Siswa
        }
    }

    private void OnTabLoginClicked(object? sender, EventArgs e)
    {
        SetTab(isLogin: true);
    }

    private void OnTabRegisterClicked(object? sender, EventArgs e)
    {
        SetTab(isLogin: false);
    }

    private void OnSwitchToRegisterTapped(object? sender, TappedEventArgs e)
    {
        SetTab(isLogin: false);
    }

    private void OnSwitchToLoginTapped(object? sender, TappedEventArgs e)
    {
        SetTab(isLogin: true);
    }

    private void SetTab(bool isLogin)
    {
        HideStatus();

        if (isLogin)
        {
            TabLoginBtn.BackgroundColor = Color.FromArgb("#1E3A5F");
            TabLoginBtn.TextColor = Colors.White;
            TabRegisterBtn.BackgroundColor = Colors.Transparent;
            TabRegisterBtn.TextColor = Color.FromArgb("#64748B");

            LoginFormSection.IsVisible = true;
            RegisterFormSection.IsVisible = false;
        }
        else
        {
            TabRegisterBtn.BackgroundColor = Color.FromArgb("#1E3A5F");
            TabRegisterBtn.TextColor = Colors.White;
            TabLoginBtn.BackgroundColor = Colors.Transparent;
            TabLoginBtn.TextColor = Color.FromArgb("#64748B");

            LoginFormSection.IsVisible = false;
            RegisterFormSection.IsVisible = true;

            if (RegisterRolePicker.SelectedIndex < 0 && RegisterRolePicker.Items.Count > 0)
            {
                RegisterRolePicker.SelectedIndex = 0;
            }
        }
    }

    private void OnQuickDemoClicked(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is string demoKey)
        {
            SetTab(isLogin: true);

            string username = demoKey.ToLowerInvariant();
            string password = "password";
            string roleName = RoleHelper.RoleSiswa;

            switch (username)
            {
                case "siswa":
                    roleName = RoleHelper.RoleSiswa;
                    break;
                case "guru":
                    roleName = RoleHelper.RoleGuru;
                    break;
                case "industri":
                    roleName = RoleHelper.RoleIndustri;
                    break;
                case "admin":
                    roleName = RoleHelper.RoleAdmin;
                    break;
                case "operator":
                    roleName = RoleHelper.RoleOperator;
                    break;
                case "monitor":
                    roleName = RoleHelper.RoleMonitor;
                    break;
            }

            LoginUsernameEntry.Text = username;
            LoginPasswordEntry.Text = password;
            LoginRolePicker.SelectedItem = roleName;

            ShowStatus($"Akun demo '{roleName}' siap digunakan. Klik Masuk!", isError: false);
        }
    }

    private void OnTogglePasswordClicked(object? sender, EventArgs e)
    {
        _isPasswordHidden = !_isPasswordHidden;
        LoginPasswordEntry.IsPassword = _isPasswordHidden;
        TogglePasswordBtn.Text = _isPasswordHidden ? "👁️" : "🙈";
    }

    private void OnRegisterRoleChanged(object? sender, EventArgs e)
    {
        var role = RegisterRolePicker.SelectedItem?.ToString() ?? string.Empty;

        switch (role)
        {
            case RoleHelper.RoleSiswa:
                RegIdNumberLabel.Text = "Nomor Induk Siswa Nasional (NISN) *";
                RegIdNumberEntry.Placeholder = "Contoh: 0067829102";
                RegOrgLabel.Text = "Asal Sekolah & Kelas *";
                RegOrgEntry.Placeholder = "Contoh: SMK Negeri 1 Jakarta - XII RPL 1";
                break;

            case RoleHelper.RoleGuru:
                RegIdNumberLabel.Text = "Nomor Induk Pegawai (NIP / NUPTK) *";
                RegIdNumberEntry.Placeholder = "Contoh: 19780512 200501 1 003";
                RegOrgLabel.Text = "Asal Sekolah *";
                RegOrgEntry.Placeholder = "Contoh: SMK Negeri 1 Jakarta";
                break;

            case RoleHelper.RoleIndustri:
                RegIdNumberLabel.Text = "ID Karyawan / NIK Pembimbing Industri *";
                RegIdNumberEntry.Placeholder = "Contoh: DUDI-09218";
                RegOrgLabel.Text = "Nama Perusahaan & Divisi *";
                RegOrgEntry.Placeholder = "Contoh: PT Telkom Indonesia (Divisi Cloud)";
                break;

            case RoleHelper.RoleOperator:
                RegIdNumberLabel.Text = "NIP / ID Operator Sekolah *";
                RegIdNumberEntry.Placeholder = "Contoh: OP-SMK-01";
                RegOrgLabel.Text = "Unit Kerja / Lembaga *";
                RegOrgEntry.Placeholder = "Contoh: BKK & Hubungan Industri SMK";
                break;

            case RoleHelper.RoleMonitor:
                RegIdNumberLabel.Text = "NIP / ID Pengawas Monev *";
                RegIdNumberEntry.Placeholder = "Contoh: MONEV-1029";
                RegOrgLabel.Text = "Instansi Pengawas *";
                RegOrgEntry.Placeholder = "Contoh: Dinas Pendidikan Provinsi / Komite";
                break;
        }
    }

    private async void OnLoginClicked(object? sender, EventArgs e)
    {
        HideStatus();

        var username = LoginUsernameEntry.Text?.Trim() ?? string.Empty;
        var password = LoginPasswordEntry.Text?.Trim() ?? string.Empty;
        var role = LoginRolePicker.SelectedItem?.ToString();

        if (string.IsNullOrWhiteSpace(username))
        {
            ShowStatus("Username atau email wajib diisi.", isError: true);
            LoginUsernameEntry.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            ShowStatus("Kata sandi wajib diisi.", isError: true);
            LoginPasswordEntry.Focus();
            return;
        }

        LoginSubmitBtn.IsEnabled = false;
        LoginSubmitBtn.Text = "Memproses Masuk...";

        var (success, message, user) = await _authService.LoginAsync(username, password, role);

        LoginSubmitBtn.IsEnabled = true;
        LoginSubmitBtn.Text = "Masuk ke Sistem PKL →";

        if (!success || user == null)
        {
            ShowStatus(message, isError: true);
            return;
        }

        ShowStatus("Login berhasil! Mengalihkan ke dashboard...", isError: false);

        await Task.Delay(300);
        var targetRoute = RoleHelper.GetDashboardRoute(user.Role);
        await Shell.Current.GoToAsync($"//{targetRoute}");
    }

    private async void OnRegisterClicked(object? sender, EventArgs e)
    {
        HideStatus();

        var roleName = RegisterRolePicker.SelectedItem?.ToString();
        var fullName = RegFullNameEntry.Text?.Trim() ?? string.Empty;
        var username = RegUsernameEntry.Text?.Trim() ?? string.Empty;
        var email = RegEmailEntry.Text?.Trim() ?? string.Empty;
        var idNum = RegIdNumberEntry.Text?.Trim() ?? string.Empty;
        var org = RegOrgEntry.Text?.Trim() ?? string.Empty;
        var pass = RegPasswordEntry.Text ?? string.Empty;
        var confirmPass = RegConfirmPasswordEntry.Text ?? string.Empty;

        if (string.IsNullOrWhiteSpace(roleName))
        {
            ShowStatus("Pilih peran pengguna terlebih dahulu.", isError: true);
            return;
        }

        if (string.IsNullOrWhiteSpace(fullName))
        {
            ShowStatus("Nama lengkap wajib diisi.", isError: true);
            RegFullNameEntry.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(username))
        {
            ShowStatus("Username wajib diisi.", isError: true);
            RegUsernameEntry.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
        {
            ShowStatus("Format email tidak valid.", isError: true);
            RegEmailEntry.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(pass) || pass.Length < 4)
        {
            ShowStatus("Kata sandi minimal 4 karakter.", isError: true);
            RegPasswordEntry.Focus();
            return;
        }

        if (pass != confirmPass)
        {
            ShowStatus("Konfirmasi kata sandi tidak cocok.", isError: true);
            RegConfirmPasswordEntry.Focus();
            return;
        }

        if (!RegAgreeCheck.IsChecked)
        {
            ShowStatus("Anda harus menyetujui ketentuan PKL.", isError: true);
            return;
        }

        RegisterSubmitBtn.IsEnabled = false;
        RegisterSubmitBtn.Text = "Mendaftarkan Akun...";

        string detail = $"{idNum}";
        var (success, message, user) = await _authService.RegisterAsync(
            fullName, username, email, pass, roleName, org, detail);

        RegisterSubmitBtn.IsEnabled = true;
        RegisterSubmitBtn.Text = "Daftar Akun Sekarang →";

        if (!success || user == null)
        {
            ShowStatus(message, isError: true);
            return;
        }

        await DisplayAlertAsync("Registrasi Berhasil 🎉",
            $"Selamat datang, {user.FullName}!\nAkun Anda sebagai {user.RoleName} berhasil didaftarkan.",
            "Buka Dashboard");

        var targetRoute = RoleHelper.GetDashboardRoute(user.Role);
        await Shell.Current.GoToAsync($"//{targetRoute}");
    }

    private void ShowStatus(string message, bool isError)
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

    private void HideStatus()
    {
        StatusBanner.IsVisible = false;
        StatusLabel.Text = string.Empty;
    }

    private async void OnForgotPasswordTapped(object? sender, TappedEventArgs e)
    {
        var identity = await DisplayPromptAsync("Pemulihan Kata Sandi 🔒",
            "Masukkan Username, Email terdaftar, atau NISN/NIP akun Anda:",
            "Lanjut", "Batal", "Contoh: siswa atau rizky.pratama@student.smk.id");
        if (string.IsNullOrWhiteSpace(identity)) return;

        var newPass = await DisplayPromptAsync("Kata Sandi Baru 🔑",
            $"Masukkan kata sandi baru untuk akun '{identity}' (minimal 4 karakter):",
            "Simpan Kata Sandi", "Batal");
        if (string.IsNullOrWhiteSpace(newPass)) return;

        var (success, msg) = await _authService.ResetPasswordAsync(identity, newPass);
        await DisplayAlertAsync(success ? "Kata Sandi Diperbarui ✅" : "Gagal Memulihkan ❌", msg, "OK");
        if (success)
        {
            LoginUsernameEntry.Text = identity;
            LoginPasswordEntry.Text = newPass;
            ShowStatus("Kata sandi berhasil direset! Silakan klik Masuk.", isError: false);
        }
    }
}