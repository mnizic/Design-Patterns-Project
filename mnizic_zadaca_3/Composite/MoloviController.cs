using mnizic_zadaca_3.IteratorPattern.Collections;
using mnizic_zadaca_3.MVC.Models;
using mnizic_zadaca_3.MVC.Views;
using mnizic_zadaca_3.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.Composite
{
    public class MoloviController : IBrodskaLuka
    {
        public static MoloviCollection listaMolova = new();

        public static void DohvatiMolove(string linijaUDatoteci)
        {
            string[] dohvaceneVrijednosti = linijaUDatoteci.Split(';');
            try
            {
                provjeriBrojDohvacenihVrijednosti(dohvaceneVrijednosti);
                Mol mol = provjeriMol(dohvaceneVrijednosti);
                provjeriDuplikat(mol);
                listaMolova.DodajMol(mol);
            }
            catch (Exception ex)
            {
                PodaciView.ispisGreske(++BrojacGresakaSingleton.InstancaBrojacGresaka.brojGreske, ex.Message);
            }
        }

        private static void provjeriDuplikat(Mol mol)
        {
            if (listaMolova.Any(mol)) throw new Exception($"Mol sa ID-om {mol.ID} vec postoji.");
        }

        private static void provjeriBrojDohvacenihVrijednosti(string[] dohvaceneVrijednosti)
        {
            if (dohvaceneVrijednosti.Length != 2) throw new Exception("Netocan broj atributa.");
        }

        private static Mol provjeriMol(string[] vrijednosti)
        {
            return new()
            {
                ID = postaviID(vrijednosti[0]),
                naziv = postaviNaziv(vrijednosti[1])
            };

        }

        private static int postaviID(string stringID)
        {
            return int.TryParse(stringID, out int id)
                 ? id
                 : throw new Exception("ID nije cijeli broj.");
        }

        private static string postaviNaziv(string stringNaziv)
        {
            return Regex.Match(stringNaziv, @"^[A-ZČŠĆĐŽa-zčšćđž]+$").Success == true
                 ? stringNaziv
                 : throw new Exception("Naziv mola neispravan.");
        }

        public void dodaj(IBrodskaLuka segmentLuke)
        {

        }

        public void ukloni(IBrodskaLuka segmentLuke)
        {

        }

        public IBrodskaLuka dohvatiDijete(int i)
        {
            return null;
        }
    }
}
