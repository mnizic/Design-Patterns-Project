using mnizic_zadaca_3.Composite.Vezovi;
using mnizic_zadaca_3.IteratorPattern;
using mnizic_zadaca_3.Singleton;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mnizic_zadaca_3.MVC.Controllers.KomandeController;
using mnizic_zadaca_3.MVC.Models;
using mnizic_zadaca_3.MVC.Views;

namespace mnizic_zadaca_3.MVC.Controllers.PodaciController
{
    public class RezervacijeController
    {
        public static List<Rezervacija> listaRezervacija { get; set; } = new();

        public static void DohvatiRezervacije(string linijaUDatoteci)
        {
            string[] dohvaceneVrijednosti = linijaUDatoteci.Split(';');

            try
            {
                provjeriBrojDohvacenihVrijednosti(dohvaceneVrijednosti);
                Rezervacija rezervacija = provjeriIspravnostZapisaRezervacije(dohvaceneVrijednosti);
                if (provjeriRezervaciju(rezervacija))
                {
                    provjeriDuplikat(rezervacija);
                    listaRezervacija.Add(rezervacija);
                }
            }
            catch (Exception ex)
            {
                PodaciView.ispisGreske(++BrojacGresakaSingleton.InstancaBrojacGresaka.brojGreske, ex.Message);
            }
        }

        private static bool provjeriRezervaciju(Rezervacija rezervacija)
        {
            Brod brod = new();

            try
            {
                brod = BrodoviController.brodoviLista.Find(x => x.ID == rezervacija.IDBroda);
            }
            catch (Exception e)
            {
                PodaciView.ispisGreske(++BrojacGresakaSingleton.InstancaBrojacGresaka.brojGreske, "Brod s liste rezervacija ne postoji");
            }

            if (brod != null)
            {
                return rezervacijaJeBezPogreske(brod, rezervacija);
            }

            return false;
        }

        private static bool rezervacijaJeBezPogreske(Brod brod, Rezervacija rezervacija)
        {
            bool ispravno = true;

            if (brodJePutnicki(brod))
            {
                for (Iterator iter = (Iterator)PutnickiVezovi.putnickiVezoviLista.GetEnumerator(); iter.MoveNext();)
                {
                    PutnickiVezovi pu = (PutnickiVezovi)iter.Current();
                    if (!provjeriStatusVeza(pu, rezervacija)) ispravno = false;
                }
            }
            else if (brodJePoslovni(brod))
            {
                for (Iterator iter = (Iterator)PoslovniVezovi.poslovniVezoviLista.GetEnumerator(); iter.MoveNext();)
                {
                    PoslovniVezovi po = (PoslovniVezovi)iter.Current();
                    if (!provjeriStatusVeza(po, rezervacija)) ispravno = false;
                }
            }
            else if (brodJeOstali(brod))
            {
                for (Iterator iter = (Iterator)OstaliVezovi.ostaliVezoviLista.GetEnumerator(); iter.MoveNext();)
                {
                    OstaliVezovi os = (OstaliVezovi)iter.Current();
                    if (!provjeriStatusVeza(os, rezervacija)) ispravno = false;
                }
            }

            return ispravno;
        }

        private static bool brodJeOstali(Brod brod)
        {
            if (brod.vrsta.Equals("JA") || brod.vrsta.Equals("BR") || brod.vrsta.Equals("RO")) return true;
            return false;
        }

        private static bool brodJePoslovni(Brod brod)
        {
            if (brod.vrsta.Equals("RI") || brod.vrsta.Equals("TE")) return true;
            return false;
        }

        private static bool brodJePutnicki(Brod brod)
        {
            if (brod.vrsta.Equals("TR") || brod.vrsta.Equals("KA") ||
                brod.vrsta.Equals("KL") || brod.vrsta.Equals("KR")) return true;
            return false;
        }

        private static bool provjeriStatusVeza(PutnickiVezovi pv, Rezervacija rezervacija)
        {
            bool slobodan = false;

            if (!RasporediController.listaZapisaRasporeda.Any(x => x.IDVez == pv.ID))
            {
                slobodan = true;
            }
            else
            {
                Raspored rasporedRezervacija = konvertirajRezervacijuURaspored(rezervacija, pv.ID);
                RasporediController.listaZapisaRasporeda.ForEach(r =>
                {
                    if (r.daniUTjednu.Contains(rasporedRezervacija.daniUTjednu[0]) &&
                        r.IDVez == rasporedRezervacija.IDVez)
                    {
                        if (!KomandaVController.vezJeZauzetUVremenskomRasponu(r,
                            rasporedRezervacija.vrijemeOd, rasporedRezervacija.vrijemeDo))
                        {
                            slobodan = true;
                        }
                    }
                });
            }

            return slobodan;
        }

        private static bool provjeriStatusVeza(PoslovniVezovi pv, Rezervacija rezervacija)
        {
            bool slobodan = false;

            if (!RasporediController.listaZapisaRasporeda.Any(x => x.IDVez == pv.ID))
            {
                slobodan = true;
            }
            else
            {
                Raspored rasporedRezervacija = konvertirajRezervacijuURaspored(rezervacija, pv.ID);
                RasporediController.listaZapisaRasporeda.ForEach(delegate (Raspored r)
                {
                    if (r.daniUTjednu.Contains(rasporedRezervacija.daniUTjednu[0]) &&
                        r.IDVez == rasporedRezervacija.IDVez)
                    {
                        if (!KomandaVController.vezJeZauzetUVremenskomRasponu(r,
                            rasporedRezervacija.vrijemeOd, rasporedRezervacija.vrijemeDo))
                        {
                            slobodan = true;
                        }
                    }
                });
            }

            return slobodan;
        }

        private static bool provjeriStatusVeza(OstaliVezovi ov, Rezervacija rezervacija)
        {
            bool slobodan = false;

            if (!RasporediController.listaZapisaRasporeda.Any(x => x.IDVez == ov.ID))
            {
                slobodan = true;
            }
            else
            {
                Raspored rasporedRezervacija = konvertirajRezervacijuURaspored(rezervacija, ov.ID);
                RasporediController.listaZapisaRasporeda.ForEach(delegate (Raspored r)
                {
                    if (r.daniUTjednu.Contains(rasporedRezervacija.daniUTjednu[0]) &&
                        r.IDVez == rasporedRezervacija.IDVez)
                    {
                        if (!KomandaVController.vezJeZauzetUVremenskomRasponu(r,
                            rasporedRezervacija.vrijemeOd, rasporedRezervacija.vrijemeDo))
                        {
                            slobodan = true;
                        }
                    }
                });
            }

            return slobodan;
        }

        private static Raspored konvertirajRezervacijuURaspored(Rezervacija rezervacija, int idVeza)
        {
            List<int> daniUtj = new();
            daniUtj.Add((int)rezervacija.datumVrijemeOd.DayOfWeek);
            DateTime datumVrijemeDo = rezervacija.datumVrijemeOd;
            datumVrijemeDo.AddHours(rezervacija.trajanjePrivezaUH);
            return new()
            {
                IDVez = idVeza,
                IDBrod = rezervacija.IDBroda,
                daniUTjednu = daniUtj,
                vrijemeOd = rezervacija.datumVrijemeOd,
                vrijemeDo = datumVrijemeDo
            };
        }

        private static void provjeriDuplikat(Rezervacija rezervacija)
        {
            long datumOdMillis = (long)pretvoriDatumVrijemeUMilisekunde(rezervacija.datumVrijemeOd);

            listaRezervacija.ForEach(r =>
            {
                long rDatumOdMillis = (long)pretvoriDatumVrijemeUMilisekunde(r.datumVrijemeOd);
                long rDatumDoMillis = rDatumOdMillis + pretvoriSateUMilisekunde(r.trajanjePrivezaUH);
                if (r.IDBroda == rezervacija.IDBroda && rDatumOdMillis <= datumOdMillis && datumOdMillis <= rDatumDoMillis)
                {
                    throw new Exception($"Brod {rezervacija.IDBroda} je vec rezervirao termin u " +
                        $"{rezervacija.datumVrijemeOd.ToString("dd.MM.yyyy. HH:mm:ss")}.");
                }
            });
        }

        private static double pretvoriDatumVrijemeUMilisekunde(DateTime datumVrijeme)
        {
            DateTime pocetakBrojanjaMilisekundi = DateTime.UnixEpoch;
            TimeSpan vrijemeProtekloDoDatumVrijeme = datumVrijeme.Subtract(pocetakBrojanjaMilisekundi);
            return vrijemeProtekloDoDatumVrijeme.TotalMilliseconds;
        }

        private static long pretvoriSateUMilisekunde(int trajanjePrivezaUH)
        {
            return trajanjePrivezaUH * 60 * 60 * 1000;
        }

        private static void provjeriBrojDohvacenihVrijednosti(string[] dohvaceneVrijednosti)
        {
            if (dohvaceneVrijednosti.Length != 3) throw new Exception("Netocan broj atributa.");
        }

        private static Rezervacija provjeriIspravnostZapisaRezervacije(string[] dohvaceneVrijednosti)
        {
            return new()
            {
                IDBroda = postaviIDBroda(dohvaceneVrijednosti[0]),
                datumVrijemeOd = postaviDatumVrijemeOd(dohvaceneVrijednosti[1]),
                trajanjePrivezaUH = postaviTrajanjePrivezaUSatima(dohvaceneVrijednosti[2])
            };
        }

        private static int postaviIDBroda(string stringIDBroda)
        {
            return int.TryParse(stringIDBroda, out int dohvaceniIDBroda)
                 ? dohvaceniIDBroda
                 : throw new Exception("ID broda nije cijeli broj.");
        }

        private static DateTime postaviDatumVrijemeOd(string stringDatumVrijemeOd)
        {
            return DateTime.TryParseExact(stringDatumVrijemeOd, "dd.MM.yyyy. HH:mm:ss",
                   CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime vrijemeOd)
                 ? vrijemeOd : throw new Exception("Datum i vrijeme neispravno zapisani.");
        }

        private static int postaviTrajanjePrivezaUSatima(string stringTrajanjePriveza)
        {
            return int.TryParse(stringTrajanjePriveza, out int dohvacenoTrajanjePriveza)
                 ? dohvacenoTrajanjePriveza
                 : throw new Exception("Trajanje u satima nije broj.");
        }
    }
}
