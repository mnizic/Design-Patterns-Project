using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.MVC.Models
{
    public class StanjeVezova
    {
        public int ID { get; set; }
        public string Vrsta { get; set; }
        public string Status { get; set; }
        public DateTime VirtualnoVrijeme { get; set;}
    }
}
