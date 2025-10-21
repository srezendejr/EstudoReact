using AutoMapper;
using EstudoReact.Model;
using EstudoReact.Server.DTO;
using EstudoReact.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace Estudo.UI.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoService _produtoService;
        private readonly IMapper _mapper;
        public ProdutoController(ProdutoService produtoService, IMapper mapper)
        {
            _produtoService = produtoService;
            _mapper = mapper;
        }

        [HttpGet(Name = "ListarProduto")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> ListarProdutos()
        {
            List<Produto> produtos = await _produtoService.ListarProduto();
            List<ProdutoDTO> dTOs = _mapper.Map<List<ProdutoDTO>>(produtos);
            return Ok(dTOs);
        }

        [HttpGet("{id}", Name = "SelecionarProduto")]
        public async Task<ActionResult<ProdutoDTO>> SelecionarProduto(int id)
        {
            Produto produto = await _produtoService.SelecionaProduto(id);
            ProdutoDTO dto = _mapper.Map<ProdutoDTO>(produto);
            return Ok(dto);
        }

        [HttpPut("{id}", Name = "AlterarProduto")]
        public async Task<IActionResult> AlterarProduto(int id, ProdutoDTO dto)
        {
            if (id != dto.Id)
                return BadRequest("ID da URL não corresponde ao ID do produto.");

            Produto produto = _mapper.Map<Produto>(dto);
            await _produtoService.SalvarProduto(produto);
            return CreatedAtRoute("SelecionarProduto", new { id = produto.Id }, dto);
        }


        [HttpPost(Name = "IncluirProduto")]
        public async Task<ActionResult<ProdutoDTO>> IncluirProduto(ProdutoDTO dto)
        {
            Produto produto = _mapper.Map<Produto>(dto);
            await _produtoService.SalvarProduto(produto);
            dto.Id = produto.Id;
            return CreatedAtRoute("SelecionarProduto", new { id = produto.Id }, dto);
        }


        [HttpDelete("{id}", Name = "ExcluirProduto")]
        public async Task<IActionResult> ExcluirProduto(int id)
        {
            var produto = await _produtoService.SelecionaProduto(id);
            if (produto == null)
                return NotFound();

            await _produtoService.ExcluirProduto(id);
            return NoContent();
        }
    }
}
