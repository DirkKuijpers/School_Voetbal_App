using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SchoolVoetbalApp.Models;

namespace SchoolVoetbalApp
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            // Startpagina
            MainFrame.Navigate(typeof(WedstrijdPagina));

            // Init saldo
            UpdateBalance();
        }

        // 🏠 HOME
        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(WedstrijdPagina));
        }

        // 📋 WEDSTRIJDEN
        private void Matches_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(WedstrijdPagina));
        }

        // 🏆 STAND
        private void Stand_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(StandenPagina));
        }

        // 👤 PROFIEL
        private void Profiel_Click(object sender, RoutedEventArgs e)
        {
            if (!Session.IsLoggedIn)
            {
                MainFrame.Navigate(typeof(LoginPagina));
            }
            else
            {
                MainFrame.Navigate(typeof(ProfielPagina));
            }

            UpdateBalance();
        }

        // 💰 SALDO UPDATE
        private void UpdateBalance()
        {
            if (SaldoText == null) return;

            if (Session.IsLoggedIn)
            {
                SaldoText.Text = $"€{Session.Balance}";
            }
            else
            {
                SaldoText.Text = "€0";
            }
        }

        // 🔄 Handig voor later (bets / updates)
        public void RefreshUI()
        {
            UpdateBalance();
        }
    }
}
