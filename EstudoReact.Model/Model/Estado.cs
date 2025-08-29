using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudoReact.Model
{
    public class Estado
    {
        public int Id { get; set; }
        public  string Nome { get; set; }

        public virtual List<Cidade> Cidades { get; set; }
    }
}
