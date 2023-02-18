
using mnizic_zadaca_3.Composite.Vezovi;
using mnizic_zadaca_3.MVC.Views;
using mnizic_zadaca_3.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.FactoryMethod.Product
{
    public class CitacVezovaProduct : Citac
    {
        public string nazivDatoteke { get; set; }

        public CitacVezovaProduct(string _nazivDatoteke)
        {
            this.nazivDatoteke = _nazivDatoteke;
        }

        public void DohvatiPodatke()
        {
            using StreamReader reader = new StreamReader(nazivDatoteke);
            try
            {
                iscitajVezoveIzDatoteke(reader);
            }
            catch (Exception ex)
            {
                PodaciView.ispisGreske(++BrojacGresakaSingleton.InstancaBrojacGresaka.brojGreske, ex.Message);
            }
        }

        private void iscitajVezoveIzDatoteke(StreamReader reader)
        {
            string preskociPrvuLiniju = reader.ReadLine();
            string linijaUDatoteci;
            while ((linijaUDatoteci = reader.ReadLine()) != null)
            {
                try
                {
                    provjeriOznakuVeza(linijaUDatoteci);
                }
                catch (Exception ex)
                {
                    PodaciView.ispisGreske(++BrojacGresakaSingleton.InstancaBrojacGresaka.brojGreske, ex.Message);
                }

            }
            PotrebniArgumentiSingleton.InstancaPotrebniArgumenti.V = true;
        }

        private void provjeriOznakuVeza(string linijaUDatoteci)
        {
            if (linijaUDatoteci.Contains("PU"))
            {
                PutnickiVezovi.DohvatiVez(linijaUDatoteci);
            }
            else if (linijaUDatoteci.Contains("PO"))
            {
                PoslovniVezovi.DohvatiVez(linijaUDatoteci);
            }
            else if (linijaUDatoteci.Contains("OS"))
            {
                OstaliVezovi.DohvatiVez(linijaUDatoteci);
            }
            else
            {
                throw new Exception("Vez sadrzi krivu vrstu.");
            }
        }
    }
}
