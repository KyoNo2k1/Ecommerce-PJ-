using EcPJ.ClassItem;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EcPJ
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
        }

        async private void Button_Clicked(object sender, EventArgs e)
        {
            if(password.Text != confirmPW.Text)
            {
                _ = DisplayAlert("Thong bao", "Xac nhan sai mat khau vui long nhap lai", "OK");
            }
            else
            {
                var signup = new SignUp()
                {
                    username = username.Text,
                    email = email.Text,
                    password = password.Text
                };
                var httpClient = new HttpClient();
                var json = JsonConvert.SerializeObject(signup);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(AppSetting.ApiUrl + "api/auth/registers", content);
                var jsonResult = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<SignUp>(jsonResult);
                if (result != null)
                {
                    _ = DisplayAlert("Thong bao", "Tao tai khoan thanh cong", "OK");
                    Application.Current.MainPage = new NavigationPage(new MainPage());
                }
                else
                {
                    _ = DisplayAlert("Thong bao", "Tao taia khoan khong thanh cong", "OK");
                }
            }
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            Application.Current.MainPage = new MainPage();
        }
    }
}