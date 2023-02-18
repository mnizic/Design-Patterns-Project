using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.Singleton
{
    public class VrijemeUcitavanjaSingleton
    {
        private static VrijemeUcitavanjaSingleton _instancaVrijemeUcitavanja;

        private VrijemeUcitavanjaSingleton() { }

        public static VrijemeUcitavanjaSingleton InstancaVrijemeUcitavanja
        {
            get
            {
                if (_instancaVrijemeUcitavanja == null)
                    _instancaVrijemeUcitavanja = new VrijemeUcitavanjaSingleton();

                return _instancaVrijemeUcitavanja;
            }
        }

        public DateTime Vrijeme { get; set; }
    }
}
