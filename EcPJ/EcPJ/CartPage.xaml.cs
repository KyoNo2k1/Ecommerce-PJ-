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
    public partial class CartPage : ContentPage
    {
        public CartPage()
        {
            InitializeComponent();
            ListViewInit();
        }
/*        int price = 0;
        int quantity = 0;*/
        async void ListViewInit()
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                var userId = Preferences.Get("userId", null);
                var token = Preferences.Get("accessToken", null);
                var tokenStr = "Bearer " + token;
                httpClient.DefaultRequestHeaders.Add("token", tokenStr);

                var cartList = await httpClient.GetStringAsync(AppSetting.ApiUrl + "api/carts/find/"+userId);
                CartItem cartListConverted = JsonConvert.DeserializeObject<CartItem>(cartList);
                ProductCart[] pdc = cartListConverted.products;
                int index = 0;
                List<Product> list = new List<Product>();
                foreach (ProductCart product in pdc)
                {
                    var productGet = await httpClient.GetStringAsync(AppSetting.ApiUrl + "api/products/find/" + pdc[index].productId.ToString());
                    var productListConverted = JsonConvert.DeserializeObject<Product>(productGet);
                    list.Add(productListConverted);
                    index++;
                }

                ShoppingCart.ItemsSource = list;
            }
            catch
            {
                _ = DisplayAlert("thong bao", "Chua co san pham duoc them vao gio hang", "OK");
            }

        }

        private void TapBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        async private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var userId = Preferences.Get("userId", null);
            var token = Preferences.Get("accessToken", null);
            var tokenStr = "Bearer " + token;
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("token", tokenStr);

            var cartList = await httpClient.GetStringAsync(AppSetting.ApiUrl + "api/carts/find/" + userId);
            CartItem cartListConverted = JsonConvert.DeserializeObject<CartItem>(cartList);
            var cartId = cartListConverted._id.ToString();
            var label = sender as Label;
            string removeId = label.ClassId;
            var response = await httpClient.PostAsync(AppSetting.ApiUrl + "api/carts/" + cartId + "/product/" + removeId, null);
            string jsonResult = await response.Content.ReadAsStringAsync();
            if (jsonResult == "true")
            {
                _ = DisplayAlert("Thong bao", "Xóa sản phẩm khỏi cart thành công", "OK");
                ListViewInit();
            }
            else
            {
                _ = DisplayAlert("Thong bao", "Xóa thất bại", "OK");
            }

        }

        private void Stepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
/*            int value = (int)e.NewValue;
            amount.Text = string.Format("{0}", value);
            var totalP = value * price;
            totalPriceText.Text = totalP.ToString() + "$";
            quantity = value;*/
        }

        async private void Button_Clicked(object sender, EventArgs e)
        {
            var userId = Preferences.Get("userId", null);
            var token = Preferences.Get("accessToken", null);
            var tokenStr = "Bearer " + token;
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("token", tokenStr);
            var cartList = await httpClient.GetStringAsync(AppSetting.ApiUrl + "api/carts/find/" + userId);
            CartItem cartListConverted = JsonConvert.DeserializeObject<CartItem>(cartList);
            Address address = new Address { city = "HCM", numAdr = "Default" };
            var order = new Order()
            {
                userId = userId,
                products = cartListConverted.products,
                amount = 1,
                address = address,
                status = "pending",
            };

            var json = JsonConvert.SerializeObject(order);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(AppSetting.ApiUrl + "api/orders", content);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Order>(jsonResult);
            if (result != null)
            {
                _ = DisplayAlert("Thong bao", "Tao don hang thanh cong", "OK");
            }
            else
            {
                _ = DisplayAlert("Thong bao", "Tao don hang that bai", "OK");
            }
        }
    }
}