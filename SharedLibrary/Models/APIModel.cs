using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibrary.Models
{
    public class APIModel
    {
        // Struktur enligt OpenWeatherMaps JSON-respons
        public Main main { get; set; }       

        public class Main
        {
            public double temp { get; set; }
            public double humidity { get; set; }
        }

    }
}
