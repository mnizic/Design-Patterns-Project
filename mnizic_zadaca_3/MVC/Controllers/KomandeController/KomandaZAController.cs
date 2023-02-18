using Microsoft.VisualBasic;
using mnizic_zadaca_3.Composite;
using mnizic_zadaca_3.Composite.Vezovi;
using mnizic_zadaca_3.Singleton;
using mnizic_zadaca_3.Visitor;
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
    public struct VrstaStatus
    {
        public string Vrsta;
        public string Status;
    }
    public class KomandaZAController
    {
        public static SortedDictionary<int, string> pregledVezova = new();
        public static Dictionary<int, VrstaStatus> vrstaStatusVezovi = new();

        public static void ispisiTablicu(string komanda)
        {
            pregledVezova.Clear();
            vrstaStatusVezovi.Clear();
            try
            {
                provjeriIspravnostKomandeZA(komanda);
                DateTime trazenoVrijeme = dohvatiDatum(komanda);
                if (KraticeZaIspisSingleton.InstancaKraticeZaIspis.Zaglavlje) KomandeView.ispisiZaglavlje("ZA");

                foreach (Raspored r in RasporediController.listaZapisaRasporeda)
                {
                    if (zapisNeSadrziDanasnjiDanUTjednu(r)) continue;
                    napuniDictionaryRetcima(r, trazenoVrijeme);
                }

                pregledVezova.ToList().ForEach(vez =>
                {
                    string vrstaVeza = dohvatiVrstuVeza(vez.Key);
                    VrstaStatus vs = new();
                    vs.Vrsta = vrstaVeza;
                    vs.Status = vez.Value;
                    vrstaStatusVezovi.Add(vez.Key, vs);
                });

                int ukupno = ispisiZbrojeveZauzetihVrstiVezova();
                if (KraticeZaIspisSingleton.InstancaKraticeZaIspis.Podnozje) 
                    KomandeView.ispisiPodnozje("ZA", ukupno);
            }
            catch (Exception ex)
            {
                KomandeView.ispisiOdgovor(ex.Message);
            }
        }

        private static void provjeriIspravnostKomandeZA(string komanda)
        {
            string[] splitKomande = komanda.Split(" ");
            if (splitKomande.Length != 3)
                throw new Exception($"Komanda {splitKomande[0]} sadrži krivi broj parametara.");
        }

        private static int ispisiZbrojeveZauzetihVrstiVezova()
        {
            ConcreteComponentVezoviPU ccvpu = new();
            ConcreteComponentVezoviPO ccvpo = new();
            ConcreteComponentVezoviOS ccvos = new();

            vrstaStatusVezovi.ToList().ForEach(v =>
            {
                if (v.Value.Vrsta.Equals("PU")) ccvpu.inkrementirajZbroj();
                if (v.Value.Vrsta.Equals("PO")) ccvpo.inkrementirajZbroj();
                if (v.Value.Vrsta.Equals("OS")) ccvos.inkrementirajZbroj();
            });

            ConcreteVisitorVezovi cvv = new ConcreteVisitorVezovi();

            if (ccvpu != null && ccvpo != null && ccvos != null)
            {
                ccvpu.Accept(cvv);
                ccvpo.Accept(cvv);
                ccvos.Accept(cvv);
            }

            return cvv.dohvatiBrojZapisa();
        }

        private static DateTime dohvatiDatum(string komanda)
        {
            DateTime trazenoVrijeme = new();
            string[] splitKomande = komanda.Split(" ");
            try
            {
                trazenoVrijeme = postaviDatumVrijeme(splitKomande[1], splitKomande[2]);
            }
            catch (Exception ex)
            {
                KomandeView.ispisiOdgovor(ex.Message);
            }
            return trazenoVrijeme;
        }

        private static DateTime postaviDatumVrijeme(string datum, string vrijeme)
        {
            return DateTime.TryParseExact(datum + " " + vrijeme, "dd.MM.yyyy. HH:mm",
                   CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime datumVrijeme)
                 ? datumVrijeme : throw new Exception("Datum i vrijeme su neispravni.");
        }

        private static void napuniDictionaryRetcima(Raspored r, DateTime trazenoVrijeme)
        {
            _ = pregledVezova.ContainsKey(r.IDVez) == true
                  ? vezJeZauzetUVremenskomRasponu(r, trazenoVrijeme) == true
                  ? pregledVezova[r.IDVez] = "Z"
                  : null
                  : vezJeZauzetUVremenskomRasponu(r, trazenoVrijeme) == true
                  ? pregledVezova[r.IDVez] = "Z"
                  : null;
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
