using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SchoolVoetbalApp.Models;

namespace SchoolVoetbalApp
{
    public sealed partial class MainWindow : Window
    {
        // Singleton-like instance to allow Pages to request navigation
        public static MainWindow? Instance { get; private set; }

        public MainWindow()
        {
            this.InitializeComponent();

            Instance = this;

            // Try to load image; if missing, keep fallback visible
            // Defer loading image until visual tree ready
            _ = DispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal, () =>
            {
                try
                {
                    var img = (Microsoft.UI.Xaml.Controls.Image)MainFrame?.FindName("LogoImage") ?? null;
                    var fallback = (Microsoft.UI.Xaml.FrameworkElement)MainFrame?.FindName("LogoFallback") ?? null;
                    if (img != null)
                    {
                        var uri = new System.Uri("ms-appx:///Assets/logo.png");
                        img.Source = new Microsoft.UI.Xaml.Media.Imaging.BitmapImage(uri);
                        if (fallback != null) fallback.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
                    }
                }
                catch
                {
                    // ignore and keep fallback
                }
            });

            // Startpagina
            MainFrame.Navigate(typeof(HomePagina));

            // Init saldo and listen for updates
            UpdateBalance();
            Models.Session.BalanceChanged += OnSessionBalanceChanged;
        }

        // Allow other pages to navigate via the main frame
        public void NavigateTo(System.Type pageType, object? parameter = null)
        {
            if (MainFrame != null)
            {
                MainFrame.Navigate(pageType, parameter);
                _ = DispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal, () =>
                {
                    try
                    {
                        var homeBtn = (this.Content as FrameworkElement)?.FindName("HomeButton") as Microsoft.UI.Xaml.Controls.Button;
                        if (homeBtn != null)
                        {
                            var isHome = pageType == typeof(HomePagina);
                            homeBtn.Background = isHome ? new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Green) : new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent);
                        }
                    }
                    catch { }
                });
            }
        }

        private void OnSessionBalanceChanged()
        {
            // Ensure update runs on UI thread
            _ = DispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal, () =>
            {
                UpdateBalance();
            });
        }

        // 🏠 HOME
        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(HomePagina));
        }

        // 📋 WEDSTRIJDEN
        private void Matches_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame != null)
            {
                MainFrame.Navigate(typeof(WedstrijdPagina));
            }
        }

        // 🏆 STAND
        private void Stand_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(StandenPagina));
        }

        // 👤 PROFIEL
        private void Profiel_Click(object sender, RoutedEventArgs e)
        {
            if (!Session.IsLoggedIn)
            {
                MainFrame.Navigate(typeof(LoginPagina));
            }
            else
            {
                MainFrame.Navigate(typeof(ProfielPagina));
            }

            // Update happens via Session.BalanceChanged subscription
        }

        // 💰 SALDO UPDATE
        private void UpdateBalance()
        {
            if (SaldoText == null) return;

            // Always show the current session balance (works for guests and logged-in users)
            SaldoText.Text = $"€{Session.Balance:F2}";

            if (UsernameText != null)
            {
                UsernameText.Text = Session.IsLoggedIn ? $"Ingelogd als: {Session.Username}" : "Niet ingelogd";
            }
        }

        // 🔄 Handig voor later (bets / updates)
        public void RefreshUI()
        {
            UpdateBalance();
        }
    }
}
