using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.Composite
{
    public interface IBrodskaLuka
    {
        public void dodaj(IBrodskaLuka segmentLuke);
        public void ukloni(IBrodskaLuka segmentLuke);
        public IBrodskaLuka dohvatiDijete(int i);
    }
}
