using EstudoReact.Model;
using EstudoReact.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace EstudoReact.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadoController : ControllerBase
    {
        private readonly EstadoService _estadoService;

        public EstadoController(EstadoService estadoService)
        {
            _estadoService = estadoService;
        }

        // GET: api/Estado
        [HttpGet]
        public async Task<ActionResult<List<Estado>>> ListaEstados()
        {
            var estados = await _estadoService.ListaEstados();
            return Ok(estados);
        }

        // GET: api/Estado/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Estado>> SelecionaEstado(int id)
        {
            try
            {
                var estado = await _estadoService.SelecionaEstado(id);
                return Ok(estado);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
