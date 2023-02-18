using mnizic_zadaca_3.FactoryMethod.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.FactoryMethod.Creator
{
    public abstract class CitacCreator
    {
        protected abstract Citac GetCitac(string nazivDatoteke);

        public Citac FactoryMethod(string nazivDatoteke)
        {
            return this.GetCitac(nazivDatoteke);
        }
    }
}
