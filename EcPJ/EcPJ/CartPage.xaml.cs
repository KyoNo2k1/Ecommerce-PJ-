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
        private string _idCart;

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
                _idCart = cartListConverted._id.ToString();
                ProductCart[] pdc = cartListConverted.products;
                int index = 0;
                int total = 0;
                List<Product> list = new List<Product>();
                foreach (ProductCart product in pdc)
                {
                    var productGet = await httpClient.GetStringAsync(AppSetting.ApiUrl + "api/products/find/" + pdc[index].productId.ToString());
                    var productListConverted = JsonConvert.DeserializeObject<Product>(productGet);
                    list.Add(productListConverted);
                    index++;
                    total = total + productListConverted.price;
                }
                if (list != null)
                {
                    LblTotalPrice.Text = total.ToString() + "$";
                    ShoppingCart.ItemsSource = list;
                }
                else
                {
                    _ = DisplayAlert("Thong bao", "Chua co san pham nao trong gio hang", "OK");
                    ShoppingCart.ItemsSource = list;

                }
            }
            catch
            {
                _ = DisplayAlert("thong bao", "Chua co san pham duoc them vao gio hang", "OK");
                List<Product> list1 = new List<Product>();

                ShoppingCart.ItemsSource = list1;

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
            }
            else
            {
                if (jsonResult == "false")
                {
                    await httpClient.DeleteAsync(AppSetting.ApiUrl + "api/carts/" + cartId);
                    _ = DisplayAlert("Thong bao", "Xóa sản phẩm khỏi cart thành công", "OK");

                }
                else
                {
                    _ = DisplayAlert("Thong bao", "Xóa thất bại", "OK");
                }
            }
            ListViewInit();

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

        async private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            bool result = await DisplayAlert("Thong bao", "Bạn muốn xóa toàn bộ giỏ hàng??", "Có", "Không");
            if (result)
            {
                var userId = Preferences.Get("userId", null);
                var token = Preferences.Get("accessToken", null);
                var tokenStr = "Bearer " + token;
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("token", tokenStr);
                try
                {
                    await httpClient.DeleteAsync(AppSetting.ApiUrl + "api/carts/"+ _idCart);
                    _ = DisplayAlert("Thong bao", "Xoa gio hang thanh cong", "OK");
                    Application.Current.MainPage = new HomePage();
                }
                catch
                {
                    _ = DisplayAlert("Thong bao", "Xoa gio hang that bai", "OK");
                }
            }
            else
            {
                // Add code here to execute if the user clicked "Không"
            }
        }
    }
}