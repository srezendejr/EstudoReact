using EstudoReact.Model;
using EstudoReact.Server.DTO;
using EstudoReact.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace Estudo.UI.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutoController : ControllerBase
    {
        ProdutoService _produtoService;
        public ProdutoController()
        {
            _produtoService = new ProdutoService();
        }

        [HttpGet(Name = "Produtos")]
        public IEnumerable<ProdutoDTO> ListarProdutos()
        {
            List<Produto> produtos = _produtoService.ListarProduto();
            List<ProdutoDTO> dTOs = new List<ProdutoDTO>();
            foreach (Produto item in produtos)
            {
                dTOs.Add(new ProdutoDTO
                {
                    Id = item.Id,
                    Nome = item.Nome,
                    Origem = item.Origem,
                });
            }
            return dTOs;
        }

        [HttpGet("{id}", Name = "Produto")]
        public ProdutoDTO SelecionarProduto(int id)
        {
            Produto produto = _produtoService.SelecionaProduto(id);
            ProdutoDTO dto = new ProdutoDTO
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Origem = produto.Origem,
            };
            return dto;
        }

        [HttpPut("{id}", Name = "Produto")]
        public void AlterarProduto(Produto produto)
        {
            _produtoService.SalvarProduto(produto);
        }

        [HttpPost(Name = "Produtos")]
        public void IncluirProduto(Produto produto)
        {
            _produtoService.SalvarProduto(produto);
        }

        [HttpDelete("{id}", Name = "Produto")]
        public void ExcluirProduto(int id)
        {
            _produtoService.ExcluirProduto(id);
        }
    }
}
