using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace SchoolVoetbalApp.Models
{
    public class FootballData
    {
        public List<Team> teams { get; set; } = new List<Team>();
        public List<Match>? matches { get; set; }
        // wedstrijden, gebruikers, voorspellingen
    }

    public static class FootballDataLoader
    {
        public static List<Team> LoadTeams()
        {
            try
            {
                var candidates = new[]
                {
                    Path.Combine(AppContext.BaseDirectory, "Data", "footballData.json"),
                    Path.Combine(Environment.CurrentDirectory, "Data", "footballData.json"),
                    Path.Combine(Directory.GetCurrentDirectory(), "Data", "footballData.json")
                };

                var file = candidates.FirstOrDefault(File.Exists);
                if (file == null)
                    return new List<Team>();

                var json = File.ReadAllText(file);
                var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var data = JsonSerializer.Deserialize<FootballData>(json, opts);
                return data?.teams ?? new List<Team>();
            }
            catch
            {
                return new List<Team>();
            }
        }

        public static List<MatchView> LoadMatchesResolved()
        {
            try
            {
                var candidates = new[]
                {
                    Path.Combine(AppContext.BaseDirectory, "Data", "footballData.json"),
                    Path.Combine(Environment.CurrentDirectory, "Data", "footballData.json"),
                    Path.Combine(Directory.GetCurrentDirectory(), "Data", "footballData.json")
                };

                var file = candidates.FirstOrDefault(File.Exists);
                if (file == null)
                    return new List<MatchView>();

                var json = File.ReadAllText(file);
                var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var data = JsonSerializer.Deserialize<FootballData>(json, opts);
                if (data == null) return new List<MatchView>();

                var teams = data.teams ?? new List<Team>();
                var matches = data.matches ?? new List<Match>();

                var result = new List<MatchView>();
                foreach (var m in matches)
                {
                    var home = teams.Find(t => t.Id == m.homeTeamId)?.Name ?? "Onbekend";
                    var away = teams.Find(t => t.Id == m.awayTeamId)?.Name ?? "Onbekend";
                    result.Add(new MatchView
                    {
                        Id = m.id,
                        HomeTeamName = home,
                        AwayTeamName = away,
                        Date = m.date,
                        Status = m.status,
                        DisplayName = $"{home} vs {away}"
                    });
                }

                return result.OrderBy(m => m.Date).ToList();
            }
            catch
            {
                return new List<MatchView>();
            }
        }

        public static string DetermineWinner(string homeName, string awayName)
        {
            try
            {
                var teams = LoadTeams();
                var home = teams.Find(t => t.Name.Equals(homeName, StringComparison.OrdinalIgnoreCase));
                var away = teams.Find(t => t.Name.Equals(awayName, StringComparison.OrdinalIgnoreCase));
                if (home == null || away == null) return string.Empty;

                if (home.Wins > away.Wins) return home.Name;
                if (away.Wins > home.Wins) return away.Name;

                if (home.Goals > away.Goals) return home.Name;
                if (away.Goals > home.Goals) return away.Name;

                return string.Empty; // draw or unknown
            }
            catch
            {
                return string.Empty;
            }
        }
    }

    public class Match
    {
        public int id { get; set; }
        public int homeTeamId { get; set; }
        public int awayTeamId { get; set; }
        public DateTime date { get; set; }
        public string status { get; set; } = string.Empty;
    }

    public class MatchView
    {
        public int Id { get; set; }
        public string HomeTeamName { get; set; } = string.Empty;
        public string AwayTeamName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Odds => MatchViewExtensions.MatchOdds(this);
    }
}
