using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.Singleton
{
    public class PotrebniArgumentiSingleton
    {
        private static PotrebniArgumentiSingleton _instancaPotrebniArgumentiSingleton;
        private PotrebniArgumentiSingleton() { }

        public static PotrebniArgumentiSingleton InstancaPotrebniArgumenti
        {
            get
            {
                if (_instancaPotrebniArgumentiSingleton == null)
                    _instancaPotrebniArgumentiSingleton = new PotrebniArgumentiSingleton();

                return _instancaPotrebniArgumentiSingleton;
            }
        }

        public bool L { get; set; } = false;
        public bool V { get; set; } = false;
        public bool B { get; set; } = false;
        public bool Br { get; set; } = false;
        public bool Vt { get; set; } = false;
        public bool Pd { get; set; } = false; 

    }
}
