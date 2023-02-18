using Microsoft.VisualBasic;
using mnizic_zadaca_3.MVC.Models;
using mnizic_zadaca_3.MVC.Views;
using mnizic_zadaca_3.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.MVC.Controllers.PodaciController
{
    public class KanaliController
    {
        public static List<Kanal> listaKanala = new();

        public static void DohvatiKanale(string linijaUDatoteci)
        {
            string[] dohvaceneVrijednosti = linijaUDatoteci.Split(';');
            try
            {
                provjeriBrojDohvacenihVrijednosti(dohvaceneVrijednosti);
                Kanal kanal = provjeriKanal(dohvaceneVrijednosti);
                provjeriDuplikat(kanal);
                listaKanala.Add(kanal);
            }
            catch (Exception ex)
            {
                PodaciView.ispisGreske(++BrojacGresakaSingleton.InstancaBrojacGresaka.brojGreske, ex.Message);
            }
        }

        private static void provjeriDuplikat(Kanal kanal)
        {
            if (listaKanala.Any(x => x.ID == kanal.ID)) throw new Exception($"Kanal sa ID-om {kanal.ID} vec postoji.");
            if (listaKanala.Any(x => x.frekvencija == kanal.frekvencija))
                throw new Exception($"Kanal sa frekvencijom {kanal.frekvencija} vec postoji.");
        }

        private static void provjeriBrojDohvacenihVrijednosti(string[] dohvaceneVrijednosti)
        {
            if (dohvaceneVrijednosti.Length != 3) throw new Exception("Netocan broj atributa.");
        }

        private static Kanal provjeriKanal(string[] vrijednosti)
        {
            return new()
            {
                ID = postaviID(vrijednosti[0]),
                frekvencija = postaviFrekvenciju(vrijednosti[1]),
                maksimalanBroj = postaviMaksimalanBroj(vrijednosti[2])
            };

        }
        private static int postaviID(string stringID)
        {
            return int.TryParse(stringID, out int id)
                 ? id
                 : throw new Exception("ID nije cijeli broj.");
        }

        private static int postaviFrekvenciju(string stringFrekvencija)
        {
            return int.TryParse(stringFrekvencija, out int frekvencija)
                 ? frekvencija
                 : throw new Exception("Frekvencija nije cijeli broj.");
        }

        private static int postaviMaksimalanBroj(string stringMaksimalanBroj)
        {
            return int.TryParse(stringMaksimalanBroj, out int maksimalanBroj)
                 ? maksimalanBroj
                 : throw new Exception("Maksimalan broj nije cijeli broj.");
        }
    }
}
