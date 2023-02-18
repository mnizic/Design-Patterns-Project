using mnizic_zadaca_3.MVC.Views;
using mnizic_zadaca_3.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.MVC.Controllers.KomandeController
{
    public class KomandaTController
    {
        public static void urediIspis(string komanda)
        {
            resetirajSve();
            string[] splitKomande = komanda.Split(" ");

            if (splitKomande.Length > 1)
            {
                try
                {
                    provjeriIspravnostKomandeT(splitKomande);

                    for (int i = 1; i < splitKomande.Length; i++)
                    {
                        if (splitKomande[i].Equals("Z")) KraticeZaIspisSingleton.InstancaKraticeZaIspis.Zaglavlje = true;
                        if (splitKomande[i].Equals("P")) KraticeZaIspisSingleton.InstancaKraticeZaIspis.Podnozje = true;
                        if (splitKomande[i].Equals("RB")) KraticeZaIspisSingleton.InstancaKraticeZaIspis.RedniBrojevi = true;
                    }
                }
                catch (Exception ex)
                {
                    KomandeView.ispisiOdgovor(ex.Message);
                }
            }
        }

        private static void resetirajSve()
        {
            KraticeZaIspisSingleton.InstancaKraticeZaIspis.Zaglavlje = false;
            KraticeZaIspisSingleton.InstancaKraticeZaIspis.Podnozje = false;
            KraticeZaIspisSingleton.InstancaKraticeZaIspis.RedniBrojevi = false;
        }

        private static void provjeriIspravnostKomandeT(string[] splitKomande)
        {
            if (splitKomande.Length > 4) throw new Exception($"Komanda {splitKomande[0]} sadrži više od 3 parametra.");

            for (int i = 1; i < splitKomande.Length; i++)
            {
                if (!(splitKomande[i].Equals("RB") || splitKomande[i].Equals("Z") || splitKomande[i].Equals("P")))
                {
                    throw new Exception($"Komanda {splitKomande[0]} sadrzi nepoznati atribut.");
                }
            }
        }
    }
}
