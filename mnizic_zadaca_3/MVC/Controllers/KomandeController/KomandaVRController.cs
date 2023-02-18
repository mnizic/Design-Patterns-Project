using mnizic_zadaca_3.MVC.Views;
using mnizic_zadaca_3.Singleton;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.MVC.Controllers.KomandeController
{
    public class KomandaVRController
    {
        public static void postaviNovoVirtualnoVrijeme(string komanda)
        {
            try
            {
                String[] splitKomande = komanda.Split(" ");
                provjeriIspravnostKomandeVR(splitKomande);
                String datum = (splitKomande[1] + " " + splitKomande[2]).Trim();
                VirtualnoVrijemeSingleton.InstancaVirtualnoVrijeme.virtualnoVrijeme = DateTime.TryParseExact(datum, "dd.MM.yyyy. HH:mm:ss",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime noviDatum) ? noviDatum
                : throw new Exception("Neispravan datum.");

                KomandeView.ispisiOdgovor($"Potvrđeno novo virutalno vrijeme: " +
                    $"{VirtualnoVrijemeSingleton.InstancaVirtualnoVrijeme.virtualnoVrijeme.ToString("dd.MM.yyyy. HH:mm:ss")}");

            }
            catch (Exception ex)
            {
                KomandeView.ispisiOdgovor(ex.Message);
            }
        }

        private static void provjeriIspravnostKomandeVR(string[] splitKomande)
        {
            if (splitKomande.Length != 3) throw new Exception($"Komanda {splitKomande[0]} je neispravna.");
        }
    }
}
