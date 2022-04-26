using System;
using Quiz_Vlajky.Views;
using System.Windows.Input;
using Quiz_Vlajky.Models;
using Xamarin.Forms;

namespace Quiz_Vlajky.ViewModels
{
    public class FlagsMainPageViewModel
    {
        public ICommand Click_Command { get; }
        
        public FlagsMainPageViewModel()
        {
            Click_Command = new Command<string>(HandleClick);
        }
        
        private async void HandleClick(string button)
        {
            var databaseService = (Application.Current as App).DatabaseService;
            
            if (button != null)
            {
                var category = (CountryCategory) Enum.Parse(typeof(CountryCategory), button);
                FlagsPlayingPageViewModel.AllCountries = await databaseService.GetCountriesByCategory(category);    
            }
            else
            {
                FlagsPlayingPageViewModel.AllCountries = await databaseService.GetAllCountries();
            }
            
            await Shell.Current.Navigation.PushAsync(new FlagsPlayingPage());
        }
    }
}