using Quiz_Vlajky.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Quiz_Vlajky.ViewModels
{
    public class FlagsPlayingPageViewModel : INotifyPropertyChanged
    {
        private const int OptionsCount = 4;
        private Color _defaultColor = Color.FromHex("#101820");
        private Color _rightColor = Color.Green;
        private Color _wrongColor = Color.DarkRed;

        public int TotalRounds => 10;
        public int CurrentRound { get; private set; }

        public Color BackgroundColor { get; private set; }

        public Country[] Options { get; private set; }
        public Country CorrectOption { get; private set; }

        public ICommand SelectFlagCommand { get; }

        private readonly Random _rng;
        private int _correctAnswers;
        
        public static List<Country> AllCountries { get; set; }
        private List<Country> _alreadyUsedCountries = new List<Country>();

        public FlagsPlayingPageViewModel()
        {
            CurrentRound = 1;
            BackgroundColor = _defaultColor;

            _rng = new Random();
            _correctAnswers = 0;

            SelectRandomCountries();

            SelectFlagCommand = new Command<string>(HandleSelectFlag);
        }

        private async void HandleSelectFlag(string flagIndexRaw)
        {
            var flagIndex = int.Parse(flagIndexRaw);

            if (flagIndex >= OptionsCount)
                return;

            var selectedCountry = Options[flagIndex];
            CheckAndHandleAnswer(selectedCountry);

            if (TotalRounds >= ++CurrentRound)
                SelectRandomCountries();
            else
                await HandleGameEnd();

            RaiseAllPropertyChanged();
        }

        private void CheckAndHandleAnswer(Country country)
        {
            if (country == CorrectOption)
            {
                ++_correctAnswers;
                BackgroundColor = _rightColor;
            }
            else BackgroundColor = _wrongColor;

            Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
            {
                BackgroundColor = _defaultColor;
                RaisePropertyChanged(nameof(BackgroundColor));
                return false;
            });
        }

        private async Task HandleGameEnd()
        {
            var alert = await DisplayAlert("Congratulations",
                $"You've chosen {_correctAnswers}/{TotalRounds} correctly!", "Play Again", "Back");

            _correctAnswers = 0;
            CurrentRound = 1;

            if (alert)
            {
                _alreadyUsedCountries.Clear();
                SelectRandomCountries();
            }
            else
                await Shell.Current.Navigation.PopAsync();

        }

        private void SelectRandomCountries()
        {
            Options = GenerateOptions().ToArray();

            var index = _rng.Next(0, OptionsCount);
            CorrectOption = Options[index];

            _alreadyUsedCountries.Add(CorrectOption);
        }

        private List<Country> GenerateOptions()
        {
            var availableCountries = new List<Country>(AllCountries);

            if (availableCountries.Count <= OptionsCount)
                return availableCountries;

            var selectedCountries = new List<Country>();

            for (var i = 0; i < OptionsCount; i++)
            {
                var count = availableCountries.Count;
                var index = _rng.Next(0, count);
                var country = availableCountries[index];

                if (_alreadyUsedCountries.Contains(country))
                {
                    i--;
                    continue;
                }

                availableCountries.Remove(country);
                selectedCountries.Add(country);
            }

            return selectedCountries;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public void RaiseAllPropertyChanged() =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));

        public Task<bool> DisplayAlert(string title, string message, string accept, string cancel) =>
            Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
    }
}