using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;
using System.Collections.Generic;
using SchoolVoetbalApp.Models;

namespace SchoolVoetbalApp
{
    public sealed partial class HomePagina : Page
    {
        public HomePagina()
        {
            this.InitializeComponent();
            LoadDashboard();
        }

        private void LoadDashboard()
        {
            var username = Session.IsLoggedIn ? Session.Username : "Gast";
            var welcome = this.FindName("WelcomeText") as TextBlock;
            if (welcome != null) welcome.Text = $"Welkom terug, {username}";

            var saldoCard = this.FindName("SaldoCard") as TextBlock;
            if (saldoCard != null) saldoCard.Text = $"€{Session.Balance:F2}";

            var teams = FootballDataLoader.LoadTeams();
            var matches = FootballDataLoader.LoadMatchesResolved();

            var matchesCount = this.FindName("MatchesCount") as TextBlock;
            if (matchesCount != null) matchesCount.Text = matches.Count.ToString();

            var totalGoals = teams.Sum(t => t.Goals);
            var totalGoalsBox = this.FindName("TotalGoals") as TextBlock;
            if (totalGoalsBox != null) totalGoalsBox.Text = totalGoals.ToString();

            var next = matches.FirstOrDefault(m => m.Date > DateTime.Now);
            var nextBox = this.FindName("NextMatch") as TextBlock;
            if (nextBox != null) nextBox.Text = next != null ? $"{next.DisplayName} - {next.Date:g}" : "Geen aankomende wedstrijden";

            var last = matches.LastOrDefault(m => m.Date <= DateTime.Now);
            var lastBox = this.FindName("LastMatch") as TextBlock;
            if (lastBox != null) lastBox.Text = last != null ? $"{last.DisplayName} - {last.Date:g}" : "Nog geen gespeelde wedstrijden";

            var top3 = teams.OrderByDescending(t => t.Goals).Take(3).ToList();
            var topList = this.FindName("TopTeamsList") as ItemsControl;
            if (topList != null) topList.ItemsSource = top3;
        }
    }
}
