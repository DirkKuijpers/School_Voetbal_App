using System;
using System.Linq;

namespace SchoolVoetbalApp.Models
{
    public static class MatchViewExtensions
    {
        public static string MatchOdds(this MatchView m)
        {
            try
            {
                var teams = FootballDataLoader.LoadTeams();
                var home = teams.Find(t => t.Name.Equals(m.HomeTeamName, StringComparison.OrdinalIgnoreCase));
                var away = teams.Find(t => t.Name.Equals(m.AwayTeamName, StringComparison.OrdinalIgnoreCase));
                if (home == null || away == null) return "Odds: 1.5 - 2.5";

                var total = Math.Max(1, home.Wins + away.Wins);
                var homePct = (double)home.Wins / total;
                var homeOdd = Math.Round(1.2 + (1 - homePct) * 2.0, 2);
                var awayOdd = Math.Round(1.2 + homePct * 2.0, 2);
                return $"Odds: {homeOdd} - {awayOdd}";
            }
            catch
            {
                return "Odds: 1.5 - 2.5";
            }
        }
    }
}
