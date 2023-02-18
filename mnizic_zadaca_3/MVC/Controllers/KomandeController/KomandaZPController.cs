using Microsoft.VisualBasic;
using mnizic_zadaca_3.ChainOfResponsibility;
using mnizic_zadaca_3.Composite;
using mnizic_zadaca_3.Composite.Vezovi;
using mnizic_zadaca_3.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mnizic_zadaca_3.MVC.Controllers.PodaciController;
using mnizic_zadaca_3.MVC.Models;

namespace mnizic_zadaca_3.MVC.Controllers.KomandeController
{
    public class KomandaZPController
    {
        public static void provediZahtjev(string komanda)
        {
            Chain dnevnik = DnevnikZapisi.doChaining();
            string[] splitKomande = komanda.Split(" ");
            bool valjanZahtjev = false;
            try
            {
                provjeriIspravnostKomandeZP(komanda, splitKomande);
                int idBroda = postaviIdBroda(splitKomande[1]);
                int trajanjeUH = postaviTrajanjeUH(splitKomande[2]);
                provjeriKanalBroda(idBroda);

                Brod brod = new();
                brod = BrodoviController.brodoviLista.Find(x => x.ID == idBroda);

                if (brod != null)
                {
                    valjanZahtjev = provjeriVrijemeZahtjeva(brod, trajanjeUH);
                }

                DateTime vrijemeDo = valjanZahtjev == true
                    ? VirtualnoVrijemeSingleton.InstancaVirtualnoVrijeme.virtualnoVrijeme
                    : new();

                string ispis = valjanZahtjev == true ? $"Brod s ID {idBroda} - " +
                    $"Privez broda na {trajanjeUH}h (NEREZERVIRAN VEZ)\n" +
                    $"\tVrijeme: {VirtualnoVrijemeSingleton.InstancaVirtualnoVrijeme.virtualnoVrijeme} - {vrijemeDo.AddHours(trajanjeUH)} "
                    : $"U vrijeme zahtjeva nema slobodnih vezova za brod s ID {idBroda}";

                unesiDnevnikZapis(dnevnik, valjanZahtjev, ispis);
            }
            catch (Exception ex)
            {
                dnevnik.unesiZapisDnevnika(Chain.zahtjevNeispravan, ex.Message);
            }
        }

        private static void provjeriKanalBroda(int idBroda)
        {
            if (!KomandaFController.listaKanalBrodova.Any(x => x.listaBrodovaNaKanalu.Any(y => y.ID == idBroda)))
            {
                throw new Exception("Brod mora prvo biti spojen na kanal prije nego što može slati zahtjeve");
            }
        }

        private static void provjeriIspravnostKomandeZP(string komanda, string[] splitKomande)
        {
            if (splitKomande.Length != 3) throw new Exception($"Zahtjev {komanda} nije ispravan.");
        }

        private static void unesiDnevnikZapis(Chain dnevnik, bool valjanZahtjev, string ispis)
        {
            if (valjanZahtjev)
            {
                dnevnik.unesiZapisDnevnika(Chain.zahtjevPrihvacen, ispis);
            }
            else
            {
                dnevnik.unesiZapisDnevnika(Chain.zahtjevOdbijen, ispis);
            }
        }

        private static bool provjeriVrijemeZahtjeva(Brod brod, int trajanjeUH)
        {
            DateTime vrijemeOd = VirtualnoVrijemeSingleton.InstancaVirtualnoVrijeme.virtualnoVrijeme;
            DateTime vrijemeDo = vrijemeOd;
            vrijemeDo.AddHours(trajanjeUH);

            bool zauzetoRaspored = provjeriRaspored(brod, vrijemeOd);
            bool zauzetoRezervacija = provjeriRezervacije(vrijemeOd, brod);

            if (zauzetoRaspored || zauzetoRezervacija)
            {
                return true;
            }

            return false;
        }

        private static bool provjeriRezervacije(DateTime vrijemeOd, Brod brod)
        {
            bool zauzet = false;
            foreach (Rezervacija r in RezervacijeController.listaRezervacija)
            {
                zauzet = vezJeRezerviranUVremenskomRasponu(r, brod, vrijemeOd);

                if (zauzet) return true;
            }
            return zauzet;
        }

        private static bool vezJeRezerviranUVremenskomRasponu(Rezervacija r, Brod brod, DateTime virtualnoVrijeme)
        {
            long rDatumOdMillis = (long)pretvoriDatumVrijemeUMilisekunde(r.datumVrijemeOd);
            long rDatumDoMillis = rDatumOdMillis + pretvoriSateUMilisekunde(r.trajanjePrivezaUH);
            long virtualnoVrijemeMillis = (long)pretvoriDatumVrijemeUMilisekunde(virtualnoVrijeme);
            Brod brodSRezervacijom = BrodoviController.brodoviLista.Find(x => x.ID == r.IDBroda);

            if (brodSRezervacijom != null && brod.vrsta == brodSRezervacijom.vrsta &&
                rDatumOdMillis <= virtualnoVrijemeMillis && virtualnoVrijemeMillis <= rDatumDoMillis)
            {
                return true;
            }
            return false;
        }

        private static long pretvoriSateUMilisekunde(int trajanjePrivezaUH)
        {
            return trajanjePrivezaUH * 60 * 60 * 1000;
        }

        private static double pretvoriDatumVrijemeUMilisekunde(DateTime datumVrijeme)
        {
            DateTime pocetakBrojanjaMilisekundi = DateTime.UnixEpoch;
            TimeSpan vrijemeProtekloDoDatumVrijeme = datumVrijeme.Subtract(pocetakBrojanjaMilisekundi);
            return vrijemeProtekloDoDatumVrijeme.TotalMilliseconds;
        }

        private static bool provjeriRaspored(Brod brod, DateTime vrijemeOd)
        {
            string vrstaVeza = provjeriVrstuVeza(brod.vrsta);

            bool zauzet = false;
            foreach (Raspored r in RasporediController.listaZapisaRasporeda)
            {
                if (vrstaVeza.Equals("PU") && PutnickiVezovi.putnickiVezoviLista.ContainsID(r.IDVez))
                {
                    zauzet = provjeriVezNaRasporedu(r, vrijemeOd);
                }
                else if (vrstaVeza.Equals("PO") && PoslovniVezovi.poslovniVezoviLista.ContainsID(r.IDVez))
                {
                    zauzet = provjeriVezNaRasporedu(r, vrijemeOd);
                }
                else if (vrstaVeza.Equals("OS") && OstaliVezovi.ostaliVezoviLista.ContainsID(r.IDVez))
                {
                    zauzet = provjeriVezNaRasporedu(r, vrijemeOd);
                }

                if (zauzet) return true;
            }

            return zauzet;
        }

        private static bool provjeriVezNaRasporedu(Raspored r, DateTime vrijemeOd)
        {
            if (vezJeZauzetUVremenskomRasponu(r, vrijemeOd))
            {
                if (r.daniUTjednu.Contains((int)vrijemeOd.DayOfWeek)) return true;
            }

            return false;
        }

        private static bool vezJeZauzetUVremenskomRasponu(Raspored r, DateTime vrijemeOd)
        {
            if (r.vrijemeOd.Hour < vrijemeOd.Hour &&
                r.vrijemeDo.Hour > vrijemeOd.Hour
                ||
                r.vrijemeOd.Hour == vrijemeOd.Hour &&
                r.vrijemeOd.Minute <= vrijemeOd.Minute
                ||
                r.vrijemeDo.Hour == vrijemeOd.Hour &&
                r.vrijemeDo.Minute >= vrijemeOd.Minute)
            {
                return true;
            }
            return false;
        }

        private static string provjeriVrstuVeza(string vrsta)
        {
            string vrstaVeza = "";
            if (vrsta.Equals("TR") || vrsta.Equals("KA") ||
                    vrsta.Equals("KL") || vrsta.Equals("KR"))
            {
                vrstaVeza = "PU";
            }
            else if (vrsta.Equals("RI") || vrsta.Equals("TE"))
            {
                vrstaVeza = "PO";
            }
            else if (vrsta.Equals("JA") || vrsta.Equals("BR") || vrsta.Equals("RO"))
            {
                vrstaVeza = "OS";
            }
            return vrstaVeza;
        }

        private static int postaviTrajanjeUH(string stringTrajanje)
        {
            return int.TryParse(stringTrajanje, out int trajanje)
                 ? trajanje
                 : throw new Exception("Trajanje u satima nije cijeli broj.");
        }

        private static int postaviIdBroda(string stringID)
        {
            return int.TryParse(stringID, out int id)
                 ? id
                 : throw new Exception("ID nije cijeli broj.");
        }
    }
}
