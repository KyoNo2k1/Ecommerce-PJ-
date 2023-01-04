using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using EcPJ.ClassItem;
using Xamarin.Essentials;

namespace EcPJ
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            ListViewInit();
        }
        async void ListViewInit()
        {
            HttpClient httpClient = new HttpClient();
            var productList = await httpClient.GetStringAsync(AppSetting.ApiUrl + "api/products");
            var productListConverted = JsonConvert.DeserializeObject<List<Product>>(productList);
            LstProduct.ItemsSource = productListConverted;
            
        }

        private void LstProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }

        private void ImgMenu_Tapped(object sender, EventArgs e)
        {

        }

        private void TapCartIcon_Tapped(object sender, EventArgs e)
        {

        }

        private void TapOrders_Tapped(object sender, EventArgs e)
        {

        }

        private void TapCart_Tapped(object sender, EventArgs e)
        {

        }

        private void TapContact_Tapped(object sender, EventArgs e)
        {

        }

        private void TapLogout_Tapped(object sender, EventArgs e)
        {

        }

        private void TapCloseMenu_Tapped(object sender, EventArgs e)
        {

        }

        //private void logout_Clicked(object sender, EventArgs e)
        //{
          //  Preferences.Remove("accessToken");
            ///Preferences.Remove("userId");
            //Preferences.Remove("userName");
        //}
    }
}