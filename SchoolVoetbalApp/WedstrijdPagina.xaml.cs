using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

namespace SchoolVoetbalApp
{
    public sealed partial class WedstrijdPagina : Page
    {
        private int saldo = 50; // start geld

        public WedstrijdPagina()
        {
            InitializeComponent();
        }

        private async void Bet_Click(object sender, RoutedEventArgs e)
        {
            TextBox input = new TextBox()
            {
                PlaceholderText = "Voer bedrag in"
            };

            ContentDialog dialog = new ContentDialog()
            {
                Title = "Zet in",
                Content = input,
                PrimaryButtonText = "OK",
                CloseButtonText = "Annuleer",
                XamlRoot = this.Content.XamlRoot
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                if (int.TryParse(input.Text, out int bedrag))
                {
                    if (bedrag > 0 && bedrag <= saldo)
                    {
                        saldo -= bedrag;

                        ContentDialog confirm = new ContentDialog()
                        {
                            Title = "Succes!",
                            Content = $"Je hebt €{bedrag} ingezet!\nNieuw saldo: €{saldo}",
                            CloseButtonText = "OK",
                            XamlRoot = this.Content.XamlRoot
                        };

                        await confirm.ShowAsync();
                    }
                    else
                    {
                        await ShowError("Niet genoeg geld of ongeldig bedrag.");
                    }
                }
                else
                {
                    await ShowError("Voer een geldig getal in.");
                }
            }
        }

        private async Task ShowError(string message)
        {
            ContentDialog error = new ContentDialog()
            {
                Title = "Fout",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };

            await error.ShowAsync();
        }
    }
}
