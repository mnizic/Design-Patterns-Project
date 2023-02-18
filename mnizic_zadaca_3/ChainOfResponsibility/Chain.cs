using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.ChainOfResponsibility
{
    public interface Chain
    {
        public static int zahtjevPrihvacen = 1;
        public static int zahtjevOdbijen = 2;
        public static int zahtjevNeispravan = 3;

        public void ispisiDnevnikInfo(int razina, string zapis);
        public void setNextChain(Chain nextInChain);
        public void unesiZapisDnevnika(int razina, string zapis);
    }
}
