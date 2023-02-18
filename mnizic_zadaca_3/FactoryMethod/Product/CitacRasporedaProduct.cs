using mnizic_zadaca_3.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mnizic_zadaca_3.MVC.Controllers.PodaciController;
using mnizic_zadaca_3.MVC.Views;

namespace mnizic_zadaca_3.FactoryMethod.Product
{
    public class CitacRasporedaProduct : Citac
    {
        public string nazivDatoteke { get; set; }
        public CitacRasporedaProduct(string _nazivDatoteke)
        {
            this.nazivDatoteke = _nazivDatoteke;
        }
        public void DohvatiPodatke()
        {
            using StreamReader reader = new StreamReader(nazivDatoteke);
            try
            {
                iscitajRasporedIzDatoteke(reader);
            }
            catch (Exception ex)
            {
                PodaciView.ispisGreske(++BrojacGresakaSingleton.InstancaBrojacGresaka.brojGreske, ex.Message);
            }
        }

        private void iscitajRasporedIzDatoteke(StreamReader reader)
        {
            string preskociPrvuLiniju = reader.ReadLine();
            string linijaUDatoteci;
            while ((linijaUDatoteci = reader.ReadLine()) != null)
            {
                try
                {
                    RasporediController.DohvatiRaspored(linijaUDatoteci);
                }
                catch (Exception ex)
                {
                    PodaciView.ispisGreske(++BrojacGresakaSingleton.InstancaBrojacGresaka.brojGreske, ex.Message);
                }

            }
        }
    }
}
