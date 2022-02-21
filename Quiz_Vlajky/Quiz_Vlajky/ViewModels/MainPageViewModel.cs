using Quiz_Vlajky.Models;
using Quiz_Vlajky.Views;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Quiz_Vlajky.ViewModels
{
    public class MainPageViewModel
    {
        public ICommand Click_Command { get; }

        public MainPageViewModel()
        {
            Click_Command = new Command<string>(HandleClick);
        }

        private void HandleClick(string button)
        {
            if (button == "Flags")
            {
                Application.Current.MainPage.Navigation.PushAsync(new FlagsMainPage());
            }
            else if (button == "Networking")
            {
                Application.Current.MainPage.Navigation.PushAsync(new NetworkingMainPage());
            }
        }
    }
}
