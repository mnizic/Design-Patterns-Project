using mnizic_zadaca_3.Composite.Vezovi;
using mnizic_zadaca_3.IteratorPattern;
using mnizic_zadaca_3.Singleton;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using mnizic_zadaca_3.MVC.Controllers.PodaciController;
using mnizic_zadaca_3.MVC.Models;
using mnizic_zadaca_3.MVC.Views;

namespace mnizic_zadaca_3.MVC.Controllers.KomandeController
{
    public class KomandaVController
    {
        public static void ispisVezova(string komanda)
        {
            List<Raspored> listaVezovaTrazeneVrste = new();
            string[] splitKomande = komanda.Split(" ");
            try
            {
                provjeriIspravnostKomandeV(splitKomande);
                string vrstaVeza = postaviVrstuVeza(splitKomande[1]);
                string status = postaviStatus(splitKomande[2]);
                DateTime vrijemeOd = postaviDatum(splitKomande[3], splitKomande[4]);
                DateTime vrijemeDo = postaviDatum(splitKomande[5], splitKomande[6]);

                if (vrijemeOd.CompareTo(vrijemeDo) > 0) throw new Exception("Vrijeme od ne moze biti vece od vremena do.");

                if(KraticeZaIspisSingleton.InstancaKraticeZaIspis.Zaglavlje) KomandeView.ispisiZaglavlje("V");

                listaVezovaTrazeneVrste.AddRange(provjeriVrstu(vrstaVeza, listaVezovaTrazeneVrste));
                listaVezovaTrazeneVrste = provjeriRasponDatumaUDanima(vrijemeOd, vrijemeDo, status, listaVezovaTrazeneVrste);
                listaVezovaTrazeneVrste = provjeriDan((int)vrijemeOd.DayOfWeek,
                    (int)vrijemeDo.DayOfWeek, listaVezovaTrazeneVrste);

                if (status.Equals("S"))
                {
                    listaVezovaTrazeneVrste.AddRange(dodajSlobodneVezoveKojiNisuNaRasporedu(vrstaVeza,
                        vrijemeOd, vrijemeDo, listaVezovaTrazeneVrste));
                }

                konacanIspis(listaVezovaTrazeneVrste, vrstaVeza, status);
            }
            catch (Exception ex)
            {
                KomandeView.ispisiOdgovor(ex.Message);
            }
        }

        private static void provjeriIspravnostKomandeV(string[] splitKomande)
        {
            if (splitKomande.Length != 7) throw new Exception($"Komanda {splitKomande[0]} je neispravna.");
        }

        private static List<Raspored> dodajSlobodneVezoveKojiNisuNaRasporedu(string vrstaVeza, DateTime zadanoVrijemeOd,
            DateTime zadanoVrijemeDo, List<Raspored> listaVezovaTrazeneVrste)
        {
            if (vrstaVeza.Equals("PU"))
            {
                listaVezovaTrazeneVrste = napuniPutnickimVezovima(zadanoVrijemeOd,
                    zadanoVrijemeDo, listaVezovaTrazeneVrste);

            }
            else if (vrstaVeza.Equals("PO"))
            {
                listaVezovaTrazeneVrste = napuniPoslovnimVezovima(zadanoVrijemeOd,
                    zadanoVrijemeDo, listaVezovaTrazeneVrste);
            }
            else if (vrstaVeza.Equals("OS"))
            {
                listaVezovaTrazeneVrste = napuniOstalimVezovima(zadanoVrijemeOd,
                    zadanoVrijemeDo, listaVezovaTrazeneVrste);
            }
            return listaVezovaTrazeneVrste;
        }

        private static List<Raspored> napuniPutnickimVezovima(DateTime zadanoVrijemeOd, DateTime zadanoVrijemeDo,
            List<Raspored> listaVezovaTrazeneVrste)
        {
            for (Iterator iter = (Iterator)PutnickiVezovi.putnickiVezoviLista.GetEnumerator(); iter.MoveNext();)
            {
                PutnickiVezovi pv = (PutnickiVezovi)iter.Current();
                if (!listaVezovaTrazeneVrste.Any(x => x.IDVez == pv.ID))
                {
                    Raspored r = new()
                    {
                        IDVez = pv.ID,
                        IDBrod = 0,
                        daniUTjednu = null,
                        vrijemeOd = zadanoVrijemeOd,
                        vrijemeDo = zadanoVrijemeDo
                    };

                    listaVezovaTrazeneVrste.Add(r);
                }
            }

            return listaVezovaTrazeneVrste;
        }

        private static List<Raspored> napuniPoslovnimVezovima(DateTime zadanoVrijemeOd, DateTime zadanoVrijemeDo,
            List<Raspored> listaVezovaTrazeneVrste)
        {
            for (Iterator iter = (Iterator)PoslovniVezovi.poslovniVezoviLista.GetEnumerator(); iter.MoveNext();)
            {
                PoslovniVezovi pv = (PoslovniVezovi)iter.Current();
                if (!listaVezovaTrazeneVrste.Any(x => x.IDVez == pv.ID))
                {
                    Raspored r = new()
                    {
                        IDVez = pv.ID,
                        IDBrod = 0,
                        daniUTjednu = null,
                        vrijemeOd = zadanoVrijemeOd,
                        vrijemeDo = zadanoVrijemeDo
                    };

                    listaVezovaTrazeneVrste.Add(r);
                }
            }
            return listaVezovaTrazeneVrste;
        }

        private static List<Raspored> napuniOstalimVezovima(DateTime zadanoVrijemeOd, DateTime zadanoVrijemeDo,
            List<Raspored> listaVezovaTrazeneVrste)
        {
            for (Iterator iter = (Iterator)OstaliVezovi.ostaliVezoviLista.GetEnumerator(); iter.MoveNext();)
            {
                OstaliVezovi os = (OstaliVezovi)iter.Current();
                if (!listaVezovaTrazeneVrste.Any(x => x.IDVez == os.ID))
                {
                    Raspored r = new()
                    {
                        IDVez = os.ID,
                        IDBrod = 0,
                        daniUTjednu = null,
                        vrijemeOd = zadanoVrijemeOd,
                        vrijemeDo = zadanoVrijemeDo
                    };

                    listaVezovaTrazeneVrste.Add(r);
                }
            }
            return listaVezovaTrazeneVrste;
        }

        private static List<Raspored> provjeriRasponDatumaUDanima(DateTime vrijemeOd, DateTime vrijemeDo,
            string status, List<Raspored> listaVezovaTrazeneVrste)
        {
            if ((int)vrijemeOd.DayOfWeek == (int)vrijemeDo.DayOfWeek)
            {
                return provjeriZauzetostUDanoVrijeme(vrijemeOd, vrijemeDo,
                status, listaVezovaTrazeneVrste).Distinct().ToList();
            }
            else
            {
                if (status == "S")
                {
                    return dohvatiSveSlobodneVezove(vrijemeOd, vrijemeDo, listaVezovaTrazeneVrste).Distinct().ToList();
                }
            }
            return listaVezovaTrazeneVrste.Distinct().ToList();
        }

        private static List<Raspored> dohvatiSveSlobodneVezove(DateTime vrijemeOd, DateTime vrijemeDo,
            List<Raspored> listaVezovaTrazeneVrste)
        {
            List<Raspored> listaSlobodnihVezova = new List<Raspored>();
            listaVezovaTrazeneVrste = listaVezovaTrazeneVrste.Distinct().ToList();

            listaSlobodnihVezova = napuniListuSlobodnimVezovima(vrijemeOd, vrijemeDo, listaVezovaTrazeneVrste);
            return (int)vrijemeOd.DayOfWeek == (int)vrijemeDo.DayOfWeek
            ? listaSlobodnihVezova.Distinct().ToList()
            : listaSlobodnihVezova.GroupBy(q => q.IDVez).SelectMany(g => g.Take(1)).ToList();
        }

        private static List<Raspored> napuniListuSlobodnimVezovima(DateTime vrijemeOd, DateTime vrijemeDo,
            List<Raspored> listaVezovaTrazeneVrste)
        {
            List<Raspored> listaSlobodnihVezova = new List<Raspored>();
            listaVezovaTrazeneVrste.ForEach(delegate (Raspored r)
            {
                if (!vezJeZauzetUVremenskomRasponu(r, vrijemeOd, r.vrijemeOd) &&
                vrijemeOd.ToShortTimeString() != r.vrijemeOd.ToShortTimeString()
                && vrijemeOd.Hour < r.vrijemeOd.Hour)
                {
                    Raspored noviRaspored = new()
                    {
                        IDVez = r.IDVez,
                        IDBrod = r.IDBrod,
                        daniUTjednu = r.daniUTjednu,
                        vrijemeOd = vrijemeOd,
                        vrijemeDo = r.vrijemeOd
                    };
                    listaSlobodnihVezova.Add(noviRaspored);
                }
                else if (!vezJeZauzetUVremenskomRasponu(r, r.vrijemeDo, vrijemeDo) &&
                vrijemeDo.ToShortTimeString() != r.vrijemeOd.ToShortTimeString()
                && vrijemeOd.Hour < r.vrijemeOd.Hour)
                {
                    Raspored noviRaspored = new()
                    {
                        IDVez = r.IDVez,
                        IDBrod = r.IDBrod,
                        daniUTjednu = r.daniUTjednu,
                        vrijemeOd = r.vrijemeDo,
                        vrijemeDo = vrijemeDo
                    };
                    listaSlobodnihVezova.Add(noviRaspored);
                }
            });
            return listaSlobodnihVezova;
        }

        private static List<Raspored> provjeriZauzetostUDanoVrijeme(DateTime zadanoVrijemeOd, DateTime zadanoVrijemeDo,
            string status, List<Raspored> listaVezovaTrazeneVrste)
        {
            List<Raspored> listaZauzetihVezova = new();
            List<Raspored> listaSlobodnihVezova = new();

            listaVezovaTrazeneVrste.ForEach(delegate (Raspored r)
            {
                if (vezJeZauzetUVremenskomRasponu(r, zadanoVrijemeOd, zadanoVrijemeDo))
                {
                    listaZauzetihVezova.Add(r);
                }
            });

            listaZauzetihVezova = listaZauzetihVezova.Distinct().ToList();
            listaSlobodnihVezova = dohvatiSveSlobodneVezove(zadanoVrijemeOd, zadanoVrijemeDo, listaZauzetihVezova);

            return status == "Z" ? listaZauzetihVezova.Distinct().ToList() : listaSlobodnihVezova.Distinct().ToList();
        }

        public static bool vezJeZauzetUVremenskomRasponu(Raspored r, DateTime vrijemeOd, DateTime vrijemeDo)
        {
            if (r.vrijemeOd.Hour > vrijemeOd.Hour &&
                r.vrijemeDo.Hour < vrijemeDo.Hour &&
                r.vrijemeOd.Hour < vrijemeDo.Hour
                ||
                r.vrijemeOd.Hour == vrijemeOd.Hour && r.vrijemeDo.Hour < vrijemeDo.Hour &&
                r.vrijemeOd.Minute >= vrijemeOd.Minute
                ||
                r.vrijemeOd.Hour > vrijemeOd.Hour && r.vrijemeDo.Hour == vrijemeDo.Hour &&
                r.vrijemeDo.Minute <= vrijemeDo.Minute
                ||
                r.vrijemeOd.Hour == vrijemeOd.Hour && r.vrijemeDo.Hour == vrijemeDo.Hour &&
                r.vrijemeOd.Minute >= vrijemeOd.Minute && r.vrijemeDo.Minute <= vrijemeDo.Minute)
            {
                return true;
            }
            return false;
        }

        private static string postaviVrstuVeza(string stringVrsta)
        {
            return Regex.Match(stringVrsta, @"(^((PU)|(PO)|(OS))$)").Success == true
                 ? stringVrsta
                 : throw new Exception("Vrsta veza neispravna.");
        }

        private static string postaviStatus(string stringStatus)
        {
            return Regex.Match(stringStatus, @"(^(Z|S)$)").Success == true
                 ? stringStatus
                 : throw new Exception("Status neispravan.");
        }

        private static DateTime postaviDatum(string datum, string vrijeme)
        {
            return DateTime.TryParseExact(datum + " " + vrijeme, "dd.MM.yyyy. HH:mm:ss",
                   CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime datumVrijeme)
                 ? datumVrijeme : throw new Exception("Datum i vrijeme su neispravni.");
        }

        private static void konacanIspis(List<Raspored> listaVezovaDaneVrste, string vrstaVeza, string status)
        {
            listaVezovaDaneVrste = listaVezovaDaneVrste.Distinct().OrderBy(x => x.IDVez).ToList();
            int redniBroj = 0;
            string ispis = "";
            listaVezovaDaneVrste.ForEach((Action<Raspored>)(r =>
            {
                ++redniBroj;
                ispis = KraticeZaIspisSingleton.InstancaKraticeZaIspis.RedniBrojevi == true
                      ? string.Format("|{0,10}|{1,10}|{2,-15}|{3,-10}|{4,20}|{5,20}|",
                        redniBroj, r.IDVez, vrstaVeza, status, r.vrijemeOd.ToShortTimeString(), r.vrijemeDo.ToShortTimeString())
                      : string.Format("|{0,10}|{1,-15}|{2,-10}|{3,20}|{4,20}|",
                        r.IDVez, vrstaVeza, status, r.vrijemeOd.ToShortTimeString(), r.vrijemeDo.ToShortTimeString());

                KomandeView.ispisiOdgovor(ispis);
            }));

            if (KraticeZaIspisSingleton.InstancaKraticeZaIspis.Podnozje) KomandeView.ispisiPodnozje("V", redniBroj);
        }

        private static List<Raspored> provjeriDan(int prviDan, int zadnjiDan,
             List<Raspored> listaVezovaDaneVrste)
        {
            List<Raspored> pomocnaListaVezova = new List<Raspored>();
            pomocnaListaVezova.AddRange(listaVezovaDaneVrste);

            foreach (Raspored r in pomocnaListaVezova)
            {
                if (r.daniUTjednu.Contains(prviDan) || r.daniUTjednu.Contains(zadnjiDan)) continue;
                while (prviDan <= zadnjiDan)
                {
                    if (prviDan == zadnjiDan)
                    {
                        listaVezovaDaneVrste.Remove(r);
                        break;
                    }
                    if (r.daniUTjednu.Contains(prviDan)) break;
                    if (prviDan == 6)
                    {
                        prviDan = 0;
                    }
                    else
                    {
                        prviDan++;
                    }
                }
            }

            return listaVezovaDaneVrste.Distinct().ToList();
        }

        private static List<Raspored> provjeriVrstu(string vrstaVeza, List<Raspored> listaVezovaTrazeneVrste)
        {

            if (vrstaVeza.Equals("PU"))
            {
                for (Iterator iter = (Iterator)PutnickiVezovi.putnickiVezoviLista.GetEnumerator(); iter.MoveNext();)
                {
                    PutnickiVezovi pv = (PutnickiVezovi)iter.Current();
                    listaVezovaTrazeneVrste.AddRange(RasporediController.listaZapisaRasporeda
                            .FindAll(raspored => raspored.IDVez == pv.ID));
                }
            }
            else if (vrstaVeza.Equals("PO"))
            {
                for (Iterator iter = (Iterator)PoslovniVezovi.poslovniVezoviLista.GetEnumerator(); iter.MoveNext();)
                {
                    PoslovniVezovi pv = (PoslovniVezovi)iter.Current();
                    listaVezovaTrazeneVrste.AddRange(RasporediController.listaZapisaRasporeda
                            .FindAll(raspored => raspored.IDVez == pv.ID));
                }
            }
            else if (vrstaVeza.Equals("OS"))
            {
                for (Iterator iter = (Iterator)OstaliVezovi.ostaliVezoviLista.GetEnumerator(); iter.MoveNext();)
                {
                    OstaliVezovi os = (OstaliVezovi)iter.Current();
                    listaVezovaTrazeneVrste.AddRange(RasporediController.listaZapisaRasporeda
                            .FindAll(raspored => raspored.IDVez == os.ID));
                }
            }
            else
            {
                throw new Exception("Neispravna vrsta vezova.");
            }

            return listaVezovaTrazeneVrste;
        }
    }
}
