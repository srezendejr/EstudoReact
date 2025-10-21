using AutoMapper;
using EstudoReact.Model;
using EstudoReact.Server.DTO;
using EstudoReact.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Estudo.UI.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly PedidoService _pedidoService;
        private readonly CompradorService _compradorService;
        private readonly ProdutoService _produtoService;
        private readonly IMapper _mapper;
        public PedidoController(PedidoService pedidoService,
                                CompradorService compradorService,
                                ProdutoService produtoService,
                                IMapper mapper)
        {
            _pedidoService = pedidoService;
            _compradorService = compradorService;
            _produtoService = produtoService;
            _mapper = mapper;
        }

        [HttpGet(Name = "ListarPedido")]
        public async Task<ActionResult<IEnumerable<PedidoDTO>>> ListarPedidos()
        {
            List<Pedido> pedidos = await _pedidoService.ListarPedido();
            List<PedidoDTO> dTOs = _mapper.Map<List<PedidoDTO>>(pedidos);
            return Ok(dTOs);
        }

        [HttpGet("{id}", Name = "SelecionarPedido")]
        public async Task<ActionResult<PedidoDTO>> SelecionarPedido(int id)
        {
            Pedido pedido = await _pedidoService.SelecionarPedido(id);
            PedidoDTO pDTO = _mapper.Map<PedidoDTO>(pedido);

            return Ok(pDTO);
        }

        [HttpPut("{id}", Name = "AlterarPedido")]
        public async Task<ActionResult<PedidoDTO>> AlterarPedido(int id, PedidoDTO dto)
        {
            try
            {
                if (id != dto.Id)
                    return BadRequest("ID da rota difere do ID do comprador.");
                Pedido pedido = _mapper.Map<Pedido>(dto);
                await _pedidoService.SalvarPedido(pedido);

                Comprador comprador = await _compradorService.SelecionaComprador(pedido.IdComprador);
                pedido.Comprador = comprador;
                PedidoDTO dTO = _mapper.Map<PedidoDTO>(pedido);
                return Ok(dTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "IncluirPedido")]
        public async Task<ActionResult<PedidoDTO>> IncluirPedido(PedidoDTO dto)
        {
            Comprador comprador = await _compradorService.SelecionaComprador(dto.IdComprador);
            Pedido pedido = _mapper.Map<Pedido>(dto);
            pedido.Comprador = comprador;
            await _pedidoService.SalvarPedido(pedido);
            
            pedido.Comprador = comprador;
            PedidoDTO dTO = _mapper.Map<PedidoDTO>(pedido);
            return Ok(dto);
        }

        [HttpDelete("{id}", Name = "ExcluirPedido")]
        public async Task<IActionResult> ExcluirPedido(int id)
        {
            await _pedidoService.ExcluirPedido(id);
            return NoContent();
        }
    }
}
