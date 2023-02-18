using mnizic_zadaca_3.FactoryMethod.Creator;
using mnizic_zadaca_3.FactoryMethod.Product;
using mnizic_zadaca_3.MVC.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.MVC.Controllers.KomandeController
{
    public class KomandaURController
    {
        public static void dohvatiRezervacije(string komanda)
        {
            string[] splitKomande = komanda.Split(" ");
            string nazivDatoteke = splitKomande[1];
            try
            {
                provjeriIspravnostKomandeUR(splitKomande);
            }
            catch (Exception ex)
            {
                KomandeView.ispisiOdgovor(ex.Message);
            }


            Citac c = new CitacRezervacijaConcrete().FactoryMethod(nazivDatoteke);
            c.DohvatiPodatke();
        }

        private static void provjeriIspravnostKomandeUR(string[] splitKomande)
        {
            if (splitKomande.Length != 2) throw new Exception($"Komanda {splitKomande[0]} je neispravna.");
        }
    }
}
