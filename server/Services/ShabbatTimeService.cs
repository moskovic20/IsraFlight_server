using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ServicesGatway.Services
{
    public class ShabbatTimeService
    {
        private readonly HttpClient _httpClient;

        public ShabbatTimeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // פונקציה לקבלת זמני כניסת ויציאת שבת
        public async Task<(DateTime Entry, DateTime Exit)> GetShabbatTimesAsync(DateTime date, string arrivalCity)
        {
            try
            {
                // בדיקה האם arrivalCity הוא ערך תקין
                if (string.IsNullOrWhiteSpace(arrivalCity))
                {
                    throw new ArgumentException("Invalid location");
                }

                // בקשה ל-API לקבלת זמני כניסת ויציאת שבת
                var response = await _httpClient.GetAsync($"https://www.hebcal.com/shabbat?cfg=json&city={arrivalCity}&gy={date.Year}&gm={date.Month}&gd={date.Day}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Failed to fetch Shabbat times: {response.ReasonPhrase}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(content);

                // בדיקה אם ה-API מחזיר נתונים
                if (json["items"] == null || !json["items"].Any())
                {
                    throw new Exception("No Shabbat times data found.");
                }

                // קבלת זמני כניסת ויציאת השבת
                var shabbatEntry = json["items"]
                    ?.FirstOrDefault(x => x["category"]?.ToString() == "candles")?["date"]?.ToObject<DateTime>();
                var shabbatExit = json["items"]
                    ?.FirstOrDefault(x => x["category"]?.ToString() == "havdalah")?["date"]?.ToObject<DateTime>();

                if (shabbatEntry == null || shabbatExit == null)
                {
                    throw new Exception("Shabbat times not found in response.");
                }

                return (shabbatEntry.Value, shabbatExit.Value); // החזרת הערכים בצורה בטוחה ללא null
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException($"Error fetching Shabbat times: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error: {ex.Message}", ex);
            }
        }
    }
}

