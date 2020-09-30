using Newtonsoft.Json;
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Services
{
    public static class WeatherService
    {
        private static HttpResponseMessage _response;
        private static readonly HttpClient _client = new HttpClient();

        private static readonly string _baseUrl = "https://api.openweathermap.org/data/2.5/";
        private static readonly string _apiUrl = $"{_baseUrl}weather?q=Örebro,se&units=metric&appid=" + Config.ApiKey;

        public static async Task<WeatherModel> FetchWeatherData()
        {
            try
            {
                _response = await _client.GetAsync(_apiUrl);

                // Status = 2XX
                if (_response.IsSuccessStatusCode)
                {
                    try
                    {
                        // Resultat i string/JSON -> APIModel -> WeatherModel
                        return JsonConvert.DeserializeObject<APIModel>(await _response.Content.ReadAsStringAsync());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error parsing data from API - {ex.Message}");
                    }
                }
                else 
                    Console.WriteLine($"Error - Status code: {_response.StatusCode}");
            }
            // _response fail, url svarar inte
            catch (Exception ex)
            {
                Console.WriteLine($"{_baseUrl} is not responding - {ex.Message}");
                Console.WriteLine($"Full query: {_apiUrl.Substring(_baseUrl.Length)}");
            }

            return null;
        }
    }
}

