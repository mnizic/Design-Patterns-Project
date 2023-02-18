using mnizic_zadaca_3.MVC.Models;
using mnizic_zadaca_3.MVC.Views;
using mnizic_zadaca_3.Singleton;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.MVC.Controllers.PodaciController
{
    public class RasporediController
    {
        public static List<Raspored> listaZapisaRasporeda { get; set; } = new();

        public static void DohvatiRaspored(string linijaUDatoteci)
        {
            string[] dohvaceneVrijednosti = linijaUDatoteci.Split(';');

            try
            {
                provjeriBrojDohvacenihVrijednosti(dohvaceneVrijednosti);
                Raspored raspored = provjeriRaspored(dohvaceneVrijednosti);
                provjeriZauzetost(raspored);
                listaZapisaRasporeda.Add(raspored);
            }
            catch (Exception ex)
            {
                PodaciView.ispisGreske(++BrojacGresakaSingleton.InstancaBrojacGresaka.brojGreske, ex.Message);
            }
        }

        private static void provjeriZauzetost(Raspored raspored)
        {
            listaZapisaRasporeda.ForEach(r =>
            {
                if (vezJeZauzetUVremenskomRasponu(r, raspored))
                {
                    r.daniUTjednu.ForEach(dan =>
                    {
                        if (raspored.daniUTjednu.Contains(dan))
                        {
                            throw new Exception($"Vez {raspored.IDVez} je vec zauzet " +
                                $"u vrijeme {raspored.vrijemeOd.ToString("HH:mm")}.");
                        }
                    });
                }
            });
        }

        private static bool vezJeZauzetUVremenskomRasponu(Raspored r, Raspored raspored)
        {
            if (r.IDVez == raspored.IDVez &&
                    (r.vrijemeOd.Hour < raspored.vrijemeOd.Hour &&
                    r.vrijemeDo.Hour > raspored.vrijemeOd.Hour
                    ||
                    r.vrijemeOd.Hour == raspored.vrijemeOd.Hour &&
                    r.vrijemeOd.Minute <= raspored.vrijemeOd.Minute
                    ||
                    r.vrijemeDo.Hour == raspored.vrijemeOd.Hour &&
                    r.vrijemeDo.Minute >= raspored.vrijemeOd.Minute))
            {
                return true;
            }
            return false;
        }

        private static void provjeriBrojDohvacenihVrijednosti(string[] dohvaceneVrijednosti)
        {
            if (dohvaceneVrijednosti.Length != 5) throw new Exception("Netocan broj atributa.");
        }

        private static Raspored provjeriRaspored(string[] dohvaceneVrijednosti)
        {
            return new()
            {
                IDVez = postaviIDVeza(dohvaceneVrijednosti[0]),
                IDBrod = postaviIDBroda(dohvaceneVrijednosti[1]),
                daniUTjednu = postaviDaneUTjednu(dohvaceneVrijednosti[2]),
                vrijemeOd = postaviVrijemeOd(dohvaceneVrijednosti[3]),
                vrijemeDo = postaviVrijemeDo(dohvaceneVrijednosti[4])
            };
        }

        private static List<int> postaviDaneUTjednu(string stringDaniUTjednu)
        {
            List<int> daniUTjednu = new List<int>();
            string[] dani = stringDaniUTjednu.Split(",");

            foreach (string d in dani)
            {
                int integerDan = postaviDanUTjednu(d);
                if (integerDan < 0 || integerDan > 6) throw new Exception("Dan nije u rasponu od 0 do 6.");
                daniUTjednu.Add(integerDan);
            }

            return daniUTjednu;
        }

        private static int postaviIDVeza(string stringIDVeza)
        {
            return int.TryParse(stringIDVeza, out int dohvaceniIDVeza)
                 ? dohvaceniIDVeza
                 : throw new Exception("ID veza nije cijeli broj.");
        }

        private static int postaviIDBroda(string stringIDBroda)
        {
            return int.TryParse(stringIDBroda, out int dohvaceniIDBroda)
                 ? dohvaceniIDBroda
                 : throw new Exception("ID broda nije cijeli broj.");
        }

        private static int postaviDanUTjednu(string stringDan)
        {
            return int.TryParse(stringDan, out int dan) ? dan
                 : throw new Exception("Dani nisu zapisani kao brojevi.");
        }

        private static DateTime postaviVrijemeOd(string stringVrijemeOd)
        {
            return DateTime.TryParseExact(stringVrijemeOd, "HH:mm",
                   CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dohvacenoVrijemeOd)
                 ? dohvacenoVrijemeOd : throw new Exception("Sati su neispravno zapisani.");
        }

        private static DateTime postaviVrijemeDo(string stringVrijemeDo)
        {
            return DateTime.TryParseExact(stringVrijemeDo, "HH:mm",
                   CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dohvacenoVrijemeDo)
                 ? dohvacenoVrijemeDo : throw new Exception("Sati su neispravno zapisani.");
        }
    }
}
