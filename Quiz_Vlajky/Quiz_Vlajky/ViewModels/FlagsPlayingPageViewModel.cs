using Quiz_Vlajky.Models;
using System;
using System.Linq;
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

        public static List<Country> _countries { get; set; }

        public static string selectedContinent { get; set; }

        private List<Country> GetContinent()
        {
            switch (selectedContinent)
            {
                case "Europe":
                    return _countries = _europeCountries;
                case "Asia":
                    return _countries = _asiaCountries;
                case "America":
                    return _countries = _americaCountries;
                case "Africa":
                    return _countries = _africaCountries;
            }
            return _countries = _worldCountries;

        }

        private List<Country> _alreadyUsedCountries = new List<Country>();

        private List<Country> GenerateOptions()
        {
            _countries = GetContinent();

            var availableCountries = new List<Country>(_countries);

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

        private readonly List<Country> _europeCountries = new List<Country>
        {
            "Albania", "Andorra", "Austria", "Belarus", "Belgium", "Bosnia", "Croatia",
            "Czechia", "Denmark", "Estonia", "Finland", "France", "Germany", "Greece", "Hungary",
            "Iceland", "Ireland", "Italy", "Latvia", "Liechtenstein", "Lithuania", "Luxembourg", "Malta", "Moldova",
            "Montenegro", "Netherlands", "Macedonia", "Norway", "Poland", "Portugal",
            "Romania", "Russia", "San_Marino", "Serbia", "Slovakia", "Slovenia", "Spain", "Sweden", "Ukraine",
            "United_Kingdom"
        };

        private readonly List<Country> _asiaCountries = new List<Country>
        {
            "Afghanistan", "Armenia", "Azerbaijan", "Bangladesh", "Bhutan", "Brunei", "Cambodia", "Cyprus",
            "East_Timor", "Georgia", "China", "India", "Indonesia", "Iran", "Iraq", "Israel", "Japan",
            "Jordan", "Kazachstan", "Kuwait", "Kyrgyzstan", "Laos", "Lebanon", "Malaysia", "Maldives",
            "Mongolia", "Myanmar", "North_Korea", "Oman", "Pakistan", "Palestine", "Philippines", "Russia",
            "Saudi_Arabia", "Singapore", "South_Korea", "Sri_Lanka", "Syria", "Taiwan", "Tajikistan",
            "Thailand", "Turkey", "Turkmenistan", "Uzbekistan", "Vietnam", "Yemen"
        };

        private readonly List<Country> _americaCountries = new List<Country>
        {
            "Anguilla", "Argentina", "Aruba", "Bahamas", "Barbados", "Belize", "Bolivia", "Bonaire",
            "Brazil", "Canada", "Colombia", "Costa_Rica", "Cuba", "Curacao", "Dominica", "Dominican_Republic",
            "Ecuador", "El_Salvador", "Falkland_Islands", "Grenada", "Guatemala", "Guyana", "Haiti", "Honduras",
            "Chile", "Jamaica", "Martinique", "Mexico", "Monsterrat", "Nicaragua", "Panama", "Paraguay", "Peru",
            "Puerto_Rico", "Saba", "Saint_Lucia", "Sint_Eustatius", "Sint_Maarten", "Suriname", "United_States",
            "Uruguay", "Venezuela"
        };

        private readonly List<Country> _africaCountries = new List<Country>
        {
            "Algeria", "Angola", "Benin", "Botswana", "Burkina_Faso", "Burundi", "Cameroon", "Cape_Verde",
            "Central_Africa", "Comoros", "Congo", "Djibouti", "Egypt", "Equatorial_Guinea", "Eritrea",
            "Eswatini", "Ethiopia", "Gabon", "Gambia", "Ghana", "Guinea", "Guinea_Bissau", "Chad",
            "Ivory_Coast", "Kenya", "Lesotho", "Liberia", "Libya", "Madagascar", "Malawi", "Mali",
            "Mauritania", "Mauritius", "Morocco", "Mozambique", "Namibia", "Nigeria", "Rwanda", "Senegal",
            "Seychelles", "Sierra_Leone", "Somalia", "South_Africa", "South_Sudan", "Sudan",
            "Tanzania", "Togo", "Tunisia", "Uganda", "Zambia", "Zimbabwe"
        };

        private List<Country> _worldCountries => _europeCountries.Concat(_americaCountries).Concat(_asiaCountries).Concat(_africaCountries).ToList();

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public void RaiseAllPropertyChanged() =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));

        public Task<bool> DisplayAlert(string title, string message, string accept, string cancel) =>
            Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
    }
}