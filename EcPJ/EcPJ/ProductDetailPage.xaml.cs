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
    public partial class ProductDetailPage : ContentPage
    {
        public ProductDetailPage(string productId)
        {
            InitializeComponent();
            ListViewInit(productId);
        }
        int price = 0;
        int quantity = 0;
        string pdid = "";
        async void ListViewInit(string productId)
        {
            pdid = productId;
            HttpClient httpClient = new HttpClient();
            var userId = Preferences.Get("userId", null);
            var token = Preferences.Get("accessToken", null);
            var tokenStr = "Bearer " + token;
            httpClient.DefaultRequestHeaders.Add("token", tokenStr);
            var productDetail = await httpClient.GetStringAsync(AppSetting.ApiUrl + "api/products/find/" + productId);
            var productListConverted = JsonConvert.DeserializeObject<Product>(productDetail);
            imgPd.Source = productListConverted.img;
            titlePd.Text = productListConverted.title;
            price = productListConverted.price;
            pricePd.Text = productListConverted.price.ToString()+"$";
            
            desPd.Text = productListConverted.desc;

        }
        private void TapBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void Stepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            int value = (int)e.NewValue;
            _displayLabel.Text = string.Format("{0}", value);
            var totalP = value * price;
            totalPriceText.Text = totalP.ToString()+"$";
            quantity = value;
        }

        async private void BtnAddToCart_Clicked(object sender, EventArgs e)
        {
            {
                try
                {
                    string id = pdid;
                    var userId = Preferences.Get("userId", null);
                    var token = Preferences.Get("accessToken", null);
                    var tokenStr = "Bearer " + token;
                    ProductCart[] products = new ProductCart[] {
                    new ProductCart {productId = id, quantity= quantity},
                };
                    var cartItem = new CartItem()
                    {
                        userId = userId,
                        products = products,
                    };
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Add("token", tokenStr);
                    var json = JsonConvert.SerializeObject(cartItem);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(AppSetting.ApiUrl + "api/carts", content);
                    var jsonResult = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<CartItem>(jsonResult);
                    if (result != null)
                    {
                        _ = DisplayAlert("Thong bao", "add thanh cong", "OK");
                    }
                }
                catch (Exception ex)
                {
                    _ = DisplayAlert("Thong bao", ex.Message, "OK");

                }
            }
        }
    }
}