using EstudoReact.Data;
using EstudoReact.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudoReact.Service
{
    public class CidadeService
    {
        Context _context;

        public CidadeService()
        {
            _context = new Context();
        }

        public List<Cidade> ListaCidades()
        {
            return _context.Cidades.ToList();
        }

        public Cidade SelecionaCidade(int id)
        {
            return _context.Cidades.FirstOrDefault(f => f.Id == id);
        }

        public List<Cidade> SelecionaCidadePorUf(int id)
        {
            return _context.Cidades.Where(w => w.IdUf == id).ToList();
        }
    }
}
