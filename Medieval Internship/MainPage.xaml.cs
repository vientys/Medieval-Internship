namespace Medieval_Internship; RICHI HAMA

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        if (RolePicker.SelectedItem is null)
        {
            StatusLabel.Text = "Pilih role dulu.";
            return;
        }

        if (string.IsNullOrWhiteSpace(UsernameEntry.Text) ||
            string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            StatusLabel.Text = "Username dan password wajib diisi.";
            return;
        }

        // TODO: nanti dipanggil ke API auth (JWT)
        StatusLabel.TextColor = Colors.Green;
        StatusLabel.Text = $"Login sbg {RolePicker.SelectedItem} (dummy, belum ke API)";

        await Task.Delay(500);
    }
}
