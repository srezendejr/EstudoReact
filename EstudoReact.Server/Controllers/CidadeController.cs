using EstudoReact.Model;
using EstudoReact.Service;
using EstudoReact.Services;
using Microsoft.AspNetCore.Mvc;

namespace EstudoReact.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CidadeController : ControllerBase
    {
        CidadeService _cidadeService;
        public CidadeController()
        {
            _cidadeService = new CidadeService();
        }

        [HttpGet(Name = "Cidades")]
        public IEnumerable<Cidade> ListaCidades()
        {
            return _cidadeService.ListaCidades();
        }

        [HttpGet("{id}", Name = "Cidade")]
        public Cidade SelecionaCidades(int id)
        {
            return _cidadeService.SelecionaCidade(id);
        }

        [HttpGet("{idUf}/Cidade", Name = "CidadePorUF")]
        public List<Cidade> SelecionaCidadePorUf(int idUf)
        {
            return _cidadeService.SelecionaCidadePorUf(idUf);
        }
    }
}
