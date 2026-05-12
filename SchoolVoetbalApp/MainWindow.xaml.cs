using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace SchoolVoetbalApp
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            // Start op home (voor nu gewoon tekst)
            MainFrame.Content = new TextBlock()
            {
                Text = "Home pagina",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 24
            };
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new TextBlock()
            {
                Text = "Home pagina",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 24
            };
        }

        private void Matches_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(WedstrijdPagina));
        }

        private void Profiel_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new TextBlock()
            {
                Text = "Profiel pagina (komt later)",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 24
            };
        }
    }
}
