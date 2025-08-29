using EstudoReact.Model;
using EstudoReact.Server.DTO;
using EstudoReact.Services;
using Microsoft.AspNetCore.Mvc;

namespace Estudo.UI.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PedidoController : ControllerBase
    {
        PedidoService _pedidoService;

        PedidoController()
        {
            _pedidoService = new PedidoService();
        }

        [HttpGet(Name = "Pedidos")]
        public IEnumerable<PedidoDTO> ListarPedidos()
        {
            List<Pedido> pedidos = _pedidoService.ListarPedido();
            List<PedidoDTO> dTOs = new List<PedidoDTO>();
            foreach (Pedido pedido in pedidos)
            {
                PedidoDTO pDTO = new PedidoDTO()
                {
                    Id = pedido.Id,
                    DataInclusao = pedido.DataInclusao,
                    Comprador = pedido.Comprador.Nome,
                    IdComprador = pedido.IdComprador
                };

                foreach (ItemPedido produto in pedido.Itens)
                {
                    ItemPedidoDTO itemdto = new ItemPedidoDTO
                    {
                        IdPedido = produto.IdPedido,
                        IdProduto = produto.IdProduto,
                        Item = produto.Item,
                        Produto = produto.Produto.Nome,
                        Moeda = produto.Moeda.ToString(),
                        Valor = produto.Valor,
                    };
                    pDTO.ItemPedidoDTOs.Add(itemdto);
                }
            }
            return dTOs;
        }

        [HttpGet("{id}", Name = "Pedido")]
        public PedidoDTO SelecionarPedido(int id)
        {
            Pedido pedido = _pedidoService.SelecionarPedido(id);

            PedidoDTO pDTO = new PedidoDTO()
            {
                Id = pedido.Id,
                DataInclusao = pedido.DataInclusao,
                Comprador = pedido.Comprador.Nome,
                IdComprador = pedido.IdComprador
            };

            foreach (ItemPedido produto in pedido.Itens)
            {
                ItemPedidoDTO itemdto = new ItemPedidoDTO
                {
                    IdPedido = produto.IdPedido,
                    IdProduto = produto.IdProduto,
                    Item = produto.Item,
                    Produto = produto.Produto.Nome,
                    Moeda = produto.Moeda.ToString(),
                    Valor = produto.Valor,
                };
                pDTO.ItemPedidoDTOs.Add(itemdto);
            }

            return pDTO;
        }

        [HttpPut("{id}", Name = "Pedido")]
        public void AlterarPedido(Pedido Pedido)
        {
            _pedidoService.SalvarPedido(Pedido);
        }

        [HttpPost(Name = "Pedidos")]
        public void IncluirPedido(Pedido Pedido)
        {
            _pedidoService.SalvarPedido(Pedido);
        }

        [HttpDelete("{id}", Name = "Pedido")]
        public void ExcluirPedido(int id)
        {
            _pedidoService.ExcluirPedido(id);
        }
    }
}
