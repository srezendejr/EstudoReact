using EstudoReact.Data;
using EstudoReact.Model;
using Microsoft.EntityFrameworkCore;

namespace EstudoReact.Service.Services
{
    public class EstadoService
    {
        private readonly Context _context;

        public EstadoService(Context context)
        {
            _context = context;
        }

        public async Task<List<Estado>> ListaEstados()
        {
            return await _context.Estados.ToListAsync();
        }

        public async Task<Estado> SelecionaEstado(int id)
        {
            return await _context.Estados.FindAsync(id);
        }
    }
}
