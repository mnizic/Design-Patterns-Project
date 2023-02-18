using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.Singleton
{
    public class VT100Singleton
    {
        private static VT100Singleton _instancaVT100Singleton;
        private VT100Singleton() { }

        public static VT100Singleton InstancaVT100
        {
            get
            {
                if (_instancaVT100Singleton == null)
                    _instancaVT100Singleton = new VT100Singleton();

                return _instancaVT100Singleton;
            }
        }

        public readonly string ANSI_ESC = "\x1b[";
        public int Br { get; set; }
        public string Vt { get; set; }
        public string Pd { get; set; }
        public int GornjiDio { get; set; }
        public int DonjiDio { get; set; }
        public bool On { get; set; } = true;
    }
}
