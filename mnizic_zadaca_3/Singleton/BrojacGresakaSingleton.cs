using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.Singleton
{
    public class BrojacGresakaSingleton
    {
        private static readonly Lazy<BrojacGresakaSingleton> _instancaBrojacGresaka = 
            new Lazy<BrojacGresakaSingleton>(() => new BrojacGresakaSingleton());

        private BrojacGresakaSingleton() { }

        public static BrojacGresakaSingleton InstancaBrojacGresaka
        {
            get { return _instancaBrojacGresaka.Value; }
        }

        public int brojGreske { get; set; }
    }
}
