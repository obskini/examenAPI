using AppSoftwareExamen.Classes;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Windows;

namespace AppSoftwareExamen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            lstPingResults.Visibility = Visibility.Collapsed;
        }

        private async void btnPingIP_Click(object sender, RoutedEventArgs e)
        {
            lstPingResults.Visibility = Visibility.Visible;
            string ipAddress = txtIPAddress.Text;

            try
            {
                string pingResults = await APIPingIP.PingIPAsync(ipAddress);

                /*Console.WriteLine("JSON Response:");
                Console.WriteLine(pingResults);*/

                APIPingIP.PingResult pingResult = JsonSerializer.Deserialize<APIPingIP.PingResult>(pingResults);

                List<APIPingIP.PingResult> pingResultsList = new List<APIPingIP.PingResult> { pingResult };

                lstPingResults.ItemsSource = pingResultsList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnGetIP_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string ipAddress = await APIGetIP.GetIPAddressAsync();
                txtIPAddress.Text = ipAddress;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
