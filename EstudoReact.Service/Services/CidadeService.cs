using EstudoReact.Data;
using EstudoReact.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudoReact.Service.Services
{
    public class CidadeService
    {
       private readonly Context _context;

        public CidadeService(Context context )
        {
            _context = context;
        }

        public async Task<List<Cidade>> ListaCidades()
        {
            return await _context.Cidades.AsNoTracking().ToListAsync();
        }

        public async Task<Cidade> SelecionaCidade(int id)
        {
            var cidade = await _context.Cidades.FindAsync(id);
            if (cidade == null)
                throw new KeyNotFoundException($"Cidade com Id {id} não encontrada.");
            return cidade;
        }

        public async Task<List<Cidade>> SelecionaCidadePorUf(int id)
        {
            return await _context.Cidades.Where(w => w.IdUf == id).ToListAsync();
        }
    }
}
