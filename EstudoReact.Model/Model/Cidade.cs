using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudoReact.Model
{
    public class Cidade
    {
        public int Id { get; set; }
        public string Nome {  get; set; }
        public int IdUf { get; set; }

        public virtual Estado UF { get; set; }

        public virtual List<Comprador> Compradores { get; set; }
    }
}
