using Microsoft.VisualBasic;
using mnizic_zadaca_3.ChainOfResponsibility;
using mnizic_zadaca_3.Composite;
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
    public class KomandaZDController
    {
        private static Rezervacija rezervacija = new();
        public static void provediZahtjev(string komanda)
        {
            Chain dnevnik = DnevnikZapisi.doChaining();
            string[] splitKomande = komanda.Split(" ");
            bool valjanZahtjev = false;
            try
            {
                provjeriIspravnostKomandeZD(komanda, splitKomande);
                int idBroda = postaviIdBroda(splitKomande[1]);
                provjeriKanalBroda(idBroda);
                List<Rezervacija> sveRezervacijeDanogBroda = RezervacijeController.listaRezervacija
                    .FindAll(x => x.IDBroda == idBroda);

                valjanZahtjev = provjeriVrijemeZahtjeva(sveRezervacijeDanogBroda);
                DateTime vrijemeDo = valjanZahtjev == true ? rezervacija.datumVrijemeOd : new();
                string ispis = valjanZahtjev == true ? $"Brod s ID {idBroda} - Privez broda (REZERVIRAN VEZ)\n" +
                    $"\tVrijeme: {rezervacija.datumVrijemeOd} - {vrijemeDo.AddHours(rezervacija.trajanjePrivezaUH)}"
                            : $"Brod s ID {idBroda} nema rezervaciju";

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

        private static void provjeriIspravnostKomandeZD(string komanda, string[] splitKomande)
        {
            if (splitKomande.Length != 2) throw new Exception($"Zahtjev {komanda} nije ispravan.");
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

        private static bool provjeriVrijemeZahtjeva(List<Rezervacija>? listaRezervacija)
        {
            bool zahtjevUnutarVremenskogRaspona = false;
            rezervacija = null;
            if (listaRezervacija != null)
            {
                foreach (Rezervacija r in listaRezervacija)
                {
                    long trenutnoVrijemeMillis =
                    (long)pretvoriDatumVrijemeUMilisekunde(VirtualnoVrijemeSingleton.InstancaVirtualnoVrijeme.virtualnoVrijeme);
                    long vrijemePocetkaMillis = (long)pretvoriDatumVrijemeUMilisekunde(r.datumVrijemeOd);
                    long vrijemeKrajaMillis = vrijemePocetkaMillis +
                        pretvoriSateUMilisekunde(r.trajanjePrivezaUH);

                    zahtjevUnutarVremenskogRaspona =
                        virtualnoVrijemeUnutarRaspona(trenutnoVrijemeMillis, vrijemePocetkaMillis, vrijemeKrajaMillis);
                    if (zahtjevUnutarVremenskogRaspona)
                    {
                        rezervacija = r;
                        return true;
                    }
                }

            }
            return zahtjevUnutarVremenskogRaspona;
        }

        private static bool virtualnoVrijemeUnutarRaspona(long trenutnoVrijemeMillis,
            long vrijemePocetkaMillis, long vrijemeKrajaMillis)
        {
            if (trenutnoVrijemeMillis >= vrijemePocetkaMillis &&
                trenutnoVrijemeMillis <= vrijemeKrajaMillis) return true;
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

        private static int postaviIdBroda(string stringID)
        {
            return int.TryParse(stringID, out int id)
                 ? id
                 : throw new Exception("ID nije cijeli broj.");
        }
    }
}
