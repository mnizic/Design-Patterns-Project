using mnizic_zadaca_3.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.MVC.Views
{
    public class PodaciView
    {
        public static List<string> listaGresaka = new();
        public static void ispisGreske(int brojGreske, string opisGreske)
        {
            listaGresaka.Add($"Greska pod rednim brojem {brojGreske} - Opis: {opisGreske}");
        }
    }
}
