using mnizic_zadaca_3.MVC.Models;
using mnizic_zadaca_3.MVC.Views;
using mnizic_zadaca_3.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.MVC.Controllers.PodaciController
{
    public class BrodoviController
    {
        public static List<Brod> brodoviLista { get; set; } = new();

        public static void DohvatiBrodove(string linijaUDatoteci)
        {
            string[] dohvaceneVrijednosti = linijaUDatoteci.Split(';');
            try
            {
                Brod brod = provjeriBrod(dohvaceneVrijednosti);
                provjeriBrojDohvacenihVrijednosti(dohvaceneVrijednosti);
                provjeriDuplikat(brod);
                brodoviLista.Add(brod);
            }
            catch (Exception ex)
            {
                PodaciView.ispisGreske(++BrojacGresakaSingleton.InstancaBrojacGresaka.brojGreske, ex.Message);
            }
        }

        private static void provjeriBrojDohvacenihVrijednosti(string[] dohvaceneVrijednosti)
        {
            if (dohvaceneVrijednosti.Length != 11) throw new Exception("Netocan broj atributa.");
        }

        private static void provjeriDuplikat(Brod brod)
        {
            if (brodoviLista.Any(x => x.ID == brod.ID)) throw new Exception($"Brod sa ID-om {brod.ID} vec postoji.");
        }

        private static Brod provjeriBrod(string[] vrijednosti)
        {
            return new()
            {
                ID = postaviID(vrijednosti[0]),
                oznakaBroda = postaviOznakuBroda(vrijednosti[1]),
                naziv = postaviNaziv(vrijednosti[2]),
                vrsta = postaviVrstu(vrijednosti[3]),
                duljina = postaviDuljinu(vrijednosti[4]),
                sirina = postaviSirinu(vrijednosti[5]),
                gaz = postaviGaz(vrijednosti[6]),
                maksimalnaBrzina = postaviMaksimalnuBrzinu(vrijednosti[7]),
                kapacitetPutnika = postaviKapacitetPutnika(vrijednosti[8]),
                kapacitetOsobnihVozila = postaviKapacitetOsobnihVozila(vrijednosti[9]),
                kapacitetTereta = postaviKapacitetTereta(vrijednosti[10])
            };
        }

        private static int postaviID(string stringID)
        {
            return int.TryParse(stringID, out int id)
                 ? id
                 : throw new Exception("ID nije cijeli broj.");
        }

        private static string postaviOznakuBroda(string stringOznakaBroda)
        {
            return Regex.Match(stringOznakaBroda, @"^[A-Z]{5}$").Success == true
                 ? stringOznakaBroda
                 : throw new Exception("Oznaka broda nije ispravna.");
        }

        private static string postaviNaziv(string stringNaziv)
        {
            return Regex.Match(stringNaziv, @"^[A-Za-z]+$").Success == true
                 ? stringNaziv
                 : throw new Exception("Naziv broda neispravan.");
        }

        private static string postaviVrstu(string stringVrsta)
        {
            return Regex.Match(stringVrsta, @"^[A-Z]{2}$").Success == true
                 ? stringVrsta
                 : throw new Exception("Vrsta broda neispravna.");
        }

        private static double postaviDuljinu(string stringDuljina)
        {
            return double.TryParse(stringDuljina, out double dohvacenaDuljina)
                 ? dohvacenaDuljina
                 : throw new Exception("Duljina nije broj.");
        }

        private static double postaviSirinu(string stringSirina)
        {
            return double.TryParse(stringSirina, out double dohvacenaSirina)
                 ? dohvacenaSirina
                 : throw new Exception("Sirina nije broj.");
        }

        private static double postaviGaz(string stringGaz)
        {
            return double.TryParse(stringGaz, out double dohvaceniGaz)
                 ? dohvaceniGaz
                 : throw new Exception("Gaz nije broj.");
        }

        private static double postaviMaksimalnuBrzinu(string stringMaksimalnaBrzina)
        {
            return double.TryParse(stringMaksimalnaBrzina, out double dohvacenaMaksimalnaBrzina)
                 ? dohvacenaMaksimalnaBrzina
                 : throw new Exception("Maksimalna brzina nije broj.");
        }

        private static int postaviKapacitetPutnika(string stringKapacitetPutnika)
        {
            return int.TryParse(stringKapacitetPutnika, out int dohvatiKapacitetPutnika)
                 ? dohvatiKapacitetPutnika
                 : throw new Exception("Kapacitet putnika nije cijeli broj.");
        }

        private static int postaviKapacitetOsobnihVozila(string stringKapacitetOsobnihVozila)
        {
            return int.TryParse(stringKapacitetOsobnihVozila, out int dohvaceniKapacitetOsobnihVozila)
                 ? dohvaceniKapacitetOsobnihVozila
                 : throw new Exception("Kapacitet osobnih vozila nije cijeli broj.");
        }

        private static int postaviKapacitetTereta(string stringKapacitetTereta)
        {
            return int.TryParse(stringKapacitetTereta, out int dohvaceniKapacitetTereta)
                 ? dohvaceniKapacitetTereta
                 : throw new Exception("Kapacitet tereta nije broj cijeli.");
        }
    }
}
