using Quiz_Vlajky.Extensions;
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
    public class PlayingPageViewModel : INotifyPropertyChanged
    {
        private Color _defaultColor = Color.FromHex("#101820");
        private Color _rightColor = Color.Green;
        private Color _wrongColor = Color.DarkRed;
        
        public int CurrentRound { get; private set; }
        public int TotalRounds { get; private set; }
        public Color BackgroundColor { get; private set; }

        public Question Question { get; private set; }
        public List<string> Answers { get; private set; }
        public static List<Question> Questions { get; set; }

        public ICommand SelectCommand { get; }

        private readonly Random _rng;
        private int _correctAnswers;

        public PlayingPageViewModel()
        {
            CurrentRound = 1;
            BackgroundColor = _defaultColor;

            _rng = new Random();
            _correctAnswers = 0;
            TotalRounds = Questions.Count;

            SelectRandomQuestion();
            SelectCommand = new Command<string>(HandleSelect);
        }

        private void HandleSelect(string rawIndex)
        {
            var flagIndex = int.Parse(rawIndex);
            var selectedAnswer = Answers[flagIndex];
            CheckAndHandleAnswer(selectedAnswer);
        }

        private void CheckAndHandleAnswer(string answer)
        {
            if (Question.CorrectAnswer == answer)
            {
                ++_correctAnswers;
                BackgroundColor = _rightColor;
            }
            else BackgroundColor = _wrongColor;
            RaisePropertyChanged(nameof(BackgroundColor));

            Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
            {
                BackgroundColor = _defaultColor;

                if (Questions.Count > 0)
                {
                    ++CurrentRound;
                    SelectRandomQuestion();
                }
                else
                    HandleGameEnd();

                RaiseAllPropertyChanged();
                return false;
            });
        }

        private async void HandleGameEnd()
        {
            await Shell.Current.DisplayAlert("Congratulations", $"You've chosen {_correctAnswers}/{TotalRounds} correctly!", "Back");
            await Shell.Current.Navigation.PopAsync();
        }

        private void SelectRandomQuestion()
        {
            Question = Questions[_rng.Next(Questions.Count)];
            Questions.Remove(Question);

            var answers = new List<string> { Question.Answer1, Question.Answer2, Question.Answer3, Question.CorrectAnswer };
            answers.Shuffle();
            Answers = answers;
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