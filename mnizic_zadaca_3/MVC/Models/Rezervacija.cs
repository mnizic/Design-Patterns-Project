using mnizic_zadaca_3.Singleton;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.MVC.Models
{
    public class Rezervacija
    {
        public int IDBroda { get; set; }
        public DateTime datumVrijemeOd { get; set; }
        public int trajanjePrivezaUH { get; set; }
    }
}
