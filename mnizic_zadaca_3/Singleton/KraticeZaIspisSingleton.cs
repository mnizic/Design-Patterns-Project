using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.Singleton
{
    public class KraticeZaIspisSingleton
    {
        private static KraticeZaIspisSingleton _instancaKraticeZaIspis;
        private KraticeZaIspisSingleton() { }

        public static KraticeZaIspisSingleton InstancaKraticeZaIspis
        {
            get
            {
                if (_instancaKraticeZaIspis == null)
                    _instancaKraticeZaIspis = new KraticeZaIspisSingleton();

                return _instancaKraticeZaIspis;
            }
        }

        public bool Zaglavlje { get; set; } = false;
        public bool RedniBrojevi { get; set; } = false;
        public bool Podnozje { get; set; } = false;
    }
}
