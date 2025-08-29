using Estudo.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudoReact.Model
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public OrigemProduto Origem { get; set; }
    }
}