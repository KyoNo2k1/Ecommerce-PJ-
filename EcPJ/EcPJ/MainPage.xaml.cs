using EcPJ.ClassItem;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EcPJ
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void TapBackArrow_Tapped(object sender, EventArgs e)
        {
            Application.Current.MainPage = new SignUpPage();
        }

        async private void loginBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                var login = new Login()
                {
                    user_name = email.Text,
                    password = password.Text
                };
                var httpClient = new HttpClient();
                var json = JsonConvert.SerializeObject(login);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(AppSetting.ApiUrl + "api/auth/login", content);
                var jsonResult = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Token>(jsonResult);
                if (result != null)
                {
                    Preferences.Set("accessToken", result.accessToken);
                    Preferences.Set("userId", result._id);
                    Preferences.Set("userName", result.username);
                    Application.Current.MainPage = new HomePage();
                }
                else
                {
                    _ = DisplayAlert("Thong bao", "Sai tk hoac mk", "OK");
                }
            }
            catch
            {
                _ = DisplayAlert("Thong bao", "Sai tk hoac mk", "OK");
            }
        }

/*        private void signupBtn_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new SignUpPage();
        }*/
    }
}
