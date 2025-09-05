using AutoMapper;
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
        private readonly CompradorService _compradorService;
        private readonly CidadeService _cidadeService;
        private readonly EstadoService _estadoService;
        private readonly IMapper _mapper;
        public CompradorController( CompradorService compradorService,
                                    CidadeService cidadeService,
                                    EstadoService estadoService,
                                    IMapper mapper
                                  )
        {
            _compradorService = compradorService;
            _cidadeService = cidadeService;
            _estadoService = estadoService;
            _mapper = mapper;
        }

        [HttpGet(Name = "Listar")]
        public ActionResult<IEnumerable<CompradoresDTO>> SelecionaCompradores()
        {
            List<Comprador> compradores = _compradorService.ListaCompradores();
            foreach (Comprador item in compradores)
            {
                item.Cidade = _cidadeService.SelecionaCidade(item.IdCidade);
                item.Cidade.UF = _estadoService.SelecionaEstado(item.Cidade.IdUf);
            }
            List<CompradoresDTO> dtos = _mapper.Map<List<CompradoresDTO>>(compradores);
            return Ok(dtos);
        }

        [HttpGet("{id}", Name = "Selecionar")]
        public ActionResult<CompradoresDTO> SelecionarComprador(int id)
        {
            Comprador comprador = _compradorService.SelecionaComprador(id);
            if (comprador == null)
                return NotFound();

            comprador.Cidade = _cidadeService.SelecionaCidade(comprador.IdCidade);
            comprador.Cidade.UF = _estadoService.SelecionaEstado(comprador.Cidade.IdUf);
            CompradoresDTO dto = _mapper.Map<CompradoresDTO>(comprador);
            if (comprador == null)
                return NotFound();

            return Ok(dto);
        }

        [HttpPut("{id}", Name = "Alterar")]
        public IActionResult AlterarComprador(int id, CompradoresDTO comprador)
        {
            if (id != comprador.Id)
                return BadRequest("ID da rota difere do ID do comprador.");

            var entidade = _mapper.Map<Comprador>(comprador);
            _compradorService.SalvarComprador(entidade);
            comprador.CidadeNome = _cidadeService.SelecionaCidade(comprador.IdCidade).Nome;
            comprador.EstadoNome = _estadoService.SelecionaEstado(comprador.IdEstado).Nome;
            return Ok(comprador);
        }

        [HttpPost(Name = "Incluir")]
        public IActionResult IncluirComprador(CompradoresDTO comprador)
        {
            var entidade = _mapper.Map<Comprador>(comprador);

            _compradorService.SalvarComprador(entidade);
            return CreatedAtRoute("Selecionar", new { id = entidade.Id }, comprador);
        }

        [HttpDelete("{id}", Name = "Excluir")]
        public IActionResult ExcluirComprador(int id)
        {
            var comprador = _compradorService.SelecionaComprador(id);
            if (comprador == null)
                return NotFound();

            _compradorService.ExcluirComprador(id);
            return NoContent();
        }
    }
}
