using mnizic_zadaca_3.Composite.Vezovi;
using mnizic_zadaca_3.MVC.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.IteratorPattern.Collections
{
    public class PutnickiVezoviCollection : IteratorAggregate
    {
        List<PutnickiVezovi> collection = new();

        public List<PutnickiVezovi> dohvatiPutnickeVezove()
        {
            return this.collection;
        }

        public void DodajPutnickiVez(PutnickiVezovi item)
        {
            this.collection.Add(item);
        }

        public bool Any(PutnickiVezovi item)
        {
            if (collection.Any(x => x.ID == item.ID)) return true;
            return false;
        }

        public bool ContainsID(int id)
        {
            if (collection.Any(x => x.ID == id)) return true;
            return false;
        }

        public PutnickiVezovi Find(int id)
        {
            return collection.Find(x => x.ID == id);
        }
        public void Clear()
        {
            this.collection.Clear();
        }

        public void AddRange(List<PutnickiVezovi> lista)
        {
            this.collection.AddRange(lista);
        }

        public override IEnumerator GetEnumerator()
        {
            return new PutnickiVezoviIterator(this);
        }

        private class PutnickiVezoviIterator : Iterator
        {
            private PutnickiVezoviCollection collection { get; set; }
            private int index = -1;

            public PutnickiVezoviIterator(PutnickiVezoviCollection collection)
            {
                this.collection = collection;
            }

            public override object Current()
            {
                return this.collection.dohvatiPutnickeVezove()[index];
            }

            public override int Key()
            {
                return this.index;
            }

            public override bool MoveNext()
            {
                int noviIndex = this.index + 1;

                if (noviIndex >= 0 && noviIndex < this.collection.dohvatiPutnickeVezove().Count)
                {
                    this.index = noviIndex;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public override void Reset()
            {
                this.index = 0;
            }
        }
    }
}
