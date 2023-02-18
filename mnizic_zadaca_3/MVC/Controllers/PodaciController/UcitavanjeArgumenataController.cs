using mnizic_zadaca_3.FactoryMethod.Creator;
using mnizic_zadaca_3.FactoryMethod.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mnizic_zadaca_3.MVC.Controllers.KomandeController;
using mnizic_zadaca_3.MVC.Models;
using System.Text.RegularExpressions;

namespace mnizic_zadaca_3.MVC.Controllers.PodaciController
{
    public class UcitavanjeArgumenataController
    {
        public static void ucitajArgumente(string[] args)
        {
            provjeriOpcije(args);
            for (int i = 0; i < args.Length; i++)
            { 
                if (args[i].Equals("-l")) ucitajLuku(args[i + 1]);
                if (args[i].Equals("-v")) ucitajVezove(args[i + 1]);
                if (args[i].Equals("-b")) ucitajBrodove(args[i + 1]);
                if (args[i].Equals("-r")) ucitajRasporede(args[i + 1]);
                if (args[i].Equals("-m")) ucitajMolove(args[i + 1]);
                if (args[i].Equals("-mv")) ucitajMolVezove(args[i + 1]);
                if (args[i].Equals("-k")) ucitajKanale(args[i + 1]);
            }
        }

        private static void provjeriOpcije(string[] args)
        {
            for(int i = 0; i < args.Length; i += 2)
            {
                if(Regex.Match(args[i], @"^-(l|v|b|r|m|mv|k|br|vt|pd)$").Success != true)
                     throw new Exception($"Opcija {args[i]} nije ispravna.");
            }
        }

        private static void ucitajRasporede(string nazivDatoteke)
        {
            Citac c = new CitacRasporedaConcrete().FactoryMethod(nazivDatoteke);
            c.DohvatiPodatke();
        }

        private static void ucitajKanale(string nazivDatoteke)
        {
            Citac c = new CitacKanalaConcrete().FactoryMethod(nazivDatoteke);
            c.DohvatiPodatke();
            inicijalizirajListuKanalBrodova();
        }

        private static void inicijalizirajListuKanalBrodova()
        {
            KanaliController.listaKanala.ForEach(k =>
            {
                KanalBrodovi kb = new(k);
                kb.listaBrodovaNaKanalu.DefaultIfEmpty().ToList();
                KomandaFController.listaKanalBrodova.Add(kb);
            });
        }

        private static void ucitajMolVezove(string nazivDatoteke)
        {
            Citac c = new CitacMolVezovaConcrete().FactoryMethod(nazivDatoteke);
            c.DohvatiPodatke();
        }

        private static void ucitajMolove(string nazivDatoteke)
        {
            Citac c = new CitacMolovaConcrete().FactoryMethod(nazivDatoteke);
            c.DohvatiPodatke();
        }

        private static void ucitajLuku(string nazivDatoteke)
        {
            Citac c = new CitacLukaConcrete().FactoryMethod(nazivDatoteke);
            c.DohvatiPodatke();
        }

        private static void ucitajVezove(string nazivDatoteke)
        {
            Citac c = new CitacVezovaConcrete().FactoryMethod(nazivDatoteke);
            c.DohvatiPodatke();
        }

        private static void ucitajBrodove(string nazivDatoteke)
        {
            Citac c = new CitacBrodovaConcrete().FactoryMethod(nazivDatoteke);
            c.DohvatiPodatke();
        }
    }
}
