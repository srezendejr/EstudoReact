using Estudo.Model.Enum;
using EstudoReact.Model;
using EstudoReact.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudoReact.Services
{
    public class PedidoService
    {
        Context _context;
        public PedidoService()
        {
            _context = new Context();
        }

        public void SalvarPedido(Pedido pedido)
        {
            try
            {
                if (ValidaPedido(pedido))
                {
                    _context.Salvar<Pedido>(pedido);
                    _context.Commit().GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Pedido> ListarPedido()
        {
            return _context.Pedidos.ToList();
        }

        public Pedido SelecionarPedido(int Id)
        {
            return _context.Pedidos.FirstOrDefault(p => p.Id == Id);
        }

        public void ExcluirPedido(int id)
        {
            try
            {
                Pedido pedido = SelecionarPedido(id);
                _context.Excluir<Pedido>(pedido);
                _context.Commit().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool ValidaPedido(Pedido pedido)
        {
            bool valida = true;

            if (pedido == null)
            {
                valida = false;
                throw new Exception("Pedido inválido");
            }

            if (pedido.DataInclusao > DateTime.Now)
            {
                valida = false;
                throw new Exception("A data do pedido é maior que a data atual");
            }

            if (pedido.Comprador == null || pedido.IdComprador == 0)
            {
                valida = false;
                throw new Exception("Informe um comprador válido para o pedido");
            }
            if (pedido.Itens == null || pedido.Itens.Count() == 0)
            {
                valida = false;
                throw new Exception("Informe itens para o pedido");
            }

            foreach (ItemPedido item in pedido.Itens)
            {
                if (item.Valor <= 0)
                {
                    valida = false;
                    throw new Exception("Informe um valor válido para o item do pedido");
                }

                if (!Enum.IsDefined(typeof(Moeda), item.Moeda))
                {
                    valida = false;
                    throw new Exception("Informe uma moeda válida");
                }
            }
            return valida;

        }
    }
}
