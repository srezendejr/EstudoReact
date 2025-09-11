using AutoMapper;
using EstudoReact.Model;
using EstudoReact.Server.DTO;
using EstudoReact.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Estudo.UI.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompradorController : ControllerBase
    {
        private readonly CompradorService _compradorService;
        private readonly CidadeService _cidadeService;
        private readonly EstadoService _estadoService;
        private readonly IMapper _mapper;
        public CompradorController(CompradorService compradorService,
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
        public async Task<ActionResult<IEnumerable<CompradoresDTO>>> SelecionaCompradores()
        {
            List<Comprador> compradores = await _compradorService.ListaCompradores();
            foreach (Comprador item in compradores)
            {
                item.Cidade = await _cidadeService.SelecionaCidade(item.IdCidade);
                item.Cidade.UF = await _estadoService.SelecionaEstado(item.Cidade.IdUf);
            }
            List<CompradoresDTO> dtos = _mapper.Map<List<CompradoresDTO>>(compradores);
            return Ok(dtos);
        }

        [HttpGet("{id}", Name = "Selecionar")]
        public async Task<ActionResult<CompradoresDTO>> SelecionarComprador(int id)
        {
            Comprador comprador = await _compradorService.SelecionaComprador(id);
            if (comprador == null)
                return NotFound();

            comprador.Cidade = await _cidadeService.SelecionaCidade(comprador.IdCidade);
            comprador.Cidade.UF = await _estadoService.SelecionaEstado(comprador.Cidade.IdUf);
            CompradoresDTO dto = _mapper.Map<CompradoresDTO>(comprador);
            return Ok(dto);
        }

        [HttpPut("{id}", Name = "Alterar")]
        public async Task<IActionResult> AlterarComprador(int id, CompradoresDTO comprador)
        {
            if (id != comprador.Id)
                return BadRequest("ID da rota difere do ID do comprador.");

            var entidade = _mapper.Map<Comprador>(comprador);
            await _compradorService.SalvarComprador(entidade);
            Cidade cidade = await _cidadeService.SelecionaCidade(comprador.IdCidade);
            Estado estado = await _estadoService.SelecionaEstado(comprador.IdEstado);
            comprador.CidadeNome = cidade?.Nome;
            comprador.EstadoNome = estado?.Nome;

            return Ok(comprador);
        }

        [HttpPost(Name = "Incluir")]
        public async Task<IActionResult> IncluirComprador(CompradoresDTO comprador)
        {

            var entidade = _mapper.Map<Comprador>(comprador);

            await _compradorService.SalvarComprador(entidade);
            return CreatedAtRoute("Selecionar", new { id = entidade.Id }, comprador);
        }

        [HttpDelete("{id}", Name = "Excluir")]
        public async Task<IActionResult> ExcluirComprador(int id)
        {
            var comprador = _compradorService.SelecionaComprador(id);
            if (comprador == null)
                return NotFound();

            await _compradorService.ExcluirComprador(id);
            return NoContent();
        }
    }
}
