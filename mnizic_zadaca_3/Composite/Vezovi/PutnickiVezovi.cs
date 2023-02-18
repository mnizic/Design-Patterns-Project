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
    public class PutnickiVezovi : VezoviController
    {
        public static PutnickiVezoviCollection putnickiVezoviLista = new();

        public static void DohvatiVez(string linijaUDatoteci)
        {
            string[] dohvaceneVrijednosti = linijaUDatoteci.Split(';');

            try
            {
                PutnickiVezovi putnicki = new();
                putnicki.provjeriBrojDohvacenihVrijednosti(dohvaceneVrijednosti);
                putnicki = putnicki.provjeriIspravnostZapisaVeza(dohvaceneVrijednosti);
                putnicki.provjeriDuplikat(putnicki);

                putnickiVezoviLista.DodajPutnickiVez(putnicki);
            }
            catch (Exception ex)
            {
                PodaciView.ispisGreske(++BrojacGresakaSingleton.InstancaBrojacGresaka.brojGreske, ex.Message);
            }
        }

        public void provjeriDuplikat(PutnickiVezovi pu)
        {
            if (putnickiVezoviLista.Any(pu)) throw new Exception($"Vez sa ID-om {pu.ID} vec postoji.");

        }

        public override PutnickiVezovi provjeriIspravnostZapisaVeza(string[] dohvaceneVrijednosti)
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
