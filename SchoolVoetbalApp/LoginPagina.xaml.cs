using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;
using System; 
using SchoolVoetbalApp.Models;

namespace SchoolVoetbalApp
{
    public sealed partial class LoginPagina : Page
    {
        public LoginPagina()
        {
            this.InitializeComponent();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            var username = (this.FindName("UsernameBox") as TextBox)?.Text?.Trim();
            if (string.IsNullOrEmpty(username))
            {
                await ShowError("Voer een gebruikersnaam in.");
                return;
            }

            var password = (this.FindName("PasswordBox") as PasswordBox)?.Password ?? string.Empty;

            // Check user exists
            var user = Session.Users.Find(u => u.Username.Equals(username, System.StringComparison.OrdinalIgnoreCase));
            if (user == null)
            {
                await ShowError("Gebruiker bestaat niet. Maak eerst een account aan.");
                return;
            }

            // Simple password check (plain text for demo; replace with hashing in production)
            if (user.PasswordHash != password)
            {
                await ShowError("Onjuist wachtwoord.");
                return;
            }

            // Login
            Session.IsLoggedIn = true;
            Session.Username = user.Username;
            Session.Balance = user.Balance;

            var status = this.FindName("StatusText") as TextBlock;
            if (status != null)
            {
                status.Text = "Inloggen gelukt.";
                status.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Green);
            }
            await Task.Delay(800);
            this.Frame?.Navigate(typeof(WedstrijdPagina));
        }

        private async Task ShowError(string message)
        {
            ContentDialog error = new ContentDialog()
            {
                Title = "Fout",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };

            await error.ShowAsync();
        }

        private async void Register_Click(object sender, RoutedEventArgs e)
        {
            var username = (this.FindName("UsernameBox") as TextBox)?.Text?.Trim();
            if (string.IsNullOrEmpty(username))
            {
                await ShowError("Voer een gebruikersnaam in om te registreren.");
                return;
            }

            var password = (this.FindName("PasswordBox") as PasswordBox)?.Password ?? string.Empty;
            if (string.IsNullOrEmpty(password))
            {
                await ShowError("Voer een wachtwoord in om te registreren.");
                return;
            }

            var exists = Session.Users.Exists(u => u.Username.Equals(username, System.StringComparison.OrdinalIgnoreCase));
            if (exists)
            {
                await ShowError("Gebruikersnaam bestaat al.");
                return;
            }

            var newUser = new Models.User { Username = username, PasswordHash = password, Balance = 50 };
            Session.Users.Add(newUser);
            // Persist new user to disk
            Session.SaveUsers();

            // Auto-login after register
            Session.IsLoggedIn = true;
            Session.Username = newUser.Username;
            Session.Balance = newUser.Balance;

            var status2 = this.FindName("StatusText") as TextBlock;
            if (status2 != null)
            {
                status2.Text = "Registratie gelukt. Je bent ingelogd.";
                status2.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Green);
            }
            await Task.Delay(800);
            this.Frame?.Navigate(typeof(WedstrijdPagina));
        }
    }
}