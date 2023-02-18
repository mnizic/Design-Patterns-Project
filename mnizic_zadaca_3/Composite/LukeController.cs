using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using mnizic_zadaca_3.Singleton;
using mnizic_zadaca_3.IteratorPattern.Collections;
using mnizic_zadaca_3.MVC.Models;
using mnizic_zadaca_3.MVC.Views;

namespace mnizic_zadaca_3.Composite
{
    public class LukeController : IBrodskaLuka
    {
        public static LukeCollection listaLuka = new();

        public List<IBrodskaLuka> segmentiLuke = new();

        public static void DohvatiLuke(string linijaUDatoteci)
        {
            string[] dohvaceneVrijednosti = linijaUDatoteci.Split(';');
            try
            {
                Luka luka = provjeriLuke(dohvaceneVrijednosti);
                provjeriBrojDohvacenihVrijednosti(dohvaceneVrijednosti);
                provjeriDuplikat(luka);
                listaLuka.DodajLuku(luka);
            }
            catch (Exception ex)
            {
                PodaciView.ispisGreske(++BrojacGresakaSingleton.InstancaBrojacGresaka.brojGreske, ex.Message);
            }
        }
        private static void provjeriDuplikat(Luka luka)
        {
            if (listaLuka.Any(luka)) throw new Exception($"Luka naziva {luka.naziv} vec postoji.");
        }

        private static void provjeriBrojDohvacenihVrijednosti(string[] dohvaceneVrijednosti)
        {
            if (dohvaceneVrijednosti.Length != 8) throw new Exception("Netocan broj atributa.");
        }

        public static Luka provjeriLuke(string[] vrijednosti)
        {
            VrijemeUcitavanjaSingleton.InstancaVrijemeUcitavanja.Vrijeme = DateTime.Now;

            return new()
            {
                naziv = postaviNaziv(vrijednosti[0]),
                gpsSirina = postaviGPSSirinu(vrijednosti[1]),
                gpsVisina = postaviGPSVisinu(vrijednosti[2]),
                dubinaLuke = postaviDubinuLuke(vrijednosti[3]),
                ukupniBrPutnickihVezova = postaviUkupniBrPutnicihVezova(vrijednosti[4]),
                ukupniBrPoslovnihVezova = postaviUkupniBrPoslovnihVezova(vrijednosti[5]),
                ukupniBrOstalihVezova = postaviUkupniBrOstalihVezova(vrijednosti[6]),
                virtualnoVrijeme = postaviVirtualnoVrijeme(vrijednosti[7])
            };
        }

        private static string postaviNaziv(string naziv)
        {
            return Regex.Match(naziv, @"(\w)+").Success == true
                 ? naziv
                 : throw new Exception("Naziv nije ispravan.");
        }

        private static double postaviGPSSirinu(string stringSirina)
        {
            return double.TryParse(stringSirina,
                                     out double gpsSirina)
                                   ? gpsSirina
                                   : throw new Exception("Geografska sirina neispravna.");
        }

        private static double postaviGPSVisinu(string stringVisina)
        {
            return double.TryParse(stringVisina,
                                     out double gpsVisina)
                                   ? gpsVisina
                                   : throw new Exception("Geografska duljina neispravna.");
        }
        private static double postaviDubinuLuke(string stringDubina)
        {
            return double.TryParse(stringDubina,
                                      out double dubina)
                                    ? dubina
                                    : throw new Exception("Dubina luke neispravna.");
        }
        private static double postaviUkupniBrPutnicihVezova(string stringUkupniBrPUVezova)
        {
            return double.TryParse(stringUkupniBrPUVezova,
                                                   out double dohvaceniBrojPutnichivVezova)
                                                 ? dohvaceniBrojPutnichivVezova
                                                 : throw new Exception("Ukupan broj putnickih vezova neispravan.");
        }
        private static double postaviUkupniBrPoslovnihVezova(string stringUkupniBrojPOVezova)
        {
            return double.TryParse(stringUkupniBrojPOVezova,
                                                   out double dohvaceniBrojPoslovnihVezova)
                                                 ? dohvaceniBrojPoslovnihVezova
                                                 : throw new Exception("Ukupan broj poslovnih vezova neispravan.");
        }
        private static double postaviUkupniBrOstalihVezova(string stringUkupniBrojOSVezova)
        {
            return double.TryParse(stringUkupniBrojOSVezova,
                                                 out double dohvaceniBrojOstalihVezova)
                                               ? dohvaceniBrojOstalihVezova
                                               : throw new Exception("Ukupan broj ostalih vezova neispravan.");
        }
        private static DateTime postaviVirtualnoVrijeme(string stringDatumVrijeme)
        {
            VirtualnoVrijemeSingleton.InstancaVirtualnoVrijeme.virtualnoVrijeme = DateTime.TryParseExact(stringDatumVrijeme,
                                            "dd.MM.yyyy. HH:mm:ss",
                                            CultureInfo.CurrentCulture, DateTimeStyles.None,
                                            out DateTime dohvacenoVirtualnoVrijeme)
                                          ? dohvacenoVirtualnoVrijeme
                                          : throw new Exception("Virtualno vrijeme neispravno.");
            return VirtualnoVrijemeSingleton.InstancaVirtualnoVrijeme.virtualnoVrijeme;
        }

        public void dodaj(IBrodskaLuka segmentLuke)
        {
            segmentiLuke.Add(segmentLuke);
        }

        public void ukloni(IBrodskaLuka segmentLuke)
        {
            segmentiLuke.Remove(segmentLuke);
        }

        public IBrodskaLuka dohvatiDijete(int i)
        {
            return segmentiLuke[i];
        }
    }
}
