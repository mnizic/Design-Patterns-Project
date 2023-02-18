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
    public class MoloviCollection : IteratorAggregate
    {
        List<Mol> collection = new();

        public List<Mol> dohvatiMolove()
        {
            return this.collection;
        }

        public void DodajMol(Mol item)
        {
            this.collection.Add(item);
        }

        public bool Any(Mol item)
        {
            if (collection.Any(x => x.ID == item.ID)) return true;
            return false;
        }

        public Mol Find(int id)
        {
            return collection.Find(x => x.ID == id);
        }

        public bool ContainsID(int id)
        {
            if (collection.Any(x => x.ID == id)) return true;
            return false;
        }

        public void Clear()
        {
            this.collection.Clear();
        }

        public void AddRange(List<Mol> lista)
        {
            this.collection.AddRange(lista);
        }

        public override IEnumerator GetEnumerator()
        {
            return new MoloviIterator(this);
        }

        private class MoloviIterator : Iterator
        {
            private MoloviCollection collection { get; set; }
            private int index = -1;

            public MoloviIterator(MoloviCollection collection)
            {
                this.collection = collection;
            }

            public override object Current()
            {
                return this.collection.dohvatiMolove()[index];
            }

            public override int Key()
            {
                return this.index;
            }

            public override bool MoveNext()
            {
                int noviIndex = this.index + 1;

                if (noviIndex >= 0 && noviIndex < this.collection.dohvatiMolove().Count)
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
