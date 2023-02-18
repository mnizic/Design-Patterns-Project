using mnizic_zadaca_3.IteratorPattern.Collections;
using mnizic_zadaca_3.MVC.Views;
using mnizic_zadaca_3.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.Composite.Vezovi
{
    public class OstaliVezovi : VezoviController
    {
        public static OstaliVezoviCollection ostaliVezoviLista = new();

        public static void DohvatiVez(string linijaUDatoteci)
        {
            string[] dohvaceneVrijednosti = linijaUDatoteci.Split(';');

            try
            {
                OstaliVezovi ostali = new();
                ostali.provjeriBrojDohvacenihVrijednosti(dohvaceneVrijednosti);
                ostali = ostali.provjeriIspravnostZapisaVeza(dohvaceneVrijednosti);
                ostali.provjeriDuplikat(ostali);
                ostaliVezoviLista.DodajOstaliVez(ostali);
            }
            catch (Exception ex)
            {
                PodaciView.ispisGreske(++BrojacGresakaSingleton.InstancaBrojacGresaka.brojGreske, ex.Message);
            }
        }

        public void provjeriDuplikat(OstaliVezovi vez)
        {
            if (ostaliVezoviLista.Any(vez))
            {
                throw new Exception($"Vez sa ID-om {vez.ID} vec postoji.");
            }
        }

        public override OstaliVezovi provjeriIspravnostZapisaVeza(string[] dohvaceneVrijednosti)
        {
            return new OstaliVezovi()
            {
                ID = postaviIDVeza(dohvaceneVrijednosti[0]),
                oznakaVeza = postaviOznakuVeza(dohvaceneVrijednosti[1]),
                vrsta = postaviVrstuVeza(dohvaceneVrijednosti[2]),
                cijenaVezaPoSatu = postaviCijenuVezaPoSatu(dohvaceneVrijednosti[3]),
                maksimalnaDuljina = postaviMaksimalnuDuljinu(dohvaceneVrijednosti[4]),
                maksimalnaSirina = postaviMaksimalnuSirinu(dohvaceneVrijednosti[5]),
                maksimalnaDubina = postaviMaksimalnuDubinu(dohvaceneVrijednosti[6])
            };
        }
    }
}
