using mnizic_zadaca_3.Composite;
using mnizic_zadaca_3.MVC.Views;
using mnizic_zadaca_3.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.FactoryMethod.Product
{
    public class CitacLukaProduct : Citac
    {
        public string nazivDatoteke { get; set; }

        public CitacLukaProduct(string _nazivDatoteke)
        {
            this.nazivDatoteke = _nazivDatoteke;
        }

        public void DohvatiPodatke()
        {
            using StreamReader reader = new StreamReader(nazivDatoteke);
            try
            {
                iscitajLukuIzDatoteke(reader);
            }
            catch (Exception ex)
            {
                PodaciView.ispisGreske(++BrojacGresakaSingleton.InstancaBrojacGresaka.brojGreske, ex.Message);
            }
        }

        private void iscitajLukuIzDatoteke(StreamReader reader)
        {
            string preskociPrvuLiniju = reader.ReadLine();
            string linijaUDatoteci;
            while ((linijaUDatoteci = reader.ReadLine()) != null)
            {
                LukeController.DohvatiLuke(linijaUDatoteci);
            }
            PotrebniArgumentiSingleton.InstancaPotrebniArgumenti.L = true;
        }
    }
}
