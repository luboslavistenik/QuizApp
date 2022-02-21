﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Quiz_Vlajky.Models;
using Quiz_Vlajky.Views;
using Xamarin.Forms;

namespace Quiz_Vlajky.ViewModels
{
    public class NetworkingMainPageViewModel
    {
        public ICommand Click_Command { get; }
        public NetworkingMainPageViewModel()
        {
            Click_Command = new Command<string>(HandleClick);
        }
        private async void HandleClick(string button)
        {
            var category = (Category)Enum.Parse(typeof(Category), button);
            var databaseService = (Application.Current as App).DatabaseService;

            PlayingPageViewModel.Questions = await databaseService.GetQuestionsByCategory(category);
            await Shell.Current.Navigation.PushAsync(new PlayingPage());
        }
    }
}