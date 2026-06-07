using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SchoolVoetbalApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProfielPagina : Page
    {
        public ProfielPagina()
        {
            InitializeComponent();
            LoadBets();
        }

        private void LoadBets()
        {
            var usernameText = this.FindName("UsernameText") as TextBlock;
            if (usernameText != null)
            {
                usernameText.Text = $"Gebruiker: {Models.Session.Username}";
            }

            var saldoText = this.FindName("ProfileSaldoText") as TextBlock;
            if (saldoText != null)
            {
                saldoText.Text = $"Saldo: €{Models.Session.Balance:F2}";
            }

            var betsList = this.FindName("BetsList") as ItemsControl;
            if (betsList != null)
            {
                // show bets only for current user
                var username = Models.Session.Username ?? string.Empty;
                var list = Models.BetHistory.Bets.FindAll(b => b.OwnerUsername == username);
                betsList.ItemsSource = list;
            }
        }
    }
}
