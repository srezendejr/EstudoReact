using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudoReact.Model
{
    public class Comprador
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Documento {  get; set; }
        public int IdCidade { get; set; }

        public virtual Cidade Cidade { get; set; }
        public virtual List<Pedido> Pedidos { get; set; }
    }
}
