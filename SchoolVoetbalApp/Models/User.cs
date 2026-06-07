using System;
namespace SchoolVoetbalApp.Models
{
    public class User
    {
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public double Balance { get; set; } = 50; // standaard inlog balans
    }
}
