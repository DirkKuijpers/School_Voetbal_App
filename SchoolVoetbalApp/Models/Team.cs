namespace SchoolVoetbalApp.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Goals { get; set; }
        public int Wins { get; set; }
    }
}
