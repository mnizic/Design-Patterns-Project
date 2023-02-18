using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.Composite.Vezovi
{
    public interface IVezovi
    {
        int ID { get; set; }
        string oznakaVeza { get; set; }
        string vrsta { get; set; }
        double cijenaVezaPoSatu { get; set; }
        double maksimalnaDuljina { get; set; }
        double maksimalnaSirina { get; set; }
        double maksimalnaDubina { get; set; }

        void provjeriBrojDohvacenihVrijednosti(string[] dohvaceneVrijednosti);

        VezoviController provjeriIspravnostZapisaVeza(string[] dohvaceneVrijednosti);

        int postaviIDVeza(string stringID);

        string postaviOznakuVeza(string oznakaVeza);

        string postaviVrstuVeza(string vrsta);

        double postaviCijenuVezaPoSatu(string stringCijenaVezaPoSatu);

        double postaviMaksimalnuDuljinu(string stringMaksimalnaDuljina);

        double postaviMaksimalnuSirinu(string stringMaksimalnaSirina);

        double postaviMaksimalnuDubinu(string stringMaksimalnaDubina);
    }
}
