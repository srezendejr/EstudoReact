using EstudoReact.Model;
using EstudoReact.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace EstudoReact.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CidadeController : ControllerBase
    {
        private readonly CidadeService _cidadeService;

        public CidadeController(CidadeService cidadeService)
        {
            _cidadeService = cidadeService;
        }

        // GET: api/Cidade
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cidade>>> ListaCidades()
        {
            var cidades = await _cidadeService.ListaCidades();
            return Ok(cidades);
        }

        // GET: api/Cidade/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Cidade>> SelecionaCidade(int id)
        {
            try
            {
                var cidade = await _cidadeService.SelecionaCidade(id);
                return Ok(cidade);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // GET: api/Cidade/por-uf/3
        [HttpGet("por-uf/{idUf:int}")]
        public async Task<ActionResult<List<Cidade>>> SelecionaCidadePorUf(int idUf)
        {
            var cidades = await _cidadeService.SelecionaCidadePorUf(idUf);
            return Ok(cidades);
        }
    }
}
