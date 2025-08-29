using Estudo.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudoReact.Model
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime DataInclusao { get; set; }
        public int IdComprador { get; set; }
        public IList<ItemPedido> Itens { get; set; }
        public virtual Comprador Comprador { get; set; }
    }

    public class ItemPedido
    {
        public int Item { get; set; }
        public int IdPedido { get; set; }
        public decimal Valor {  get; set; }
        public Moeda Moeda { get; set; }
        public int IdProduto { get; set; }

        public virtual Produto Produto { get; set; }
        public virtual Pedido Pedido { get; set; } 
    }
}
