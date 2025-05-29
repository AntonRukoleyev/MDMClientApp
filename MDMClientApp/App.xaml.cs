using MDMClientApp.Services;
using MDMClientApp.Views;

namespace MDMClientApp
{
    public partial class App : Application
    {
        [Obsolete]
        public App()
        {
            InitializeComponent();
            DatabaseService.InitializeDatabase();
            MainPage = new NavigationPage(new LoginPage());
        }
    }
}