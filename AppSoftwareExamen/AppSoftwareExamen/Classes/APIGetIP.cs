using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppSoftwareExamen.Classes
{
    public class APIGetIP
    {
        private const string ApiUrl = "https://api.ipify.org/?format=json";

        public static async Task<string> GetIPAddressAsync()
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(ApiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var jsonResponse = JsonSerializer.Deserialize<IPResponse>(responseBody);
                        string ipAddress = jsonResponse.ip;
                        return ipAddress;
                    }
                    else
                    {
                        throw new Exception("Failed to get IP address.");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while getting IP address. ", ex);
                }
            }
        }

        private class IPResponse
        {
            public string ip { get; set; }
        }
    }
}
