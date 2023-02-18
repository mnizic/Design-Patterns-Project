using mnizic_zadaca_3.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.MVC.Views
{
    public class KomandeView
    {
        public static List<string> listaOdgovora = new();
        public static void ispisiOdgovor(string odgovor)
        {
            listaOdgovora.Add(odgovor);
        }

        public static void ispisiZaglavlje(string komanda)
        {
            if (komanda.Equals("I"))
            {
                ispisZaglavljeI();
            } 
            else if (komanda.Equals("V"))
            {
                ispisZaglavljeV();
            } 
            else if (komanda.Equals("ZA"))
            {
                ispisZaglavljeZA();
            } 
            else if (komanda.Equals("LOG"))
            {
                ispisZaglavljeLOG();
            }   
        }

        private static void ispisZaglavljeLOG()
        {
            if (KraticeZaIspisSingleton.InstancaKraticeZaIspis.Zaglavlje)
            {
                if (KraticeZaIspisSingleton.InstancaKraticeZaIspis.RedniBrojevi)
                {
                    ispisiOdgovor(string.Format("{0,-5} {1,35}",
                        "RB", "Zahtjevi"));
                }
                else
                {
                    ispisiOdgovor(string.Format("{0,35}",
                        "Zahtjevi"));
                }
            }
        }

        private static void ispisZaglavljeZA()
        {
            if (KraticeZaIspisSingleton.InstancaKraticeZaIspis.Zaglavlje)
            {
                if (KraticeZaIspisSingleton.InstancaKraticeZaIspis.RedniBrojevi)
                {
                    ispisiOdgovor(string.Format("|{0,-15}|{1,-15}|{2,15}|{3,15}|",
                        "RB", "Vrsta veza", "Status", "Ukupan broj"));
                }
                else
                {
                    ispisiOdgovor(string.Format("|{0,-15}|{1,-15}|{2,15}|",
                        "Vrsta veza", "Status", "Ukupan broj"));
                }
            }
        }

        private static void ispisZaglavljeV()
        {
            if (KraticeZaIspisSingleton.InstancaKraticeZaIspis.RedniBrojevi)
            {
                ispisiOdgovor(string.Format("|{0,-10}|{1,-10}|{2,-15}|{3,-10}|{4,-20}|{5,-20}|",
                            "RB", "ID veza", "Vrsta", "Status", "Vrijeme od", "Vrijeme do"));
            }
            else
            {
                ispisiOdgovor(string.Format("|{0,-10}|{1,-15}|{2,-10}|{3,-20}|{4,-20}|",
                             "ID veza", "Vrsta", "Status", "Vrijeme od", "Vrijeme do"));
            }
        }

        private static void ispisZaglavljeI()
        {
            if (KraticeZaIspisSingleton.InstancaKraticeZaIspis.Zaglavlje)
            {
                if (KraticeZaIspisSingleton.InstancaKraticeZaIspis.RedniBrojevi)
                {
                    ispisiOdgovor(string.Format("|{0,-10}|{1,-10}|{2,-10}|{3,-10}|",
                        "RB", "ID veza", "Vrsta veza", "Status"));
                }
                else
                {
                    ispisiOdgovor(string.Format("|{0,-10}|{1,-10}|{2,-10}|",
                        "ID veza", "Vrsta veza", "Status"));
                }
            }
        }

        public static void ispisiPodnozje(string komanda, int ukupnoZapisa)
        {
            if (komanda.Equals("I"))
            {
                ispisPodnozjeI(ukupnoZapisa);
            } 
            else if (komanda.Equals("V"))
            {
                ispisPodnozjeV(ukupnoZapisa);
            } 
            else if (komanda.Equals("ZA"))
            {
                ispisPodnozjeZA(ukupnoZapisa);
            } 
            else if (komanda.Equals("LOG"))
            {
                ispisPodnozjeLOG(ukupnoZapisa);
            }
        }

        private static void ispisPodnozjeLOG(int ukupnoZapisa)
        {
            if (KraticeZaIspisSingleton.InstancaKraticeZaIspis.Podnozje)
            {
                ispisiOdgovor(string
                    .Format("{0,-5} {1,-35}", "UKUPNO ZAPISA:", ukupnoZapisa));
            }
        }

        private static void ispisPodnozjeZA(int ukupnoZapisa)
        {
            if (KraticeZaIspisSingleton.InstancaKraticeZaIspis.Podnozje)
            {
                if (KraticeZaIspisSingleton.InstancaKraticeZaIspis.RedniBrojevi)
                {
                    ispisiOdgovor(string
                    .Format("|{0,-15} {1,15} {2,-15} {3,15}|", "UKUPNO ZAPISA:", "", "", ukupnoZapisa));
                }
                else
                {
                    ispisiOdgovor(string
                    .Format("|{0,-15} {1,15} {2,15}|", "UKUPNO ZAPISA:", "", ukupnoZapisa));
                }
            }
        }

        private static void ispisPodnozjeV(int ukupnoZapisa)
        {
            if (KraticeZaIspisSingleton.InstancaKraticeZaIspis.Podnozje)
            {
                if (KraticeZaIspisSingleton.InstancaKraticeZaIspis.RedniBrojevi)
                {
                    ispisiOdgovor(string
                    .Format("|{0,-10} {1,10} {2,-15} {3,-10} {4,20} {5,20}|",
                    "UKUPNO:", "", "", "", "", ukupnoZapisa));
                }
                else
                {
                    ispisiOdgovor(string
                    .Format("|{0,-10} {1,-15} {2,-10} {3,20} {4,20}|",
                    "UKUPNO:", "", "", "", ukupnoZapisa));
                }
            }
        }

        private static void ispisPodnozjeI(int ukupnoZapisa)
        {
            if (KraticeZaIspisSingleton.InstancaKraticeZaIspis.Podnozje)
            {
                if (KraticeZaIspisSingleton.InstancaKraticeZaIspis.RedniBrojevi)
                {
                    ispisiOdgovor(string
                    .Format("|{0,-10 } {1,10} {2,-10} {3,10}|", "UKUPNO:", "", "", ukupnoZapisa));
                }
                else
                {
                    ispisiOdgovor(string
                    .Format("|{0,-10} {1,10} {2,10}|", "UKUPNO:", "", ukupnoZapisa));
                }
            }
        }

        public static void ispisiDnevnik()
        {
            int redniBroj = 0;
            ispisiZaglavlje("LOG");

            if (SviZapisiDnevnikaSingleton.InstancaSviZapisiDnevnika.sviZapisiDnevnik.Count != 0)
            {
                SviZapisiDnevnikaSingleton.InstancaSviZapisiDnevnika.sviZapisiDnevnik.ForEach(d =>
                {
                    ++redniBroj;
                    if (KraticeZaIspisSingleton.InstancaKraticeZaIspis.RedniBrojevi)
                    {
                        ispisiOdgovor(string.Format("{0,-5} {1,35} ", redniBroj, d.zapis));
                    }
                    else
                    {
                        ispisiOdgovor(string.Format("{0,35}", d.zapis));
                    }
                });
            }
            else
            {
                ispisiOdgovor(string.Format("{0,45}", "Dnevnik je prazan"));
            }

            ispisiPodnozje("LOG", redniBroj);
        }
    }
}
