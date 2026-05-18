using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolVoetbalApp.Models
{
    public class Bet
    {
        public string MatchName { get; set; }

        public string ChosenTeam { get; set; }

        public double Amount { get; set; }

        public bool Won { get; set; }

    }
}
