using AutoMapper;
using EstudoReact.Model;
using EstudoReact.Server.DTO;
using EstudoReact.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace EstudoReact.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadoController : ControllerBase
    {
        private readonly EstadoService _estadoService;
        private readonly IMapper _mapper;
        public EstadoController(EstadoService estadoService, IMapper mapper)
        {
            _estadoService = estadoService;
            _mapper = mapper;
        }

        // GET: api/Estado
        [HttpGet]
        public async Task<ActionResult<List<Estado>>> ListaEstados()
        {
            List<Estado> estados = await _estadoService.ListaEstados();
            List<EstadoDTO> lstEstados = _mapper.Map<List<EstadoDTO>>(estados);
            
            return Ok(lstEstados);
        }

        // GET: api/Estado/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Estado>> SelecionaEstado(int id)
        {
            try
            {
                Estado estado = await _estadoService.SelecionaEstado(id);
                EstadoDTO dto = _mapper.Map<EstadoDTO>(estado);
                return Ok(dto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
