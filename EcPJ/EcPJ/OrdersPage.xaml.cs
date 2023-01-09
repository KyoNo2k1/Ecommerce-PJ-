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
    public partial class OrdersPage : ContentPage
    {
        public OrdersPage()
        {
            InitializeComponent();
            ListViewInit();
        }
        async void ListViewInit()
        {
            HttpClient httpClient = new HttpClient();
            var userId = Preferences.Get("userId", null);
            var token = Preferences.Get("accessToken", null);
            var tokenStr = "Bearer " + token;
            httpClient.DefaultRequestHeaders.Add("token", tokenStr);
            var orderListA = await httpClient.GetStringAsync(AppSetting.ApiUrl + "api/orders/find/" + userId);
            var orderListConverted = JsonConvert.DeserializeObject<List<Order>>(orderListA);
            orderList.ItemsSource = orderListConverted;

        }
        private void orderList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedItem = e.SelectedItem as Order;
            var id = selectedItem._id;
            Navigation.PushModalAsync(new OrderDetailPage(id));
        }

        private void TapBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}