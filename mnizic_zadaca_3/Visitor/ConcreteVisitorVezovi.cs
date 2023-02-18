using mnizic_zadaca_3.MVC.Views;
using mnizic_zadaca_3.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.Visitor
{
    public class ConcreteVisitorVezovi : IVisitor
    {
        private int redniBroj = 0;
        public void Visit(ConcreteComponentVezoviPU element)
        {

            if (KraticeZaIspisSingleton.InstancaKraticeZaIspis.RedniBrojevi)
            {
                KomandeView.ispisiOdgovor(String.Format("|{0,15}|{1,-15}|{2,-15}|{3,15}|", 
                    ++redniBroj, "PU", "ZAUZETI", element.dohvatiZbroj()));
            } else
            {
                ++redniBroj;
                KomandeView.ispisiOdgovor(String.Format("|{0,-15}|{1,-15}|{2,15}|", "PU", "ZAUZETI", element.dohvatiZbroj()));
            }
        }

        public void Visit(ConcreteComponentVezoviPO element)
        {
            if (KraticeZaIspisSingleton.InstancaKraticeZaIspis.RedniBrojevi)
            {
                KomandeView.ispisiOdgovor(String.Format("|{0,15}|{1,-15}|{2,-15}|{3,15}|",
                    ++redniBroj, "PO", "ZAUZETI", element.dohvatiZbroj()));
            }
            else
            {
                ++redniBroj;
                KomandeView.ispisiOdgovor(String.Format("|{0,-15}|{1,-15}|{2,15}|", "PO", "ZAUZETI", element.dohvatiZbroj()));
            }
        }

        public void Visit(ConcreteComponentVezoviOS element)
        {
            if (KraticeZaIspisSingleton.InstancaKraticeZaIspis.RedniBrojevi)
            {
                KomandeView.ispisiOdgovor(String.Format("|{0,15}|{1,-15}|{2,-15}|{3,15}|",
                    ++redniBroj, "OS", "ZAUZETI", element.dohvatiZbroj()));
            }
            else
            {
                ++redniBroj;
                KomandeView.ispisiOdgovor(String.Format("|{0,-15}|{1,-15}|{2,15}|", "OS", "ZAUZETI", element.dohvatiZbroj()));
            }
        }

        public int dohvatiBrojZapisa()
        {
            return redniBroj;
        }
    }
}
