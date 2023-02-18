using mnizic_zadaca_3.Composite;
using mnizic_zadaca_3.MVC.Models;
using mnizic_zadaca_3.MVC.Views;
using mnizic_zadaca_3.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.ChainOfResponsibility
{
    public class ZahtjevPrihvacenDnevnik : Chain
    {
        private readonly int razina;
        private Chain? nextInChain;

        public ZahtjevPrihvacenDnevnik(int razina) => this.razina = razina;

        public void setNextChain(Chain nextInChain)
        {
            this.nextInChain = nextInChain;
        }

        public void unesiZapisDnevnika(int razina, string zapis)
        {
            if (razina == this.razina)
            {
                ispisiDnevnikInfo(razina, zapis);
            } 
            else
            {
                nextInChain?.unesiZapisDnevnika(razina, zapis);
            }
        }

        public void ispisiDnevnikInfo(int razina, string zapis)
        {
            string konacniIspis = VirtualnoVrijemeSingleton.InstancaVirtualnoVrijeme.virtualnoVrijeme.ToString() + 
                " - ZAHTJEV PRIHVACEN: " + zapis;

            KomandeView.ispisiOdgovor(konacniIspis);
            SviZapisiDnevnikaSingleton.InstancaSviZapisiDnevnika.sviZapisiDnevnik.Add(new Dnevnik(razina, konacniIspis));
        }
    }
}
