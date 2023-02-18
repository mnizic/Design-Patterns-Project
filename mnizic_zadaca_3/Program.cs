using System.Globalization;
using System.Text.RegularExpressions;
using mnizic_zadaca_3.ChainOfResponsibility;
using mnizic_zadaca_3.Composite;
using mnizic_zadaca_3.Composite.Vezovi;
using mnizic_zadaca_3.FactoryMethod.Creator;
using mnizic_zadaca_3.FactoryMethod.Product;
using mnizic_zadaca_3.MVC.Models;
using mnizic_zadaca_3.Singleton;
using mnizic_zadaca_3.MVC.Controllers.KomandeController;
using mnizic_zadaca_3.MVC.Controllers.PodaciController;
using mnizic_zadaca_3.MVC.Views;

namespace mnizic_zadaca_3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write(VT100Singleton.InstancaVT100.ANSI_ESC + "H");
            Console.Write(VT100Singleton.InstancaVT100.ANSI_ESC + "J");
            try
            {
                postaviBrVtPd(args);
                postaviGornjiDonjiDio();
                inicijalizirajArgumente(args);
            }
            catch (Exception ex)
            {
                PodaciView.ispisGreske(++BrojacGresakaSingleton.InstancaBrojacGresaka.brojGreske, ex.Message);
                Console.WriteLine(ex.Message);
                Thread.Sleep(3000);
            }
        }

        private static void postaviGornjiDonjiDio()
        {
            VT100Singleton.InstancaVT100.GornjiDio = (int)Math.Round(VT100Singleton.InstancaVT100.Br *
                    (double.Parse(VT100Singleton.InstancaVT100.Vt.Split(":")[0]) / 100));
            VT100Singleton.InstancaVT100.DonjiDio = (int)Math.Round(VT100Singleton.InstancaVT100.Br *
                (double.Parse(VT100Singleton.InstancaVT100.Vt.Split(":")[1]) / 100));
        }

        private static void ispisiGreske()
        {
            if (VT100Singleton.InstancaVT100.Pd.Split(":")[0] == "P")
            {
                for(int i = PodaciView.listaGresaka.IndexOf(PodaciView.listaGresaka[PodaciView.listaGresaka.Count - VT100Singleton.InstancaVT100.GornjiDio]);
                    i < PodaciView.listaGresaka.Count; i++)
                {
                    if (i == PodaciView.listaGresaka.Count - 1)
                        Console.Write(VT100Singleton.InstancaVT100.ANSI_ESC + "31m" + PodaciView.listaGresaka[i]);
                    else Console.Write(VT100Singleton.InstancaVT100.ANSI_ESC + "31m" + PodaciView.listaGresaka[i] + "\n");
                }
                Console.Write(VT100Singleton.InstancaVT100.ANSI_ESC + "H");
                Console.Write(VT100Singleton.InstancaVT100.ANSI_ESC + 
                    (VT100Singleton.InstancaVT100.GornjiDio + VT100Singleton.InstancaVT100.DonjiDio - 1) + "B");
            } 
            else
            {
                Console.Write(VT100Singleton.InstancaVT100.ANSI_ESC + VT100Singleton.InstancaVT100.GornjiDio + "B");
                for (int i = PodaciView.listaGresaka.IndexOf(PodaciView.listaGresaka[PodaciView.listaGresaka.Count - VT100Singleton.InstancaVT100.DonjiDio]);
                    i < PodaciView.listaGresaka.Count; i++)
                {
                    if (i == PodaciView.listaGresaka.Count - 1) 
                        Console.Write(VT100Singleton.InstancaVT100.ANSI_ESC + "31m" + PodaciView.listaGresaka[i]);
                    else Console.Write(VT100Singleton.InstancaVT100.ANSI_ESC + "31m" + PodaciView.listaGresaka[i] + "\n");
                }
                Console.Write(VT100Singleton.InstancaVT100.ANSI_ESC + "H");
                Console.Write(VT100Singleton.InstancaVT100.ANSI_ESC + (VT100Singleton.InstancaVT100.GornjiDio-1) + "B");
            }
            Console.Write(VT100Singleton.InstancaVT100.ANSI_ESC + "0m"); 
        }

        private static void postaviBrVtPd(string[] args)
        {
            if (!args.Contains("-br")) throw new Exception("Nedostaje argument -br");
            if (!args.Contains("-vt")) throw new Exception("Nedostaje argument -vt");
            if (!args.Contains("-pd")) throw new Exception("Nedostaje argument -pd");

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals("-br")) ucitajBr(args[i + 1]);
                if (args[i].Equals("-vt")) ucitajVt(args[i + 1]);
                if (args[i].Equals("-pd")) ucitajPd(args[i + 1]);
            }
        }

        private static void ucitajBr(string stringBr)
        {
            VT100Singleton.InstancaVT100.Br = int.TryParse(stringBr, out int broj)
                 ? broj
                 : throw new Exception("Argument -br nije cijeli broj.");

            if (VT100Singleton.InstancaVT100.Br < 24 || VT100Singleton.InstancaVT100.Br > 80) 
                throw new Exception("Argument -br mora biti u rasponu od 24 do 80");

            PotrebniArgumentiSingleton.InstancaPotrebniArgumenti.Br = true;
        }

        private static void ucitajVt(string stringVt)
        {
            VT100Singleton.InstancaVT100.Vt = Regex.Match(stringVt, 
                @"^([0-9]|^[1-9][0-9]|^(100)):([0-9]$|[1-9][0-9]$|(100))$").Success == true
                 ? stringVt
                 : throw new Exception("Argument -vt nije u formatu {x:y}.");

            string[] splitVt = VT100Singleton.InstancaVT100.Vt.Split(":");
            if (int.Parse(splitVt[0]) + int.Parse(splitVt[1]) != 100)
                throw new Exception("Suma brojeva x i y kod argumenta -vt {x:y} mora biti jednaka 100.");

            PotrebniArgumentiSingleton.InstancaPotrebniArgumenti.Vt = true;
        }
        
        private static void ucitajPd(string stringPd)
        {
            VT100Singleton.InstancaVT100.Pd = Regex.Match(stringPd, @"(^P:R$)|(^R:P$)").Success == true
                 ? stringPd
                 : throw new Exception("Argument -pd nema jednu od dvije moguce vrijednosti {R:P | P:R}.");

            PotrebniArgumentiSingleton.InstancaPotrebniArgumenti.Pd = true;
        }

        private static void inicijalizirajArgumente(string[] args)
        {
            BrojacGresakaSingleton.InstancaBrojacGresaka.brojGreske = 0;
            UcitavanjeArgumenataController.ucitajArgumente(args);
            MolVezoviController.provjeriSadrzavajuLiVezoviMolove();
            ispisiGreske();
            provjeriSveObavezneArgumente();
            inicijalizirajComposite();

            string komanda = "";
            while (!komanda.Equals("Q"))
            {
                Console.Write("Unesi komandu (VT pali/gasi emulator): ");
                komanda = Console.ReadLine();
                izvrsiTrazenuKomandu(komanda);
                if (VT100Singleton.InstancaVT100.On)
                {
                    ispisiOdgovore();
                }
                else
                {
                    KomandeView.listaOdgovora.ForEach(odg =>
                    {
                        Console.WriteLine(odg);
                    });
                    KomandeView.listaOdgovora.Clear();
                }
            }
            
            Console.Write(VT100Singleton.InstancaVT100.ANSI_ESC + "H");
            Console.Write(VT100Singleton.InstancaVT100.ANSI_ESC + "J");
        }

        private static void inicijalizirajComposite()
        {
            IBrodskaLuka luka = new LukeController();
            IBrodskaLuka molovi = new MoloviController();
            IBrodskaLuka vezovi = new VezoviController();
            luka.dodaj(molovi);
            luka.dodaj(vezovi);
        }

        private static void ispisiOdgovore()
        {
            if (VT100Singleton.InstancaVT100.Pd.Split(":")[0] == "R")
            {
                prebrisiCijeliRDio("G");
                ispisRGornji();
            }
            else
            {
                prebrisiCijeliRDio("D");
                ispisRDonji();
            }
            Console.Write(VT100Singleton.InstancaVT100.ANSI_ESC + "0m");
            KomandeView.listaOdgovora.Clear();
        }

        private static void ispisRGornji()
        {
            if (KomandeView.listaOdgovora.Count < VT100Singleton.InstancaVT100.GornjiDio)
            {
                for (int i = 0; i < KomandeView.listaOdgovora.Count; i++)
                {
                    Console.Write(VT100Singleton.InstancaVT100.ANSI_ESC + "32m" + KomandeView.listaOdgovora[i] + "\n");
                }

                for (int i = KomandeView.listaOdgovora.Count; i < VT100Singleton.InstancaVT100.GornjiDio - 1; i++)
                {
                    Console.WriteLine(VT100Singleton.InstancaVT100.ANSI_ESC + "2K");
                }
            }
            else
            {
                for (int i = KomandeView.listaOdgovora.IndexOf(KomandeView.listaOdgovora
                    [KomandeView.listaOdgovora.Count - VT100Singleton.InstancaVT100.GornjiDio + 1]);
                i < KomandeView.listaOdgovora.Count; i++)
                {
                    Console.Write(VT100Singleton.InstancaVT100.ANSI_ESC + "32m" + KomandeView.listaOdgovora[i] + "\n");
                }
            }
            Console.Write(VT100Singleton.InstancaVT100.ANSI_ESC + "H");
            Console.Write(VT100Singleton.InstancaVT100.ANSI_ESC + (VT100Singleton.InstancaVT100.GornjiDio - 1) + "B");
        }

        private static void ispisRDonji()
        {
            if (KomandeView.listaOdgovora.Count < VT100Singleton.InstancaVT100.DonjiDio)
            {
                for (int i = 0; i < KomandeView.listaOdgovora.Count; i++)
                {
                    Console.Write(VT100Singleton.InstancaVT100.ANSI_ESC + "32m" + KomandeView.listaOdgovora[i] + "\n");
                }

                for (int i = KomandeView.listaOdgovora.Count; i < VT100Singleton.InstancaVT100.DonjiDio - 1; i++)
                {
                    Console.WriteLine(VT100Singleton.InstancaVT100.ANSI_ESC + "2K");
                }
            }
            else
            {
                for (int i = KomandeView.listaOdgovora.IndexOf(KomandeView.listaOdgovora
                    [KomandeView.listaOdgovora.Count - VT100Singleton.InstancaVT100.DonjiDio + 1]);
                i < KomandeView.listaOdgovora.Count; i++)
                {
                    Console.Write(VT100Singleton.InstancaVT100.ANSI_ESC + "32m" + KomandeView.listaOdgovora[i] + "\n");
                }

                Console.Write(VT100Singleton.InstancaVT100.ANSI_ESC +
                    (VT100Singleton.InstancaVT100.DonjiDio - KomandeView.listaOdgovora.Count) + "B");
            }

            Console.Write(VT100Singleton.InstancaVT100.ANSI_ESC + "H");
            Console.Write(VT100Singleton.InstancaVT100.ANSI_ESC +
                (VT100Singleton.InstancaVT100.GornjiDio + VT100Singleton.InstancaVT100.DonjiDio - 1) + "B");
        }

        private static void prebrisiCijeliRDio(string GD)
        {
            if (GD.Equals("G"))
            {
                Console.Write(VT100Singleton.InstancaVT100.ANSI_ESC + "H");
                for (int i = 0; i < VT100Singleton.InstancaVT100.GornjiDio; i++)
                {
                    Console.WriteLine(VT100Singleton.InstancaVT100.ANSI_ESC + "2K");
                }
                Console.Write(VT100Singleton.InstancaVT100.ANSI_ESC + "H");
            } else
            {
                Console.Write(VT100Singleton.InstancaVT100.ANSI_ESC + "H");
                Console.Write(VT100Singleton.InstancaVT100.ANSI_ESC + VT100Singleton.InstancaVT100.GornjiDio + "B");
                for (int i = 0; i < VT100Singleton.InstancaVT100.DonjiDio; i++)
                {
                    Console.WriteLine(VT100Singleton.InstancaVT100.ANSI_ESC + "2K");
                }
                Console.Write(VT100Singleton.InstancaVT100.ANSI_ESC + "H");
                Console.Write(VT100Singleton.InstancaVT100.ANSI_ESC + VT100Singleton.InstancaVT100.GornjiDio + "B");
            }
        }

        private static void provjeriSveObavezneArgumente()
        {
            if (!PotrebniArgumentiSingleton.InstancaPotrebniArgumenti.L) throw new Exception("Argument -l ne postoji!");
            if (!PotrebniArgumentiSingleton.InstancaPotrebniArgumenti.V) throw new Exception("Argument -v ne postoji!");
            if (!PotrebniArgumentiSingleton.InstancaPotrebniArgumenti.B) throw new Exception("Argument -b ne postoji!");
            if (!PotrebniArgumentiSingleton.InstancaPotrebniArgumenti.Br) throw new Exception("Argument -br ne postoji!");
            if (!PotrebniArgumentiSingleton.InstancaPotrebniArgumenti.Vt) throw new Exception("Argument -vt ne postoji!");
            if (!PotrebniArgumentiSingleton.InstancaPotrebniArgumenti.Pd) throw new Exception("Argument -pd ne postoji!");
        }

        private static void izvrsiTrazenuKomandu(string komanda)
        {
            provjeriPostojiLiKomanda(komanda);
            prviSetKomandi(komanda);
            drugiSetKomandi(komanda);
            treciSetKomandi(komanda);
        }

        private static void provjeriPostojiLiKomanda(string komanda)
        {
            if (Regex.Match(komanda,
                @"^(I|VR|V|UR|ZD|ZP|F|T|ZA|SPS|VPS|Q|LOG|VT)").Success != true)
            {
                ispisiNovoVirtualnoVrijeme();
                KomandeView.ispisiOdgovor($"Komanda {komanda} ne postoji.");
            }   
        }

        private static void prviSetKomandi(string komanda)
        {
            if (komanda.Equals("I"))
            {
                ispisiNovoVirtualnoVrijeme();
                KomandaIController.ispisiTablicu();
            }
            if (komanda.StartsWith("VR"))
            {
                ispisiNovoVirtualnoVrijeme();
                KomandaVRController.postaviNovoVirtualnoVrijeme(komanda);
            }
            if (komanda.StartsWith("UR"))
            {
                ispisiNovoVirtualnoVrijeme();
                KomandaURController.dohvatiRezervacije(komanda);
            }
            if (komanda.StartsWith("V") && !komanda.StartsWith("VR") && !komanda.StartsWith("VT") && !komanda.StartsWith("VPS"))
            {
                ispisiNovoVirtualnoVrijeme();
                KomandaVController.ispisVezova(komanda);
            }
            if (komanda.StartsWith("ZD"))
            {
                ispisiNovoVirtualnoVrijeme();
                KomandaZDController.provediZahtjev(komanda);
            }
            if (komanda.StartsWith("ZP"))
            {
                ispisiNovoVirtualnoVrijeme();
                KomandaZPController.provediZahtjev(komanda);
            }
        }

        private static void drugiSetKomandi(string komanda)
        {
            if (komanda.StartsWith("F"))
            {
                ispisiNovoVirtualnoVrijeme();
                if (!komanda.Contains("Q"))
                {
                    KomandaFController.dodajBrodNaKanal(komanda);
                } else
                {
                    KomandaFController.odjaviBrodSKanala(komanda);
                }
            }
            if (komanda.StartsWith("T"))
            {
                ispisiNovoVirtualnoVrijeme();
                KomandaTController.urediIspis(komanda);
            }
            if (komanda.StartsWith("ZA"))
            {
                ispisiNovoVirtualnoVrijeme();
                KomandaZAController.ispisiTablicu(komanda);
            }
            if (komanda.Equals("LOG"))
            {
                ispisiNovoVirtualnoVrijeme();
                KomandeView.ispisiDnevnik(); 
            }
        }

        private static void treciSetKomandi(string komanda)
        {
            if (komanda.StartsWith("SPS"))
            {
                ispisiNovoVirtualnoVrijeme();
                KomandaSPSVPSController.pohraniPostojeceStanje(komanda);
            }
            if (komanda.StartsWith("VPS"))
            {
                ispisiNovoVirtualnoVrijeme();
                KomandaSPSVPSController.dohvatiPohranjenoStanje(komanda);
            }
            if (komanda.Equals("VT"))
            {
                Console.Write(VT100Singleton.InstancaVT100.ANSI_ESC + "H");
                Console.Write(VT100Singleton.InstancaVT100.ANSI_ESC + "J");
                ispisiNovoVirtualnoVrijeme();

                if (VT100Singleton.InstancaVT100.On)
                {
                    KomandeView.ispisiOdgovor("VT off.");
                    VT100Singleton.InstancaVT100.On = false;
                }
                else
                {
                    KomandeView.ispisiOdgovor("VT on.");
                    ispisiGreske();
                    VT100Singleton.InstancaVT100.On = true;
                }
            }
        }

        public static void ispisiNovoVirtualnoVrijeme()
        {
            DateTime novoVrijeme = VirtualnoVrijemeSingleton.InstancaVirtualnoVrijeme.virtualnoVrijeme +=
               DateTime.Now.Subtract(VrijemeUcitavanjaSingleton.InstancaVrijemeUcitavanja.Vrijeme);
            KomandeView.ispisiOdgovor(novoVrijeme.ToString());
            VrijemeUcitavanjaSingleton.InstancaVrijemeUcitavanja.Vrijeme = DateTime.Now;
        }      
    } 
}