using EstudoReact.Model;
using EstudoReact.Data;
using Microsoft.EntityFrameworkCore;
using EstudoReact.Util;

namespace EstudoReact.Service.Services
{
    public class CompradorService
    {
        private readonly Context _context;

        public CompradorService(Context context)
        {
            _context = context;
        }

        public async Task SalvarComprador(Comprador comprador)
        {
            try
            {
                ValidarComprador(comprador);

                if (comprador.Id == 0)
                    _context.Salvar(comprador);
                else
                    _context.Alterar(comprador);

                _context.Commit();
            }
            catch
            {
                throw;
            }
        }
        public async Task<Comprador> SelecionaComprador(int id)
        {
            return await _context.Compradores.FindAsync(id);
        }
        public async Task<List<Comprador>> ListaCompradores()
        {
            return await _context.Compradores.ToListAsync();
        }

        public async Task<Comprador> ObterCompradorPorId(int id)
        {
            return await _context.Compradores.FindAsync(id);
        }

        public async Task ExcluirComprador(int id)
        {
            try
            {
                Comprador comprador = await ObterCompradorPorId(id) ?? throw new Exception("Comprador não encontrado");

                await ValidarExcluirComprador(comprador);

                _context.Excluir(comprador);
                _context.Commit();
            }
            catch
            {
                throw;
            }
        }

        private async Task ValidarExcluirComprador(Comprador comprador)
        {
            var pedidos = await _context.Pedidos.FindAsync(comprador.Id);
            if (pedidos == null)
                throw new Exception("Não é possível excluir o comprador, pois há pedidos para ele.");
        }

        private void ValidarComprador(Comprador comprador)
        {
            if (comprador == null)
                throw new Exception("Comprador inválido");

            if (comprador.IdCidade == 0)
                throw new Exception("Informe uma cidade válida para o comprador");

            if (string.IsNullOrWhiteSpace(comprador.Nome))
                throw new Exception("Informe um nome para o comprador");

            if (!DocumentoValido(comprador.Documento))
                throw new Exception("Informe um documento válido");

            if (_context.Compradores.Any(a => a.Documento == comprador.Documento))
                throw new Exception("Já existe um comprador com este documento");
        }

        private bool DocumentoValido(string documento)
        {
            return !string.IsNullOrEmpty(documento) &&
                   (documento.Length == 11 && Util.Util.IsCpf(documento) ||
                    documento.Length == 14 && Util.Util.IsCnpj(documento));
        }
    }
}
