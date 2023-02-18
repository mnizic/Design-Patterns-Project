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
    public class CitacKanalaProduct : Citac
    {
        public string nazivDatoteke { get; set; }

        public CitacKanalaProduct(string _nazivDatoteke)
        {
            this.nazivDatoteke = _nazivDatoteke;
        }

        public void DohvatiPodatke()
        {
            using StreamReader reader = new StreamReader(nazivDatoteke);
            try
            {
                iscitajKanaleIzDatoteke(reader);
            }
            catch (Exception ex)
            {
                PodaciView.ispisGreske(++BrojacGresakaSingleton.InstancaBrojacGresaka.brojGreske, ex.Message);
            }
        }

        private void iscitajKanaleIzDatoteke(StreamReader reader)
        {
            string preskociPrvuLiniju = reader.ReadLine();
            string linijaUDatoteci;
            while ((linijaUDatoteci = reader.ReadLine()) != null)
            {
                try
                {
                    KanaliController.DohvatiKanale(linijaUDatoteci);
                }
                catch (Exception ex)
                {
                    PodaciView.ispisGreske(++BrojacGresakaSingleton.InstancaBrojacGresaka.brojGreske, ex.Message);
                }   

            }
        }
    }
}
