using EstudoReact.Model;
using EstudoReact.Service;
using Microsoft.AspNetCore.Mvc;

namespace EstudoReact.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class EstadoController : ControllerBase
    {
        EstadoService _estadoService;
        public EstadoController()
        {
            _estadoService = new EstadoService();
        }

        [HttpGet(Name ="ListaEstados")]
        public List<Estado> ListaEstados()
        {
            return _estadoService.ListaEstados();
        }

        [HttpGet("{id}", Name ="SelecionaEstado")]
        public Estado SelecionaEstado(int id)
        {
            return _estadoService.SelecionaEstado(id);
        }
    }
}
