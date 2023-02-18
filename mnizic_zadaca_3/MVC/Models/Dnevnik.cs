using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.MVC.Models
{
    public class Dnevnik
    {
        public int razina { get; set; }
        public string zapis { get; set; }
        public Dnevnik(int razina, string zapis)
        {
            this.razina = razina;
            this.zapis = zapis;
        }
    }
}
