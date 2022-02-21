using Quiz_Vlajky.Services;
using Xamarin.Forms;

namespace Quiz_Vlajky
{
    public partial class App : Application
    {
        public DatabaseService DatabaseService { get; }

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
            DatabaseService = new DatabaseService();
        }

        protected async override void OnStart()
        {
            await DatabaseService.Initialize();
        }
    }
}
