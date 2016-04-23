using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
namespace ica.smartphone
{
    public class LoadingWebsite : ContentPage
    {
        //these need to be defined at class level for use in methods.
        WebView webView;
        ActivityIndicator indicator;
        Button button;
        public LoadingWebsite()
        {
            this.Title = "ICA";

            var layout = new StackLayout();
            button = new Button() {Text="Connect internet",IsVisible=false };
            button.Clicked += Reconnect;
            //Loading label should not render by default.
            indicator = new ActivityIndicator() { IsRunning=true, IsVisible = true,HorizontalOptions=LayoutOptions.CenterAndExpand,VerticalOptions=LayoutOptions.CenterAndExpand };

            webView = new WebView() { IsVisible = false, HeightRequest = 1000, WidthRequest = 1000, Source = "http://ica.arthurcv.com/" };

            webView.Navigated += webviewNavigated;
            webView.Navigating += webviewNavigating;

            layout.Children.Add(button);
            layout.Children.Add(indicator);
            layout.Children.Add(webView);
            Content = layout;
        }
        private void Reconnect(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.Title = "ICA";

                var layout = new StackLayout();
                button = new Button() { Text = "Connect internet", IsVisible = false };
                button.Clicked += Reconnect;
                //Loading label should not render by default.
                indicator = new ActivityIndicator() { IsRunning = true, IsVisible = true, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };

                webView = new WebView() { IsVisible = false, HeightRequest = 1000, WidthRequest = 1000, Source = "http://ica.arthurcv.com/" };

                webView.Navigated += webviewNavigated;
                webView.Navigating += webviewNavigating;

                layout.Children.Add(button);
                layout.Children.Add(indicator);
                layout.Children.Add(webView);
                Content = layout;
            });
        }
        void webviewNavigating(object sender, WebNavigatingEventArgs e)
        {
            this.indicator.IsVisible = true;
            this.webView.IsVisible = false;
            this.button.IsVisible = false;
        }
        void webviewNavigated(object sender, WebNavigatedEventArgs e)
        {
            if (e.Result.ToString() == "Success")
            {
                this.webView.IsVisible = true;
                this.indicator.IsVisible = false;
                this.button.IsVisible = false;
            }
            else
            {
                this.webView.IsVisible = false;
                this.indicator.IsVisible = false;
                this.button.IsVisible = true;
            }
        }
    }
}
