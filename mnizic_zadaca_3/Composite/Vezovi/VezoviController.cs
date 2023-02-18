using mnizic_zadaca_3.Composite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.Composite.Vezovi
{
    public class VezoviController : IBrodskaLuka, IVezovi
    {
        public int ID { get; set; }
        public string oznakaVeza { get; set; }
        public string vrsta { get; set; }
        public double cijenaVezaPoSatu { get; set; }
        public double maksimalnaDuljina { get; set; }
        public double maksimalnaSirina { get; set; }
        public double maksimalnaDubina { get; set; }

        public void provjeriBrojDohvacenihVrijednosti(string[] dohvaceneVrijednosti)
        {
            if (dohvaceneVrijednosti.Length != 7) throw new Exception("Netocan broj atributa.");
        }

        public int postaviIDVeza(string stringID)
        {
            return int.TryParse(stringID, out int number)
                 ? number
                 : throw new Exception("ID nije cijeli broj.");
        }

        public string postaviOznakuVeza(string oznakaVeza)
        {
            return Regex.Match(oznakaVeza, @"^(\w){5}$").Success == true
                 ? oznakaVeza
                 : throw new Exception("Oznaka veza nije ispravna.");
        }

        public string postaviVrstuVeza(string vrsta)
        {
            return Regex.Match(vrsta, @"^[A-Z]{2}$").Success == true
                 ? vrsta
                 : throw new Exception("Vrsta veza neispravna.");
        }

        public double postaviCijenuVezaPoSatu(string stringCijenaVezaPoSatu)
        {
            return double.TryParse(stringCijenaVezaPoSatu, out double dohvacenaCijenaVezaPoSatu)
                 ? dohvacenaCijenaVezaPoSatu
                 : throw new Exception("Cijena nije broj.");
        }

        public double postaviMaksimalnuDuljinu(string stringMaksimalnaDuljina)
        {
            return double.TryParse(stringMaksimalnaDuljina, out double dohvacenaMaksimalnaDuljina)
                 ? dohvacenaMaksimalnaDuljina
                 : throw new Exception("Maksimalna duljina nije broj.");
        }

        public double postaviMaksimalnuSirinu(string stringMaksimalnaSirina)
        {
            return double.TryParse(stringMaksimalnaSirina, out double dohvacenaMaksimalnaSirina)
                 ? dohvacenaMaksimalnaSirina
                 : throw new Exception("Maksimalna sirina nije broj.");
        }

        public double postaviMaksimalnuDubinu(string stringMaksimalnaDubina)
        {
            return double.TryParse(stringMaksimalnaDubina, out double dohvacenaMaksimalnaDubina)
                 ? dohvacenaMaksimalnaDubina
                 : throw new Exception("Maksimalna dubina nije broj.");
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

        public virtual VezoviController provjeriIspravnostZapisaVeza(string[] dohvaceneVrijednosti)
        {
            throw new NotImplementedException();
        }
    }
}
