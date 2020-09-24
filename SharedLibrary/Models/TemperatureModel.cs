using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibrary.Models
{
    public class TemperatureModel
    {
        public TemperatureModel(double temp, double hum)
        {
            Temperature = temp;
            Humidity = hum;
        }

        public double Temperature { get; set; }
        public double Humidity { get; set; }
    }
}
