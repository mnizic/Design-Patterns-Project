using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.ChainOfResponsibility
{
    public class DnevnikZapisi
    {
        public static Chain doChaining()
        {
            Chain zahtjevPrihvacenLog = new ZahtjevPrihvacenDnevnik(Chain.zahtjevPrihvacen);

            Chain zahtjevOdbijenLog = new ZahtjevOdbijenDnevnik(Chain.zahtjevOdbijen);
            zahtjevPrihvacenLog.setNextChain(zahtjevOdbijenLog);

            Chain zahtjevNeispravanLog = new ZahtjevNeispravanDnevnik(Chain.zahtjevNeispravan);
            zahtjevOdbijenLog.setNextChain(zahtjevNeispravanLog);

            return zahtjevPrihvacenLog;
        }
    }
}
