using mnizic_zadaca_3.ChainOfResponsibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mnizic_zadaca_3.MVC.Controllers.PodaciController;
using mnizic_zadaca_3.MVC.Models;
using mnizic_zadaca_3.MVC.Views;

namespace mnizic_zadaca_3.MVC.Controllers.KomandeController
{
    public class KomandaFController
    {
        public static List<KanalBrodovi> listaKanalBrodova { get; set; } = new();

        public static void dodajBrodNaKanal(string komanda)
        {
            string[] splitKomande = komanda.Split(" ");
            try
            {
                provjeriIspravnostKomandeF(komanda, splitKomande);
                int idBroda = postaviIdBroda(splitKomande[1]);
                int frekvencija = postaviFrekvenciju(splitKomande[2]);

                Brod? brod = BrodoviController.brodoviLista.Find(x => x.ID == idBroda);

                brodNePostoji(brod);
                brodVecPostojiNaKanalu(brod);
                KanalBrodovi? kb = listaKanalBrodova.Find(x => x.kanal.frekvencija == frekvencija);
                frekvencijaNePostoji(kb);

                if (kb != null)
                {
                    if (kb.listaBrodovaNaKanalu.Count < kb.kanal.maksimalanBroj)
                    {
                        kb.listaBrodovaNaKanalu.Add(brod);
                    }
                    else
                    {
                        throw new Exception($"Kanal frekvencije {kb.kanal.frekvencija} premašio " +
                            $"je broj maksimalno dopuštenih sudionika.");
                    }

                    if (kb.listaBrodovaNaKanalu.Contains(brod))
                    {
                        KomandeView.ispisiOdgovor($"Brod s ID {brod.ID} spaja se na kanalu {kb.kanal.frekvencija}.");
                    }
                }
            }
            catch (Exception ex)
            {
                KomandeView.ispisiOdgovor(ex.Message);
            }
        }

        public static void odjaviBrodSKanala(string komanda)
        {
            string[] splitKomande = komanda.Split(" ");

            try
            {
                provjeriIspravnostKomandeF(komanda, splitKomande);
                int idBroda = postaviIdBroda(splitKomande[1]);
                int frekvencija = postaviFrekvenciju(splitKomande[2]);

                Brod? brod = BrodoviController.brodoviLista.Find(x => x.ID == idBroda);

                brodNePostoji(brod);
                brodNePostojiNaKanalu(brod);
                KanalBrodovi? kb = listaKanalBrodova.Find(x => x.kanal.frekvencija == frekvencija);
                frekvencijaNePostoji(kb);

                if (kb != null && kb.listaBrodovaNaKanalu.Contains(brod))
                {
                    listaKanalBrodova.Remove(kb);
                    kb.listaBrodovaNaKanalu.Remove(brod);
                    listaKanalBrodova.Add(kb);
                    KomandeView.ispisiOdgovor($"Brod s ID {brod.ID} se odjavljuje s kanala {kb.kanal.frekvencija}.");
                }
            }
            catch (Exception ex)
            {
                KomandeView.ispisiOdgovor(ex.Message);
            }
        }

        private static void brodNePostojiNaKanalu(Brod? brod)
        {
            if (!listaKanalBrodova.Any(x => x.listaBrodovaNaKanalu.Contains(brod)))
                throw new Exception($"Ne postoji navedeni brod s ID {brod.ID} na nijednom kanalu.");
        }

        private static void frekvencijaNePostoji(KanalBrodovi? kb)
        {
            if (kb == null) throw new Exception($"Ne postoji navedena frekvencija {kb.kanal.frekvencija}");
        }

        private static void brodNePostoji(Brod? brod)
        {
            if (brod == null) throw new Exception($"Ne postoji brod s ID {brod.ID}");
        }

        private static void brodVecPostojiNaKanalu(Brod brod)
        {
            if (listaKanalBrodova.Any(x => x.listaBrodovaNaKanalu.Contains(brod)))
                throw new Exception($"Brod s ID {brod.ID} već ima kanal.");
        }

        private static int postaviFrekvenciju(string stringFrekvencije)
        {
            return int.TryParse(stringFrekvencije, out int dohvacenaFrekvencija)
                 ? dohvacenaFrekvencija
                 : throw new Exception("Frekvencija nije cijeli broj.");
        }

        private static int postaviIdBroda(string stringIDBroda)
        {
            return int.TryParse(stringIDBroda, out int dohvaceniIDBroda)
                 ? dohvaceniIDBroda
                 : throw new Exception("ID broda nije cijeli broj.");
        }

        private static void provjeriIspravnostKomandeF(string komanda, string[] splitKomande)
        {
            if (!(splitKomande.Length == 3 || splitKomande.Length == 4))
                throw new Exception($"Komanda {splitKomande[0]} je neispravna.");
        }


    }
}
