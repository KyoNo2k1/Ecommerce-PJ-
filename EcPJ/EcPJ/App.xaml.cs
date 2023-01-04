using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EcPJ
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Preferences.Remove("accessToken");
            Preferences.Remove("userId");
            Preferences.Remove("userName");

            if (Preferences.ContainsKey("accessToken"))
            {
                MainPage = new NavigationPage(new HomePage());
            }
            else
            {
                MainPage = new MainPage();
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
