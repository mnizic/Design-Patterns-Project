using mnizic_zadaca_3.MVC.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.IteratorPattern.Collections
{
    public class LukeCollection : IteratorAggregate
    {
        public List<Luka> collection = new();

        public List<Luka> dohvatiLuke()
        {
            return this.collection;
        }

        public void DodajLuku(Luka item)
        {
            this.collection.Add(item);
        }

        public bool Any(Luka item)
        {
            if (collection.Any(x => x.naziv == item.naziv)) return true;
            return false;
        }

        public override IEnumerator GetEnumerator()
        {
            return new LukaIterator(this);
        }

        private class LukaIterator : Iterator
        {
            private LukeCollection collection { get; set; }
            private int index = -1;

            public LukaIterator(LukeCollection collection)
            {
                this.collection = collection;
            }

            public override object Current()
            {
                return this.collection.dohvatiLuke()[index];
            }

            public override int Key()
            {
                return this.index;
            }

            public override bool MoveNext()
            {
                int noviIndex = this.index + 1;

                if (noviIndex >= 0 && noviIndex < this.collection.dohvatiLuke().Count)
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
