using mnizic_zadaca_3.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.Singleton
{
    public class SviZapisiDnevnikaSingleton
    {
        private static SviZapisiDnevnikaSingleton _instancaSviZapisiDnevnika;
        private SviZapisiDnevnikaSingleton() { }

        public static SviZapisiDnevnikaSingleton InstancaSviZapisiDnevnika
        {
            get
            {
                if (_instancaSviZapisiDnevnika == null)
                {
                    _instancaSviZapisiDnevnika = new SviZapisiDnevnikaSingleton();
                }
                    
                    

                return _instancaSviZapisiDnevnika;
            }
        }

        public List<Dnevnik> sviZapisiDnevnik = new();
    }
}
