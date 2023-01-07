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
            var currectSelection = e.CurrentSelection.FirstOrDefault() as Product;
            if (currectSelection == null) return;
            Navigation.PushModalAsync(new ProductDetailPage(currectSelection._id));
            ((CollectionView)sender).SelectedItem = null;
        }

        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        async private void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                string id = ((Button)sender).ClassId;
                var userId = Preferences.Get("userId", null);
                var token = Preferences.Get("accessToken", null);
                var tokenStr = "Bearer " + token;
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("token", tokenStr);
                var cartList = await httpClient.GetStringAsync(AppSetting.ApiUrl + "api/carts/find/" + userId);
                CartItem cartListConverted = JsonConvert.DeserializeObject<CartItem>(cartList);
                ProductCart[] products = new ProductCart[] {
                    new ProductCart {productId = id, _id = cartListConverted._id },
                };


                if (cartListConverted._id != null)
                {
                    var productClone = cartListConverted.products;
                    var newPd = products.Concat(productClone).ToArray();

                    var cartItemConcat = new CartItem()
                    {
                        _id = cartListConverted._id,
                        userId = userId,
                        products = newPd,
                    };
                    var jsonConcat = JsonConvert.SerializeObject(cartItemConcat);
                    var contentConcat = new StringContent(jsonConcat, Encoding.UTF8, "application/json");
                    var responseConcat = await httpClient.PutAsync(AppSetting.ApiUrl + "api/carts/" + cartListConverted._id, contentConcat);
                    var jsonResultConcat = await responseConcat.Content.ReadAsStringAsync();
                    var resultConcat = JsonConvert.DeserializeObject<CartItem>(jsonResultConcat);
                    if (resultConcat != null)
                    {
                        _ = DisplayAlert("Thong bao", "add thanh cong vao gio hang", "OK");
                    }
                }
                else
                {
                    var cartItem = new CartItem()
                    {
                        _id = cartListConverted._id,
                        userId = userId,
                        products = products,
                    };
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


            }
            catch (Exception ex)
            {
                _ = DisplayAlert("Thong bao", ex.Message, "OK");

            }
        }

        private async void ImgMenu_Tapped(object sender, EventArgs e)
        {
            GridOverlay.IsVisible = true;
            await SlMenu.TranslateTo(0, 0, 400, Easing.Linear);
        }
        private async void CloseHamBurgerMenu()
        {
            await SlMenu.TranslateTo(-250, 0, 400, Easing.Linear);
            GridOverlay.IsVisible = false;
        }
        private void TapCartIcon_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new CartPage());
        }

        private void TapOrders_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new OrdersPage());
        }

        private void TapCart_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new CartPage());
        }

        private void TapContact_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ContactPage());
        }

        private void TapLogout_Tapped(object sender, EventArgs e)
        {
            Preferences.Remove("accessToken");
            Preferences.Remove("userId");
            Preferences.Remove("userName");

            Application.Current.MainPage = new SignUpPage();
        }

        private void TapCloseMenu_Tapped(object sender, EventArgs e)
        {
            CloseHamBurgerMenu();
        }

    }
}