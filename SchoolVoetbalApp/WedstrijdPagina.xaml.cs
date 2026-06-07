using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;
using System.Threading.Tasks;
using SchoolVoetbalApp.Models;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System.Collections.Generic;

namespace SchoolVoetbalApp
{
    public sealed partial class WedstrijdPagina : Page
    {

        public record RecentResult(string HomeTeam, string AwayTeam, string Score, string Date);

        public WedstrijdPagina()
        {
            InitializeComponent();
            LoadTopTeams();
        }

        protected override void OnNavigatedTo(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            LoadMatches();
        }

        private void LoadMatches()
        {
            var matches = FootballDataLoader.LoadMatchesResolved();
            var ctl = this.FindName("MatchesList") as ItemsControl;
            if (ctl == null) return;

            if (matches == null || matches.Count == 0)
            {
                var demo = new System.Collections.Generic.List<MatchView>
                {
                    new MatchView { DisplayName = "Team A vs Team B", HomeTeamName = "Team A", AwayTeamName = "Team B" },
                    new MatchView { DisplayName = "Team C vs Team D", HomeTeamName = "Team C", AwayTeamName = "Team D" },
                    new MatchView { DisplayName = "Team E vs Team F", HomeTeamName = "Team E", AwayTeamName = "Team F" }
                };
                ctl.ItemsSource = demo;
                return;
            }

            ctl.ItemsSource = matches.Take(6).ToList();
            PopulateRecentResults();
        }

        private void PopulateRecentResults()
        {
            var ctl = this.FindName("ResultsGrid") as ItemsControl;
            if (ctl == null) return;
            var recent = new List<RecentResult>
            {
                new RecentResult("Ajax","PSV","3 - 1","2026-05-21"),
                new RecentResult("Feyenoord","AZ","2 - 0","2026-05-20"),
                new RecentResult("Utrecht","NAC","1 - 1","2026-05-19"),
                new RecentResult("Twente","RKC","4 - 2","2026-05-18"),
                new RecentResult("PEC","Ajax","0 - 2","2026-05-17"),
                new RecentResult("AZ","Feyenoord","2 - 2","2026-05-16")
            };
            ctl.ItemsSource = recent;
        }

        private void ViewMatch_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var match = btn?.Tag as MatchView;
            if (match == null) return;

            try
            {
                MainWindow.Instance?.NavigateTo(typeof(WedstrijdDetailPagina), match);
            }
            catch
            {
                (Window.Current?.Content as Frame)?.Navigate(typeof(WedstrijdDetailPagina), match);
            }
        }

        private void LoadTopTeams()
        {
            var teams = FootballDataLoader.LoadTeams();
            var ctl = this.FindName("TopTeamsList") as ItemsControl;
            if (teams == null || teams.Count == 0)
            {
                var list = new System.Collections.Generic.List<Team>
                {
                    new Team { Name = "Team A", Goals = 34 },
                    new Team { Name = "Team C", Goals = 29 },
                    new Team { Name = "Team E", Goals = 26 }
                };
                if (ctl != null) ctl.ItemsSource = list;
                return;
            }

            var top = teams.OrderByDescending(t => t.Goals).Take(5).ToList();
            if (ctl != null) ctl.ItemsSource = top;
        }

        private async void Bet_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var match = btn?.Tag as MatchView;
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

            // Simple dialog: amount + team choice
            var stack = new StackPanel();
            var amountBox = new TextBox() { PlaceholderText = "Voer bedrag in" };
            var choice = new ComboBox() { SelectedIndex = 0 };
            choice.Items.Add(match.HomeTeamName);
            choice.Items.Add(match.AwayTeamName);
            stack.Children.Add(amountBox);
            stack.Children.Add(choice);

            var dialog = new ContentDialog()
            {
                Title = "Zet in - Kies team",
                Content = stack,
                PrimaryButtonText = "OK",
                CloseButtonText = "Annuleer",
                XamlRoot = this.Content.XamlRoot
            };

            var res = await dialog.ShowAsync();
            if (res != ContentDialogResult.Primary) return;

            if (!double.TryParse(amountBox.Text, out double bedrag) || bedrag <= 0 || bedrag > Session.Balance)
            {
                await ShowError("Voer een geldig bedrag in binnen je saldo.");
                return;
            }

            var chosenTeam = choice.SelectedItem?.ToString() ?? match.HomeTeamName;

            var result = BetService.PlaceBet(match, chosenTeam, bedrag);

            var title = result.won ? "Gewonnen" : "Verloren";
            var msg = result.won ? $"Gefeliciteerd! {chosenTeam} heeft gewonnen. Je nieuwe saldo: €{result.newBalance:F2}" : $"Helaas, {chosenTeam} verloor of er was geen winnaar. Je nieuwe saldo: €{result.newBalance:F2}";

            var confirm = new ContentDialog()
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

        private void Card_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Border b)
            {
                b.Opacity = 0.98;
                if (b.RenderTransform is ScaleTransform st)
                {
                    st.ScaleX = 1.02;
                    st.ScaleY = 1.02;
                }
            }
        }

        private void Card_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Border b)
            {
                b.Opacity = 1.0;
                if (b.RenderTransform is ScaleTransform st)
                {
                    st.ScaleX = 1.0;
                    st.ScaleY = 1.0;
                }
            }
        }
    }
}
