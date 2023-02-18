using mnizic_zadaca_3.Composite.Vezovi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.IteratorPattern.Collections
{
    public class OstaliVezoviCollection : IteratorAggregate
    {
        List<OstaliVezovi> collection = new();

        public List<OstaliVezovi> dohvatiOstaleVezove()
        {
            return this.collection;
        }

        public void DodajOstaliVez(OstaliVezovi item)
        {
            this.collection.Add(item);
        }

        public bool Any(OstaliVezovi item)
        {
            if (collection.Any(x => x.ID == item.ID)) return true;
            return false;
        }

        public bool ContainsID(int id)
        {
            if (collection.Any(x => x.ID == id)) return true;
            return false;
        }

        public OstaliVezovi Find(int id)
        {
            return collection.Find(x => x.ID == id);
        }

        public void Clear()
        {
            this.collection.Clear();
        }

        public void AddRange(List<OstaliVezovi> lista)
        {
            this.collection.AddRange(lista);
        }

        public override IEnumerator GetEnumerator()
        {
            return new OstaliVezoviIterator(this);
        }

        private class OstaliVezoviIterator : Iterator
        {
            private OstaliVezoviCollection collection { get; set; }
            private int index = -1;

            public OstaliVezoviIterator(OstaliVezoviCollection collection)
            {
                this.collection = collection;
            }

            public override object Current()
            {
                return this.collection.dohvatiOstaleVezove()[index];
            }

            public override int Key()
            {
                return this.index;
            }

            public override bool MoveNext()
            {
                int noviIndex = this.index + 1;

                if (noviIndex >= 0 && noviIndex < this.collection.dohvatiOstaleVezove().Count)
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
