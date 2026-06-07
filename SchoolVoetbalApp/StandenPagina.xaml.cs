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
    public sealed partial class StandenPagina : Page
    {
        public StandenPagina()
        {
            InitializeComponent();
            LoadStanden();
        }

        private void LoadDemoStanden()
        {
            // demo data for top wins
            var wins = new List<dynamic>
            {
                new { Rank = 1, Team = "Team A", Wins = 22 },
                new { Rank = 2, Team = "Team B", Wins = 20 },
                new { Rank = 3, Team = "Team C", Wins = 18 },
                new { Rank = 4, Team = "Team D", Wins = 17 },
                new { Rank = 5, Team = "Team E", Wins = 15 },
                new { Rank = 6, Team = "Team F", Wins = 14 },
                new { Rank = 7, Team = "Team G", Wins = 13 },
                new { Rank = 8, Team = "Team H", Wins = 12 },
                new { Rank = 9, Team = "Team I", Wins = 11 },
                new { Rank = 10, Team = "Team J", Wins = 10 }
            };

            var goals = new List<dynamic>
            {
                new { Rank = 1, Team = "Team A", Goals = 68 },
                new { Rank = 2, Team = "Team C", Goals = 60 },
                new { Rank = 3, Team = "Team B", Goals = 58 },
                new { Rank = 4, Team = "Team E", Goals = 54 },
                new { Rank = 5, Team = "Team D", Goals = 50 },
                new { Rank = 6, Team = "Team F", Goals = 48 },
                new { Rank = 7, Team = "Team G", Goals = 45 },
                new { Rank = 8, Team = "Team H", Goals = 43 },
                new { Rank = 9, Team = "Team I", Goals = 39 },
                new { Rank = 10, Team = "Team J", Goals = 37 }
            };

            var ctlWins = this.FindName("TopWinsList") as ItemsControl;
            if (ctlWins != null) ctlWins.ItemsSource = wins;

            var ctlGoals = this.FindName("TopGoalsList") as ItemsControl;
            if (ctlGoals != null) ctlGoals.ItemsSource = goals;

            // create demo promotion points using the demo wins list
            var points = new System.Collections.Generic.List<dynamic>();
            int pRank = 1;
            int ptsValue = 80;
            foreach (var item in wins.Take(10))
            {
                points.Add(new { Rank = pRank++, Team = item.Team, Points = ptsValue });
                ptsValue -= 3;
            }
            var ctlPoints = this.FindName("TopPointsList") as ItemsControl;
            if (ctlPoints != null) ctlPoints.ItemsSource = points;
        }

        private void LoadStanden()
        {
            var teams = Models.FootballDataLoader.LoadTeams();
            if (teams == null || teams.Count == 0)
            {
                // fallback to demo
                LoadDemoStanden();
                return;
            }

            var wins = new System.Collections.Generic.List<dynamic>();
            var goals = new System.Collections.Generic.List<dynamic>();

            int rank = 1;
            foreach (var t in teams.OrderByDescending(t => t.Wins).Take(10))
            {
                wins.Add(new { Rank = rank++, Team = t.Name, Wins = t.Wins });
            }

            rank = 1;
            foreach (var t in teams.OrderByDescending(t => t.Goals).Take(10))
            {
                goals.Add(new { Rank = rank++, Team = t.Name, Goals = t.Goals });
            }

            var ctlWins = this.FindName("TopWinsList") as ItemsControl;
            if (ctlWins != null) ctlWins.ItemsSource = wins;

            var ctlGoals = this.FindName("TopGoalsList") as ItemsControl;
            if (ctlGoals != null) ctlGoals.ItemsSource = goals;

            // create made-up promotion points using team names from loaded teams
            var points = new System.Collections.Generic.List<dynamic>();
            rank = 1;
            int ptsValue2 = 80;
            foreach (var name in teams.OrderByDescending(t => t.Wins).ThenByDescending(t => t.Goals).Select(t => t.Name).Distinct().Take(10))
            {
                points.Add(new { Rank = rank++, Team = name, Points = ptsValue2 });
                ptsValue2 -= 3;
            }
            var ctlPoints = this.FindName("TopPointsList") as ItemsControl;
            if (ctlPoints != null) ctlPoints.ItemsSource = points;
        }
    }
}
