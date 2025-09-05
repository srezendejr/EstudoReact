using EstudoReact.Model;
using EstudoReact.Data;

namespace EstudoReact.Services
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
                    _context.Salvar<Comprador>(comprador);
                else
                    _context.Alterar<Comprador>(comprador);

                _context.Commit();
            }
            catch
            {
                throw;
            }
        }
        public Comprador SelecionaComprador(int id)
        {
            return _context.Compradores.FirstOrDefault(f => f.Id == id);
        }
        public List<Comprador> ListaCompradores()
        {
            return _context.Compradores.ToList();
        }

        public Comprador ObterCompradorPorId(int id)
        {
            return _context.Compradores.FirstOrDefault(c => c.Id == id);
        }

        public void ExcluirComprador(int id)
        {
            try
            {
                var comprador = ObterCompradorPorId(id) ?? throw new Exception("Comprador não encontrado");

                ValidarExcluirComprador(comprador);

                _context.Excluir<Comprador>(comprador);
                _context.Commit();
            }
            catch
            {
                throw;
            }
        }

        private void ValidarExcluirComprador(Comprador comprador)
        {
            var pedidos = _context.Pedidos.Where(w => w.IdComprador == comprador.Id).ToList();
            if (pedidos.Any())
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
        }

        private bool DocumentoValido(string documento)
        {
            return (!string.IsNullOrEmpty(documento)) &&
                   ((documento.Length == 11 && Util.Util.IsCpf(documento)) ||
                    (documento.Length == 14 && Util.Util.IsCnpj(documento)));
        }
    }
}
