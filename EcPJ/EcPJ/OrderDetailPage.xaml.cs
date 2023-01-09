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
    public partial class OrderDetailPage : ContentPage
    {
        private string _idOrder;
        public OrderDetailPage(string idOrder)
        {
            InitializeComponent();
            _idOrder = idOrder;
            ListViewInit(idOrder);
        }
        async void ListViewInit(string idOrder)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                var userId = Preferences.Get("userId", null);
                var token = Preferences.Get("accessToken", null);
                var tokenStr = "Bearer " + token;
                httpClient.DefaultRequestHeaders.Add("token", tokenStr);

                var orderGet = await httpClient.GetStringAsync(AppSetting.ApiUrl + "api/orders/find/" + userId + "/order/" + idOrder);
                var orderConverted = JsonConvert.DeserializeObject<OrderItem>(orderGet);

                ProductCart[] pdc = orderConverted.products;


                int index = 0;
                int total = 0;
                List<ProductQuan> list = new List<ProductQuan>();
                foreach (ProductCart product in pdc)
                {
                    var productGet = await httpClient.GetStringAsync(AppSetting.ApiUrl + "api/products/find/" + pdc[index].productId.ToString());
                    ProductQuan productListConverted = JsonConvert.DeserializeObject<ProductQuan>(productGet);
                    productListConverted.quantity = product.quantity;
                    productListConverted.total = total;
                    list.Add(productListConverted);
                    index++;
                    total = total + product.quantity * productListConverted.price;
                }
                totalPrice.Text = total.ToString() + "$";
                listOrderProduct.ItemsSource = list;

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

        async private void Button_Clicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert(
            "Thông báo",
            "Bạn muốn xóa order này?",
            "Có",
            "Không"
            );

            if (result)
            {
                HttpClient httpClient = new HttpClient();
                var userId = Preferences.Get("userId", null);
                var token = Preferences.Get("accessToken", null);
                var tokenStr = "Bearer " + token;
                httpClient.DefaultRequestHeaders.Add("token", tokenStr);

                var response = await httpClient.DeleteAsync(AppSetting.ApiUrl + "api/orders/" + _idOrder);
                string jsonResult = await response.Content.ReadAsStringAsync();
                if(jsonResult != null)
                {
                    _ = DisplayAlert("Thong bao", "Xoa order thanh cong", "OK");
                    Application.Current.MainPage = new HomePage();


                }
            }
            else
            {
                // The user pressed the "No" button
                // Do something here
            }
        }
    }
}