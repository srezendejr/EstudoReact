using EstudoReact.Model;
using EstudoReact.Server.DTO;
using EstudoReact.Services;
using Microsoft.AspNetCore.Mvc;

namespace Estudo.UI.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompradorController : ControllerBase
    {
        CompradorService _compradorService;
        public CompradorController()
        {
            _compradorService = new CompradorService();
        }

        [HttpGet(Name = "Listar")]
        public IEnumerable<CompradoresDTO> SelecionaCompradores()
        {
            List<Comprador> compradores = _compradorService.ListaCompradores();
            List<CompradoresDTO> dTOs = new List<CompradoresDTO>();
            foreach (Comprador item in compradores)
            {
                dTOs.Add(new CompradoresDTO
                {
                    Id = item.Id,
                    Nome = item.Nome,
                    Cidade = item.Cidade.Nome,
                    IdCidade = item.IdCidade
                });
            }
            return dTOs;
        }

        [HttpGet("{id}", Name = "Selecionar")]
        public CompradoresDTO SelecionarComprador(int id)
        {
            Comprador comprador = _compradorService.SelecionaComprador(id);
            CompradoresDTO dTO = new CompradoresDTO()
            {
                Id = comprador.Id,
                Nome = comprador.Nome,
                Cidade = comprador.Cidade.Nome,
                IdCidade = comprador.IdCidade
            };

            return dTO;
        }

        [HttpPut("{id}", Name = "Alterar")]
        public void Alterarcomprador(CompradoresDTO comprador)
        {
            Comprador comp = new Comprador { 
                Id = comprador.Id,
                IdCidade = comprador.IdCidade,
                Documento = comprador.Documento,
                Nome = comprador.Nome
            };
            _compradorService.SalvarComprador(comp);
        }

        [HttpPost(Name = "Incluir")]
        public void Incluircomprador(CompradoresDTO comprador)
        {
            Comprador comp = new Comprador
            {
                Id = comprador.Id,
                IdCidade = comprador.IdCidade,
                Documento = comprador.Documento,
                Nome = comprador.Nome
            };
            _compradorService.SalvarComprador(comp);
        }

        [HttpDelete("{id}", Name = "Excluir")]
        public void Excluircomprador(int id)
        {
            _compradorService.ExcluirComprador(id);
        }
    }
}
