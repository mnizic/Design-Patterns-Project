using mnizic_zadaca_3.Singleton;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.MVC.Models
{
    public class Raspored
    {
        public int IDVez { get; set; }
        public int IDBrod { get; set; }
        public List<int> daniUTjednu { get; set; } = new List<int>();
        public DateTime vrijemeOd { get; set; } = new DateTime();
        public DateTime vrijemeDo { get; set; } = new DateTime();
    }
}
