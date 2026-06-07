using System;
namespace SchoolVoetbalApp.Models
{
    public static class BetService
    {
        public static (bool won, double newBalance, string winner) PlaceBet(MatchView match, string chosenTeam, double amount)
        {
            if (match == null) throw new ArgumentNullException(nameof(match));

            Session.Balance -= amount;
            Session.RaiseBalanceChanged();

            // Welk team wint bepalen
            var winner = FootballDataLoader.DetermineWinner(match.HomeTeamName, match.AwayTeamName);
            bool won = !string.IsNullOrEmpty(winner) && string.Equals(winner, chosenTeam, StringComparison.OrdinalIgnoreCase);

            if (won)
            {
                // 2x inzet uitbetalen
                Session.Balance += amount * 2;
                Session.RaiseBalanceChanged();
            }

            var bet = new Bet
            {
                MatchName = match.DisplayName,
                ChosenTeam = chosenTeam,
                Amount = amount,
                Won = won,
                IsSettled = true,
                OwnerUsername = Session.Username ?? string.Empty
            };
            BetHistory.Bets.Add(bet);

            return (won, Session.Balance, winner);
        }
    }
}
