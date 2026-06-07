using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SchoolVoetbalApp.Models
{
    public static class Session
    {
        private static readonly string _storagePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SchoolVoetbalApp");
        private static readonly string _usersFile = Path.Combine(_storagePath, "users.json");

        static Session()
        {
            try
            {
                if (!Directory.Exists(_storagePath))
                    Directory.CreateDirectory(_storagePath);

                if (File.Exists(_usersFile))
                {
                    var json = File.ReadAllText(_usersFile);
                    var list = JsonSerializer.Deserialize<List<User>>(json);
                    if (list != null)
                        Users.AddRange(list);
                }
            }
            catch
            {
                // ignore load errors
            }
        }

        public static bool IsLoggedIn { get; set; } = false;

        public static string Username { get; set; } = "Gast";

        private static double _balance = 0;
        public static double Balance
        {
            get => _balance;
            set
            {
                _balance = value;
                var user = CurrentUserInternal();
                if (user != null)
                {
                    user.Balance = _balance;
                    SaveUsers();
                }
                RaiseBalanceChanged();
            }
        }

        public static List<User> Users { get; } = new List<User>();

        private static User? CurrentUserInternal()
        {
            if (!IsLoggedIn) return null;
            return Users.Find(u => u.Username.Equals(Username, StringComparison.OrdinalIgnoreCase));
        }

        public static User? CurrentUser => CurrentUserInternal();

        public static event Action? BalanceChanged;

        public static void RaiseBalanceChanged()
        {
            BalanceChanged?.Invoke();
        }

        public static void SaveUsers()
        {
            try
            {
                var json = JsonSerializer.Serialize(Users);
                File.WriteAllText(_usersFile, json);
            }
            catch
            {
                // ignore save errors
            }
        }
    }
}
