using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibrary.Models
{
    public class WeatherModel
    {
        public WeatherModel(double temp, double hum)
        {
            Temperature = temp;
            Humidity = hum;
        }

        public double Temperature { get; set; }
        public double Humidity { get; set; }

        // Implicit omvandling från APIModel -> Weathermodel
        public static implicit operator WeatherModel(APIModel am)
            => new WeatherModel(am.main.temp, am.main.humidity);
    }
}
