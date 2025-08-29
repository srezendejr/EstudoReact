using EstudoReact.Model;
using EstudoReact.Data;

namespace EstudoReact.Services
{
    public class CompradorService
    {
        Context _context;

        public CompradorService()
        {
            _context = new Context();
        }

        public void SalvarComprador(Comprador comprador)
        {
            try
            {
                if (ValidaComprador(comprador))
                {
                    _context.Salvar<Comprador>(comprador);
                    _context.Commit().GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Comprador> ListaCompradores()
        {
            return _context.Compradores.ToList();
        }

        public Comprador SelecionaComprador(int id)
        {
            return _context.Compradores.FirstOrDefault(c => c.Id == id);
        }
        public void ExcluirComprador(int id)
        {
            try
            {
                Comprador comprador = SelecionaComprador(id);
                if (ValidarExcluirComprador(comprador))
                {
                    _context.Excluir<Comprador>(comprador);
                    _context.Commit().GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool ValidarExcluirComprador(Comprador comprador)
        {
            bool validado = true;
            var lstComprador = _context.Pedidos.Where(w => w.IdComprador == comprador.Id).ToList();
            if (lstComprador.Count > 0)
            {
                validado = false;
                throw new Exception("Não é possível excluir o comprador, pois há pedido para ele.");
            }
            return validado;
        }
        private bool ValidaComprador(Comprador comprador)
        {
            bool valida = true;
            if (comprador == null)
            {
                valida = false;
                throw new Exception("Comprador inválido");
            }

            if (comprador.IdCidade == 0)
            {
                valida = false;
                throw new Exception("Informe uma cidade válida para o comprador");
            }

            if (string.IsNullOrEmpty(comprador.Nome))
            {
                valida = false;
                throw new Exception("Informe um nome para o comprador");
            }

            if (string.IsNullOrEmpty(comprador.Documento)
                || !Util.Util.IsCpf(comprador.Documento)
                || !Util.Util.IsCnpj(comprador.Documento)
               )
            {
                valida = false;
                throw new Exception("Informe um documento válido");
            }
            return valida;
        }
    }
}
