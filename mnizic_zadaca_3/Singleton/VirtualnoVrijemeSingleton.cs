using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.Singleton
{
    public class VirtualnoVrijemeSingleton
    {
        private static VirtualnoVrijemeSingleton _instancaVirtualnoVrijeme;

        private VirtualnoVrijemeSingleton() { }

        public static VirtualnoVrijemeSingleton InstancaVirtualnoVrijeme
        {
            get
            {
                if (_instancaVirtualnoVrijeme == null)
                    _instancaVirtualnoVrijeme = new VirtualnoVrijemeSingleton();

                return _instancaVirtualnoVrijeme;
            }
        }

        public DateTime virtualnoVrijeme { get; set; }
    }
}
