using mnizic_zadaca_3.Composite;
using mnizic_zadaca_3.Composite.Vezovi;
using mnizic_zadaca_3.Singleton;
using mnizic_zadaca_3.IteratorPattern;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mnizic_zadaca_3.MVC.Controllers.PodaciController;
using mnizic_zadaca_3.MVC.Models;
using mnizic_zadaca_3.MVC.Views;

namespace mnizic_zadaca_3.MVC.Controllers.KomandeController
{
    public class KomandaIController
    {
        public static SortedDictionary<int, string> pregledVezova = new();

        public static void ispisiTablicu()
        {
            pregledVezova.Clear();
            if (KraticeZaIspisSingleton.InstancaKraticeZaIspis.Zaglavlje) KomandeView.ispisiZaglavlje("I");

            foreach (Raspored r in RasporediController.listaZapisaRasporeda)
            {
                if (zapisNeSadrziDanasnjiDanUTjednu(r)) continue;
                DateTime virtualnoVrijeme = postaviVirtualnoVrijeme();
                napuniDictionaryRetcima(r, virtualnoVrijeme);
            }

            dodajSlobodneVezoveKojeNisuNaRasporedu();
            int ukupnoZapisa = ispisiRetkeTablice();
            if(KraticeZaIspisSingleton.InstancaKraticeZaIspis.Podnozje)
            {
                KomandeView.ispisiPodnozje("I", ukupnoZapisa);
            }
                
        }

        private static int ispisiRetkeTablice()
        {
            int redniBroj = 0;
            pregledVezova.ToList().ForEach(vez =>
            {
                string vrstaVeza = dohvatiVrstuVeza(vez.Key);
                ++redniBroj;
                if (KraticeZaIspisSingleton.InstancaKraticeZaIspis.RedniBrojevi)
                {
                    KomandeView.ispisiOdgovor(string.Format("|{0,10}|{1,10}|{2,-10}|{3,-10}|",
                        redniBroj, vez.Key, vrstaVeza, vez.Value));
                }
                else
                {
                    KomandeView.ispisiOdgovor(string.Format("|{0,10}|{1,-10}|{2,-10}|",
                        vez.Key, vrstaVeza, vez.Value));
                }
            });

            return redniBroj;
        }

        private static void napuniDictionaryRetcima(Raspored r, DateTime virtualnoVrijeme)
        {
            _ = pregledVezova.ContainsKey(r.IDVez) == true
                  ? vezJeZauzetUVremenskomRasponu(r, virtualnoVrijeme) == true
                  ? pregledVezova[r.IDVez] = "Z"
                  : null
                  : vezJeZauzetUVremenskomRasponu(r, virtualnoVrijeme) == true
                  ? pregledVezova[r.IDVez] = "Z"
                  : pregledVezova[r.IDVez] = "S";
        }

        private static string dohvatiVrstuVeza(int idVeza)
        {
            if (PutnickiVezovi.putnickiVezoviLista.ContainsID(idVeza))
            {
                return PutnickiVezovi.putnickiVezoviLista.Find(idVeza).vrsta;
            }
            else if (PoslovniVezovi.poslovniVezoviLista.ContainsID(idVeza))
            {
                return PoslovniVezovi.poslovniVezoviLista.Find(idVeza).vrsta;
            }
            else if (OstaliVezovi.ostaliVezoviLista.ContainsID(idVeza))
            {
                return OstaliVezovi.ostaliVezoviLista.Find(idVeza).vrsta;
            }
            return "";
        }

        private static void dodajSlobodneVezoveKojeNisuNaRasporedu()
        {
            for (Iterator iter = (Iterator)PutnickiVezovi.putnickiVezoviLista.GetEnumerator(); iter.MoveNext();)
            {
                PutnickiVezovi pu = (PutnickiVezovi)iter.Current();
                if (!pregledVezova.ContainsKey(pu.ID))
                {
                    pregledVezova.Add(pu.ID, "S");
                }
            }
            for (Iterator iter = (Iterator)PoslovniVezovi.poslovniVezoviLista.GetEnumerator(); iter.MoveNext();)
            {
                PoslovniVezovi po = (PoslovniVezovi)iter.Current();
                if (!pregledVezova.ContainsKey(po.ID))
                {
                    pregledVezova.Add(po.ID, "S");
                }
            }
            for (Iterator iter = (Iterator)OstaliVezovi.ostaliVezoviLista.GetEnumerator(); iter.MoveNext();)
            {
                OstaliVezovi os = (OstaliVezovi)iter.Current();
                if (!pregledVezova.ContainsKey(os.ID))
                {
                    pregledVezova.Add(os.ID, "S");
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
