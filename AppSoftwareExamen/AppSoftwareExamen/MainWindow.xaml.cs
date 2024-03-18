using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Windows;
using AppSoftwareExamen.Classes;
using static AppSoftwareExamen.Classes.db;

namespace AppSoftwareExamen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        db database;
        public MainWindow()
        {
            InitializeComponent();
            database = new db();

            lstPingResults.Visibility = Visibility.Collapsed;
            lstPreviousPings.Visibility = Visibility.Collapsed;
        }

        private async void btnPingIP_Click(object sender, RoutedEventArgs e)
        {
            lstPreviousPings.Visibility = Visibility.Collapsed;
            lstPingResults.Visibility = Visibility.Visible;

            string ipAddress = txtIPAddress.Text;

            if (txtIPAddress.Text != null && txtIPAddress.Text != "" && txtIPAddress.Text.Contains("."))
            {
                try
                {
                    string pingResults = await APIPingIP.PingIPAsync(ipAddress);

                    APIPingIP.PingResult pingResult = JsonSerializer.Deserialize<APIPingIP.PingResult>(pingResults);

                    List<APIPingIP.PingResult> pingResultsList = new List<APIPingIP.PingResult> { pingResult };

                    lstPingResults.ItemsSource = pingResultsList;

                    string senderr = await APIGetIP.GetIPAddressAsync();
                    await database.InsertPingResultAsync(senderr, ipAddress, pingResult.TimeMs.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Vul een geldig IP-Adres in!");
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

        private async void btnViewPastPings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                db database = new db();
                List<PreviousPingResult> pingResults = await database.GetPreviousPingsAsync();
                lstPreviousPings.ItemsSource = pingResults;

                lstPingResults.Visibility = Visibility.Collapsed;
                lstPreviousPings.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
