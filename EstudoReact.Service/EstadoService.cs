using EstudoReact.Data;
using EstudoReact.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudoReact.Service
{
    public class EstadoService
    {
        Context _context;

        public EstadoService()
        {
            _context = new Context();
        }

        public List<Estado> ListaEstados()
        {
            return _context.Estados.ToList();
        }

        public Estado SelecionaEstado(int id)
        {
            return _context.Estados.FirstOrDefault(f => f.Id == id);
        }
    }
}
