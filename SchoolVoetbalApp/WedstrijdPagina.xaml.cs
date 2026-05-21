using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using SchoolVoetbalApp.Models;

namespace SchoolVoetbalApp
{
    public sealed partial class WedstrijdPagina : Page
    {
        public WedstrijdPagina()
        {
            InitializeComponent();
        }

        private async void Bet_Click(object sender, RoutedEventArgs e)
        {
            // Prevent betting when there is no balance
            if (Session.Balance <= 0)
            {
                await ShowError("Je hebt geen saldo om in te zetten.");
                return;
            }

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
                if (double.TryParse(input.Text, out double bedrag))
                {
                    if (bedrag > 0 && bedrag <= Session.Balance)
                    {
                        Session.Balance -= bedrag;
                        Session.RaiseBalanceChanged();

                        ContentDialog confirm = new ContentDialog()
                        {
                            Title = "Succes!",
                            Content = $"Je hebt €{bedrag} ingezet!\nNieuw saldo: €{Session.Balance}",
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
