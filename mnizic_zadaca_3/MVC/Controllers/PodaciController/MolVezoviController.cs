using Microsoft.VisualBasic;
using mnizic_zadaca_3.IteratorPattern.Collections;
using mnizic_zadaca_3.Composite.Vezovi;
using mnizic_zadaca_3.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mnizic_zadaca_3.IteratorPattern;
using mnizic_zadaca_3.MVC.Models;
using mnizic_zadaca_3.MVC.Views;

namespace mnizic_zadaca_3.MVC.Controllers.PodaciController
{
    public class MolVezoviController
    {
        public static List<MolVezovi> listaMolVezova = new();
        public static List<PutnickiVezovi> listaPUVezovaKojiImajuMol = new();
        public static List<PoslovniVezovi> listaPOVezovaKojiImajuMol = new();
        public static List<OstaliVezovi> listaOSVezovaKojiImajuMol = new();

        public static void DohvatiMolVezove(string linijaUDatoteci)
        {
            string[] dohvaceneVrijednosti = linijaUDatoteci.Split(';');
            try
            {
                provjeriBrojDohvacenihVrijednosti(dohvaceneVrijednosti);
                MolVezovi molVez = provjeriMolVez(dohvaceneVrijednosti);
                provjeriDuplikat(molVez);
                listaMolVezova.Add(molVez);
            }
            catch (Exception ex)
            {
                PodaciView.ispisGreske(++BrojacGresakaSingleton.InstancaBrojacGresaka.brojGreske, ex.Message);
            }
        }

        public static void provjeriSadrzavajuLiVezoviMolove()
        {
            napuniVezoveKojiImajuMolove();
            baciGreskuVezovimaKojiNemajuMolove();
        }

        private static void baciGreskuVezovimaKojiNemajuMolove()
        {
            provjeriPUVezove();
            provjeriPOVezove();
            provjeriOSVezove();
        }

        private static void provjeriOSVezove()
        {
            for (Iterator iter = (Iterator)OstaliVezovi.ostaliVezoviLista.GetEnumerator(); iter.MoveNext();)
            {
                OstaliVezovi os = (OstaliVezovi)iter.Current();
                try
                {
                    if (!listaOSVezovaKojiImajuMol.Any(x => x.ID == os.ID))
                    {
                        throw new Exception($"Vez pod ID-om {os.ID} nema mol!");
                    }
                }
                catch (Exception ex)
                {
                    PodaciView.ispisGreske(++BrojacGresakaSingleton.InstancaBrojacGresaka.brojGreske, ex.Message);
                }
            }

            OstaliVezovi.ostaliVezoviLista.Clear();
            OstaliVezovi.ostaliVezoviLista.AddRange(listaOSVezovaKojiImajuMol);
        }

        private static void provjeriPOVezove()
        {
            for (Iterator iter = (Iterator)PoslovniVezovi.poslovniVezoviLista.GetEnumerator(); iter.MoveNext();)
            {
                PoslovniVezovi po = (PoslovniVezovi)iter.Current();
                try
                {
                    if (!listaPOVezovaKojiImajuMol.Any(x => x.ID == po.ID))
                    {
                        throw new Exception($"Vez pod ID-om {po.ID} nema mol!");
                    }
                }
                catch (Exception ex)
                {
                    PodaciView.ispisGreske(++BrojacGresakaSingleton.InstancaBrojacGresaka.brojGreske, ex.Message);
                }
            }

            PoslovniVezovi.poslovniVezoviLista.Clear();
            PoslovniVezovi.poslovniVezoviLista.AddRange(listaPOVezovaKojiImajuMol);
        }

        private static void provjeriPUVezove()
        {
            for (Iterator iter = (Iterator)PutnickiVezovi.putnickiVezoviLista.GetEnumerator(); iter.MoveNext();)
            {
                PutnickiVezovi pu = (PutnickiVezovi)iter.Current();
                try
                {
                    if (!listaPUVezovaKojiImajuMol.Any(x => x.ID == pu.ID))
                    {
                        throw new Exception($"Vez pod ID-om {pu.ID} nema mol!");
                    }
                }
                catch (Exception ex)
                {
                    PodaciView.ispisGreske(++BrojacGresakaSingleton.InstancaBrojacGresaka.brojGreske, ex.Message);
                }
            }

            PutnickiVezovi.putnickiVezoviLista.Clear();
            PutnickiVezovi.putnickiVezoviLista.AddRange(listaPUVezovaKojiImajuMol);
        }

        private static void napuniVezoveKojiImajuMolove()
        {
            listaMolVezova.ForEach(mv =>
            {
                mv.idVezova.ForEach(v =>
                {
                    if (PutnickiVezovi.putnickiVezoviLista.ContainsID(v))
                    {
                        listaPUVezovaKojiImajuMol.Add(PutnickiVezovi.putnickiVezoviLista.Find(v));
                    }
                    else if (PoslovniVezovi.poslovniVezoviLista.ContainsID(v))
                    {
                        listaPOVezovaKojiImajuMol.Add(PoslovniVezovi.poslovniVezoviLista.Find(v));
                    }
                    else if (OstaliVezovi.ostaliVezoviLista.ContainsID(v))
                    {
                        listaOSVezovaKojiImajuMol.Add(OstaliVezovi.ostaliVezoviLista.Find(v));
                    }
                });
            });
        }

        private static void provjeriDuplikat(MolVezovi molVez)
        {
            if (listaMolVezova.Any(x => x.idMol == molVez.idMol))
            {
                throw new Exception($"Mol sa ID-om {molVez.idMol} vec postoji.");
            }
        }

        private static void provjeriBrojDohvacenihVrijednosti(string[] dohvaceneVrijednosti)
        {
            if (dohvaceneVrijednosti.Length != 2) throw new Exception("Netocan broj atributa.");
        }

        private static MolVezovi provjeriMolVez(string[] vrijednosti)
        {
            return new()
            {
                idMol = postaviIdMola(vrijednosti[0]),
                idVezova = postaviListuVezova(vrijednosti[1])
            };

        }

        private static int postaviIdMola(string stringIdMola)
        {
            return int.TryParse(stringIdMola, out int idMola)
                 ? idMola
                 : throw new Exception("ID mola nije cijeli broj.");
        }

        private static List<int> postaviListuVezova(string stringVezovi)
        {
            List<int> listaVezova = new();
            string[] splitStringVezova = stringVezovi.Split(',');

            splitStringVezova.ToList().ForEach(v =>
            {
                int id = postaviIdVeza(v);
                listaVezova.Add(id);
            });

            return listaVezova;
        }

        private static int postaviIdVeza(string stringIdVeza)
        {
            return int.TryParse(stringIdVeza, out int idVeza)
                 ? idVeza
                 : throw new Exception("ID veza nije cijeli broj.");
        }
    }
}
