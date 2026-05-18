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

        public static double Balance { get; set; } = 0;
    }
}
