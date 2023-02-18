using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.MVC.Models
{
    public class Brod
    {
        public int ID { get; set; }
        public string oznakaBroda { get; set; }
        public string naziv { get; set; }
        public string vrsta { get; set; }
        public double duljina { get; set; }
        public double sirina { get; set; }
        public double gaz { get; set; }
        public double maksimalnaBrzina { get; set; }
        public int kapacitetPutnika { get; set; }
        public int kapacitetOsobnihVozila { get; set; }
        public int kapacitetTereta { get; set; }
    }
}
