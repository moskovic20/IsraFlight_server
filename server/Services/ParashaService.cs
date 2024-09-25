using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ServicesGatway.Services;
public class ParashaService
{
    private readonly HttpClient _httpClient;

    public ParashaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // פונקציה לקבלת פרשת השבוע
    public async Task<string?> GetParashaAsync(DateTime date)
    {

        try
        {
            string dateString = date.ToString("yyyy-MM-dd");
            // בקשה ל-API לקבלת פרשת השבוע
            var response = await _httpClient.GetAsync($"https://www.hebcal.com/torah?cfg=json&gy={date.Year}&gm={date.Month}&gd={date.Day}");

            if (response.IsSuccessStatusCode)
            {

                var content = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(content);

                // החזרת פרשת השבוע
                return json["items"]?[0]?["title"]?.ToString() ?? throw new Exception("Parasha not found in response.");


            }

            throw new HttpRequestException($"Failed to fetch Parasha: {response.ReasonPhrase}");

        }
        catch (Exception ex)
        {
            // זריקת החריגה למעלה בשרשרת
            throw new Exception($"Error fetching Parasha: {ex.Message}", ex);
        }

    }
}



