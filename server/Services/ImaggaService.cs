using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text;

namespace ServicesGatway.Services
{
    public class ImaggaService
    {
        private readonly HttpClient _httpClient;

        private readonly List<string> _airplaneTags = new List<string>
        {
            "airplane", "plane", "jet", "aircraft", "aviation", "airliner", "glider", "biplane"
        };

        public ImaggaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new System.Uri("https://api.imagga.com/v2/");
        }

        /*public async Task<bool> IsAirplane(string imageUrl)
        {
            // שימוש ב-API Key ו-API Secret
            var apiKey = "acc_d1ecfb0ba6f2b9b";
            var apiSecret = "27d8539ca835b00ce7586881f43a67c1";

            var authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{apiKey}:{apiSecret}"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authValue);

            // שליחת בקשה ל-Imagga
            var response = await _httpClient.GetAsync($"tags?image_url={System.Uri.EscapeDataString(imageUrl)}");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {response.StatusCode} - {errorContent}");
                return false;
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(jsonResponse);

            // בדיקה אם התמונה מכילה תגיות שקשורות לכלי טיס
            var tags = json["result"]["tags"];
            foreach (var tag in tags)
            {
                string tagName = tag["tag"]["en"].ToString().ToLower();

                // בדיקה אם התגית נמצאת ברשימת התגיות לכלי טיס
                if (_airplaneTags.Contains(tagName))
                {
                    return true; // זוהה כמטוס
                }
            }

            return false; // לא זוהה כמטוס
        }*/
        public async Task<bool> IsAirplane(string imageUrl)
        {
            // שימוש ב-API Key ו-API Secret
            var apiKey = "acc_d1ecfb0ba6f2b9b";
            var apiSecret = "27d8539ca835b00ce7586881f43a67c1";

            var authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{apiKey}:{apiSecret}"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authValue);

            try
            {
                // שליחת בקשה ל-Imagga
                var response = await _httpClient.GetAsync($"tags?image_url={System.Uri.EscapeDataString(imageUrl)}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {response.StatusCode} - {errorContent}");
                    return false;
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(jsonResponse);

                // בדיקה אם התמונה מכילה תגיות שקשורות לכלי טיס
                var tags = json["result"]?["tags"];
                if (tags == null)
                {
                    Console.WriteLine("No tags found in the response.");
                    return false;
                }

                foreach (var tag in tags)
                {
                    string tagName = tag["tag"]?["en"]?.ToString().ToLower();
                    if (tagName != null && _airplaneTags.Contains(tagName))
                    {
                        return true; // זוהה כמטוס
                    }
                }

                return false; // לא זוהה כמטוס
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return false;
            }
        }

    }
}
