using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.MVC.Models
{
    public class Luka
    {
        public string naziv { get; set; }
        public double gpsSirina { get; set; }
        public double gpsVisina { get; set; }
        public double dubinaLuke { get; set; }
        public double ukupniBrPutnickihVezova { get; set; }
        public double ukupniBrPoslovnihVezova { get; set; }
        public double ukupniBrOstalihVezova { get; set; }
        public DateTime virtualnoVrijeme { get; set; }
    }
}
