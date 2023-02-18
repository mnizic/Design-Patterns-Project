using mnizic_zadaca_3.Composite.Vezovi;
using mnizic_zadaca_3.IteratorPattern;
using mnizic_zadaca_3.MVC.Controllers.PodaciController;
using mnizic_zadaca_3.MVC.Models;
using mnizic_zadaca_3.MVC.Views;
using mnizic_zadaca_3.Singleton;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.MVC.Controllers.KomandeController
{
    public class KomandaSPSVPSController
    {
        public static SortedDictionary<string, List<StanjeVezova>> popisStanja = new();
        public static List<StanjeVezova> listaVezova;

        public static void pohraniPostojeceStanje(string komanda)
        {
            listaVezova = new();
            dodajSlobodneVezoveKojeNisuNaRasporedu();

            foreach (Raspored r in RasporediController.listaZapisaRasporeda)
            {
                if (zapisNeSadrziDanasnjiDanUTjednu(r)) continue;
                DateTime virtualnoVrijeme = postaviVirtualnoVrijeme();
                napuniDictionaryRetcima(r, virtualnoVrijeme);
            }

            try
            {
                string naziv = provjeriKomandu(komanda).Split("\"")[1];
                popisStanja.Add(naziv, listaVezova);
                KomandeView.ispisiOdgovor($"Pohranjeno stanje pod nazivom \"{naziv}\""); 
            } 
            catch (Exception ex)
            {
                KomandeView.ispisiOdgovor(ex.Message);
            }  
        }

        private static string provjeriKomandu(string komanda)
        {
            return Regex.Match(komanda, @"^(SPS|VPS) "".+""$").Success
                     ? komanda
                     : throw new Exception("Ispravan format komande je (SPS|VPS) \"[naziv]\".");
        }

        public static void dohvatiPohranjenoStanje(string komanda)
        {
            try
            {
                string naziv = provjeriKomandu(komanda).Split("\"")[1];
                if (postojiKey(naziv))
                {
                    KomandaVRController.postaviNovoVirtualnoVrijeme("VR " + popisStanja[naziv][0].VirtualnoVrijeme.ToString("dd.MM.yyyy. HH:mm:ss"));
                } 
                else
                {
                    throw new Exception($"Ne postoji stanje s navedenim nazivom: \"{naziv}\"");
                }
            }
            catch (Exception ex)
            {
                KomandeView.ispisiOdgovor(ex.Message);
            }
        }

        private static bool postojiKey(string naziv)
        {
            if(popisStanja[naziv] != null) return true;
            return false;
        }

        private static void napuniDictionaryRetcima(Raspored r, DateTime virtualnoVrijeme)
        {
            _ = listaVezova.Any(x => x.ID == r.IDVez) == true
                  ? vezJeZauzetUVremenskomRasponu(r, virtualnoVrijeme) == true
                  ? listaVezova.Find(x => x.ID == r.IDVez).Status = "Z"
                  : null
                  : vezJeZauzetUVremenskomRasponu(r, virtualnoVrijeme) == true
                  ? listaVezova.Find(x => x.ID == r.IDVez).Status = "Z"
                  : listaVezova.Find(x => x.ID == r.IDVez).Status = "S";
        }

        private static void dodajSlobodneVezoveKojeNisuNaRasporedu()
        {
            for (Iterator iter = (Iterator)PutnickiVezovi.putnickiVezoviLista.GetEnumerator(); iter.MoveNext();)
            {
                PutnickiVezovi pu = (PutnickiVezovi)iter.Current();
                if (!listaVezova.Any(x => x.ID == pu.ID))
                {
                    StanjeVezova sv = new()
                    {
                        ID = pu.ID,
                        Status = "S",
                        Vrsta = "PU",
                        VirtualnoVrijeme = VirtualnoVrijemeSingleton.InstancaVirtualnoVrijeme.virtualnoVrijeme
                    };
                    listaVezova.Add(sv);
                }
            }
            for (Iterator iter = (Iterator)PoslovniVezovi.poslovniVezoviLista.GetEnumerator(); iter.MoveNext();)
            {
                PoslovniVezovi po = (PoslovniVezovi)iter.Current();
                if (!listaVezova.Any(x => x.ID == po.ID))
                {
                    StanjeVezova sv = new()
                    {
                        ID = po.ID,
                        Status = "S",
                        Vrsta = "PO",
                        VirtualnoVrijeme = VirtualnoVrijemeSingleton.InstancaVirtualnoVrijeme.virtualnoVrijeme
                    };
                    listaVezova.Add(sv);
                }
            }
            for (Iterator iter = (Iterator)OstaliVezovi.ostaliVezoviLista.GetEnumerator(); iter.MoveNext();)
            {
                OstaliVezovi os = (OstaliVezovi)iter.Current();
                if (!listaVezova.Any(x => x.ID == os.ID))
                {
                    StanjeVezova sv = new()
                    {
                        ID = os.ID,
                        Status = "S",
                        Vrsta = "OS",
                        VirtualnoVrijeme = VirtualnoVrijemeSingleton.InstancaVirtualnoVrijeme.virtualnoVrijeme
                    };
                    listaVezova.Add(sv);
                }
            }
        }

        private static bool zapisNeSadrziDanasnjiDanUTjednu(Raspored r)
        {
            int danasnjiDan = (int)VirtualnoVrijemeSingleton.InstancaVirtualnoVrijeme.virtualnoVrijeme.DayOfWeek;
            return !r.daniUTjednu.Contains(danasnjiDan);
        }

        private static DateTime postaviVirtualnoVrijeme()
        {
            string vrijeme = VirtualnoVrijemeSingleton.InstancaVirtualnoVrijeme.virtualnoVrijeme.ToString("HH:mm");
            return DateTime.ParseExact(vrijeme, "HH:mm",
                   CultureInfo.InvariantCulture, DateTimeStyles.None);
        }

        private static bool vezJeZauzetUVremenskomRasponu(Raspored r, DateTime virtualnoVrijeme)
        {
            if (r.vrijemeOd.Hour < virtualnoVrijeme.Hour &&
                    r.vrijemeDo.Hour > virtualnoVrijeme.Hour
                    ||
                    r.vrijemeOd.Hour == virtualnoVrijeme.Hour &&
                    r.vrijemeOd.Minute <= virtualnoVrijeme.Minute
                    &&
                    r.vrijemeDo.Hour == virtualnoVrijeme.Hour &&
                    r.vrijemeDo.Minute >= virtualnoVrijeme.Minute)
            {
                return true;
            }
            return false;
        }
    }
}
