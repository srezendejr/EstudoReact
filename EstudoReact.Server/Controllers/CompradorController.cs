using EstudoReact.Model;
using EstudoReact.Server.DTO;
using EstudoReact.Service;
using EstudoReact.Services;
using Microsoft.AspNetCore.Mvc;

namespace Estudo.UI.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompradorController : ControllerBase
    {
        CompradorService _compradorService;
        CidadeService _cidadeService;
        EstadoService _estadoService;
        public CompradorController()
        {
            _compradorService = new CompradorService();
            _cidadeService = new CidadeService();
            _estadoService = new EstadoService();
        }

        [HttpGet(Name = "Listar")]
        public IEnumerable<CompradoresDTO> SelecionaCompradores()
        {
            List<Comprador> compradores = _compradorService.ListaCompradores();
            List<CompradoresDTO> dTOs = new List<CompradoresDTO>();
            foreach (Comprador item in compradores)
            {
                item.Cidade = _cidadeService.SelecionaCidade(item.IdCidade);
                item.Cidade.UF = _estadoService.SelecionaEstado(item.Cidade.IdUf);
                dTOs.Add(new CompradoresDTO
                {
                    Id = item.Id,
                    Nome = item.Nome,
                    CidadeNome = item.Cidade.Nome,
                    IdCidade = item.IdCidade,
                    Documento = item.Documento,
                    IdEstado = item.Cidade.IdUf,
                    EstadoNome = item.Cidade.UF.Nome
                });
            }
            return dTOs;
        }

        [HttpGet("{id}", Name = "Selecionar")]
        public CompradoresDTO SelecionarComprador(int id)
        {
            Comprador comprador = _compradorService.SelecionaComprador(id);
            comprador.Cidade = _cidadeService.SelecionaCidade(comprador.IdCidade);
            comprador.Cidade.UF = _estadoService.SelecionaEstado(comprador.Cidade.IdUf);
            CompradoresDTO dTO = new CompradoresDTO()
            {
                Id = comprador.Id,
                Nome = comprador.Nome,
                CidadeNome = comprador.Cidade.Nome,
                IdCidade = comprador.IdCidade,
                IdEstado = comprador.Cidade.IdUf,
                EstadoNome = comprador.Cidade.UF.Nome
            };

            return dTO;
        }

        [HttpPut("{id}", Name = "Alterar")]
        public void Alterarcomprador(CompradoresDTO comprador)
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

        [HttpPost(Name = "Incluir")]
        public void Incluircomprador(CompradoresDTO comprador)
        {
            Comprador comp = new Comprador
            {
                Id = comprador.Id,
                IdCidade = comprador.IdCidade,
                Documento = comprador.Documento,
                Nome = comprador.Nome,
                Cidade = new Cidade { Id = comprador.IdCidade }
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
