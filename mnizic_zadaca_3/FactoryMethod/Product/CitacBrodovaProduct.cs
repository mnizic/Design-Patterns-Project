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
    public class CitacBrodovaProduct : Citac
    {
        public string nazivDatoteke { get; set; }

        private List<string> oznakeBrodova { get; set; } = new List<string>()
        {
            "TR", "KA", "KL", "KR", "RI", "TE", "JA", "BR", "RO"
        };

        public CitacBrodovaProduct(string _nazivDatoteke)
        {
            this.nazivDatoteke = _nazivDatoteke;
        }

        public void DohvatiPodatke()
        {
            using StreamReader reader = new StreamReader(nazivDatoteke);
            try
            {
                iscitajBrodoveIzDatoteke(reader);
            }
            catch (Exception ex)
            {
                PodaciView.ispisGreske(++BrojacGresakaSingleton.InstancaBrojacGresaka.brojGreske, ex.Message);
            }
        }

        private void iscitajBrodoveIzDatoteke(StreamReader reader)
        {
            string preskociPrvuLiniju = reader.ReadLine();
            string linijaUDatoteci;
            while ((linijaUDatoteci = reader.ReadLine()) != null)
            {
                try
                {
                    provjeriOznakuBroda(linijaUDatoteci);
                }
                catch (Exception ex)
                {
                    PodaciView.ispisGreske(++BrojacGresakaSingleton.InstancaBrojacGresaka.brojGreske, ex.Message);
                }
            }
            PotrebniArgumentiSingleton.InstancaPotrebniArgumenti.B = true;
        }

        private void provjeriOznakuBroda(string linijaUDatoteci)
        {
            if (oznakeBrodova.Any(linijaUDatoteci.Contains))
            {
                BrodoviController.DohvatiBrodove(linijaUDatoteci);
            }
            else
            {
                throw new Exception("Brod sadrzi krivu oznaku vrste.");
            }
        }
    }
}
