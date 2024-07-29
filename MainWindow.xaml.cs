using Microsoft.Web.WebView2.Core;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace WpfAppWebBrowser
{
    public partial class MainWindow : Window
    {
        private const string FavoritesFile = "favorites.fvr";

        public MainWindow()
        {
            InitializeComponent();
            InitializeWebView2();
            LoadFavorites();
        }

        // 블로그 참조
        private async void InitializeWebView2()
        {
            await webView.EnsureCoreWebView2Async(null);
        }

        private void BtnGo_Click(object sender, RoutedEventArgs e)
        {
            NavigateToUrl(txtUrl.Text);
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (webView.CoreWebView2.CanGoBack)
            {
                webView.CoreWebView2.GoBack();
            }
        }

        private void BtnForward_Click(object sender, RoutedEventArgs e)
        {
            if (webView.CoreWebView2.CanGoForward)
            {
                webView.CoreWebView2.GoForward();
            }
        }

        private void BtnFavorite_Click(object sender, RoutedEventArgs e)
        {
            if (rbAdd.IsChecked == true)
            {
                AddFavorite(txtUrl.Text);
            }
            else if (rbLoad.IsChecked == true && lstFavorites.SelectedItem != null)
            {
                NavigateToUrl(lstFavorites.SelectedItem.ToString());
            }
        }

        // 블로그 참조
        private void NavigateToUrl(string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                {
                    url = "http://" + url;
                }
                webView.CoreWebView2.Navigate(url);
                txtUrl.Text = url;
            }
        }

        private void AddFavorite(string url)
        {
            if (!string.IsNullOrWhiteSpace(url) && !lstFavorites.Items.Contains(url))
            {
                lstFavorites.Items.Add(url);
                SaveFavorites();
            }
        }

        // 블로그 참조
        private void SaveFavorites()
        {
            using (StreamWriter writer = new StreamWriter(FavoritesFile))
            {
                foreach (var item in lstFavorites.Items)
                {
                    writer.WriteLine(item.ToString());
                }
            }
        }

        private void LoadFavorites()
        {
            if (File.Exists(FavoritesFile))
            {
                using (StreamReader reader = new StreamReader(FavoritesFile))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        lstFavorites.Items.Add(line);
                    }
                }
            }
        }
    }
}
