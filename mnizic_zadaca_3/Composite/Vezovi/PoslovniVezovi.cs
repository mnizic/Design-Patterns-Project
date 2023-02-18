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
    public class PoslovniVezovi : VezoviController
    {
        public static PoslovniVezoviCollection poslovniVezoviLista = new();

        public static void DohvatiVez(string linijaUDatoteci)
        {
            string[] dohvaceneVrijednosti = linijaUDatoteci.Split(';');

            try
            {
                PoslovniVezovi poslovni = new();
                poslovni.provjeriBrojDohvacenihVrijednosti(dohvaceneVrijednosti);
                poslovni = poslovni.provjeriIspravnostZapisaVeza(dohvaceneVrijednosti);
                poslovni.provjeriDuplikat(poslovni);
                poslovniVezoviLista.DodajPoslovniVez(poslovni);

            }
            catch (Exception ex)
            {
                PodaciView.ispisGreske(++BrojacGresakaSingleton.InstancaBrojacGresaka.brojGreske, ex.Message);
            }
        }

        public void provjeriDuplikat(PoslovniVezovi po)
        {
            if (poslovniVezoviLista.Any(po))
            {
                throw new Exception($"Vez sa ID-om {po.ID} vec postoji.");
            }
        }

        public override PoslovniVezovi provjeriIspravnostZapisaVeza(string[] dohvaceneVrijednosti)
        {
            return new()
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
