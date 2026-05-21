using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolVoetbalApp.Models
{
    public static class Session
    {
        public static bool IsLoggedIn { get; set; } = false;

        public static string Username { get; set; } = "Gast";
        // Default starting balance for guest users
        public static double Balance { get; set; } = 50;

        // Notifies UI when balance changes so nav can update in real-time
        public static event System.Action? BalanceChanged;

        public static void RaiseBalanceChanged()
        {
            BalanceChanged?.Invoke();
        }
    }
}
