using mnizic_zadaca_3.FactoryMethod.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.FactoryMethod.Creator
{
    internal class CitacRezervacijaConcrete : CitacCreator
    {
        protected override Citac GetCitac(string nazivDatoteke)
        {
            Citac citacRezervacijaProduct = new CitacRezervacijaProduct(nazivDatoteke);
            return citacRezervacijaProduct;
        }
    }
}
