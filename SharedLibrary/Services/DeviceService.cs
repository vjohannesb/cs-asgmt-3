using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Azure.Devices.Client;
using MAD = Microsoft.Azure.Devices;
using SharedLibrary.Models;

namespace SharedLibrary.Services
{
    public static class DeviceService
    {
        // Console App (Device Client) -> IoT Device
        public static async Task SendMessageAsync(DeviceClient deviceClient)
        {
            while (true)
            {
                // Anropa och vänta in resultat från OpenWeatherMap via WeatherService
                var weatherData = await WeatherService.FetchWeatherData();

                // "Omformatera" till TemperatureModel för en egen struktur
                // Om resultat inte gick att hämta, skicka null (varningar skickas direkt från WeatherService)
                var data = weatherData != null
                    ? new TemperatureModel(temp: weatherData.main.temp, hum: weatherData.main.humidity)
                    : null;

                // Konvertera till JSON & vidare till bytes (spara json för konsolutskrift) 
                string json = JsonConvert.SerializeObject(data);
                var payload = new Message(Encoding.UTF8.GetBytes(json));

                await deviceClient.SendEventAsync(payload);

                Console.WriteLine($"Message sent: {json}");
                await Task.Delay(5 * 1000);
            }
        }

        // Console App <- Azure Function    [Receiver]
        public static async Task ReceiveMessageAsync(DeviceClient deviceClient)
        {
            while (true)
            {
                // Försök hämta meddelande/"payload" från Azure Function
                var payload = await deviceClient.ReceiveAsync();

                // Printa payload om det finns innehåll, annars gå vidare till nästa iteration
                if (payload != null)
                {
                    Console.WriteLine($"Message received: {Encoding.UTF8.GetString(payload.GetBytes())}");
                    await deviceClient.CompleteAsync(payload);
                }
            }
        }

        // Azure Function -> Console App    [Sender]
        public static async Task SendMessageToDeviceAsync(MAD.ServiceClient serviceClient, string targetDeviceId, string message)
        {
            // Skicka meddelande "message" till Device "targetDeviceId" mha SendAsync
            var payload = new MAD.Message(Encoding.UTF8.GetBytes(message));
            await serviceClient.SendAsync(targetDeviceId, payload);
        }
    }
}
