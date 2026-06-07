using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using SchoolVoetbalApp.Models;

namespace SchoolVoetbalApp
{
    public sealed partial class WedstrijdDetailPagina : Page
    {
        public WedstrijdDetailPagina()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Intentionally left blank. XAML references this handler (Loaded="Page_Loaded").
            // If initialization on load is required, add code here.
        }

        protected override void OnNavigatedTo(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var matchObj = e.Parameter as Models.MatchView;
            if (matchObj != null)
            {
                var title = this.FindName("MatchTitle") as TextBlock;
                if (title != null) title.Text = matchObj.DisplayName;

                var pred = this.FindName("PredictionText") as TextBlock;
                if (pred != null)
                {
                    // Simple prediction based on wins ratio
                    var teams = FootballDataLoader.LoadTeams();
                    var home = teams.Find(t => t.Name.Equals(matchObj.HomeTeamName, StringComparison.OrdinalIgnoreCase));
                    var away = teams.Find(t => t.Name.Equals(matchObj.AwayTeamName, StringComparison.OrdinalIgnoreCase));
                    if (home != null && away != null)
                    {
                        var totalWins = Math.Max(1, home.Wins + away.Wins);
                        var homePct = (int)Math.Round(100.0 * home.Wins / totalWins);
                        var awayPct = 100 - homePct;
                        pred.Text = $"{home.Name} kans {homePct}% - {away.Name} kans {awayPct}%";
                    }
                    else
                    {
                        pred.Text = "Voorspelling niet beschikbaar";
                    }
                }

                // Store match id/name in page Tag for Bet_Click
                this.Tag = matchObj;
                return;
            }

            // fallback if a string was passed
            var match = e.Parameter as string ?? "Onbekende Wedstrijd";
            var t2 = this.FindName("MatchTitle") as TextBlock;
            if (t2 != null) t2.Text = match;
        }

        private async void Bet_Click(object sender, RoutedEventArgs e)
        {
            // Page.Tag contains MatchView set in OnNavigatedTo
            var match = this.Tag as Models.MatchView;
            if (match == null)
            {
                await ShowError("Geen wedstrijd geselecteerd.");
                return;
            }

            if (Session.Balance <= 0)
            {
                await ShowError("Je hebt geen saldo om in te zetten.");
                return;
            }

            var stack = new StackPanel();
            var amountBox = new TextBox() { PlaceholderText = "Voer bedrag in" };
            var choice = new ComboBox() { SelectedIndex = 0 };
            choice.Items.Add(match.HomeTeamName);
            choice.Items.Add(match.AwayTeamName);
            stack.Children.Add(amountBox);
            stack.Children.Add(choice);

            ContentDialog dialog = new ContentDialog()
            {
                Title = "Zet in - Kies team",
                Content = stack,
                PrimaryButtonText = "OK",
                CloseButtonText = "Annuleer",
                XamlRoot = this.Content.XamlRoot
            };

            var dlgResult = await dialog.ShowAsync();
            if (dlgResult != ContentDialogResult.Primary) return;

            if (!double.TryParse(amountBox.Text, out double bedrag))
            {
                await ShowError("Voer een geldig getal in.");
                return;
            }
            if (bedrag <= 0 || bedrag > Session.Balance)
            {
                await ShowError("Niet genoeg geld of ongeldig bedrag.");
                return;
            }

            var chosenTeam = choice.SelectedItem?.ToString() ?? match.HomeTeamName;

            var betResult = BetService.PlaceBet(match, chosenTeam, bedrag);

            var title = betResult.won ? "Gewonnen" : "Verloren";
            var msg = betResult.won ? $"Gefeliciteerd! {chosenTeam} heeft gewonnen. Je nieuwe saldo: €{betResult.newBalance:F2}" : $"Helaas, {chosenTeam} verloor of er was geen winnaar. Je nieuwe saldo: €{betResult.newBalance:F2}";

            ContentDialog confirm = new ContentDialog()
            {
                Title = title,
                Content = msg,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };

            await confirm.ShowAsync();
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
    }
}
