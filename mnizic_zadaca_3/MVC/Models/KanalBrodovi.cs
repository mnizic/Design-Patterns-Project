using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnizic_zadaca_3.MVC.Models
{
    public class KanalBrodovi
    {
        public Kanal kanal { get; set; }
        public List<Brod> listaBrodovaNaKanalu { get; set; }

        public KanalBrodovi(Kanal kanal)
        {
            listaBrodovaNaKanalu = new();
            this.kanal = kanal;
        }
    }
}
