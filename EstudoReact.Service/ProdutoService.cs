using Estudo.Model.Enum;
using EstudoReact.Model;
using EstudoReact.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudoReact.Services
{
    public class ProdutoService
    {
        Context _context;
        public ProdutoService() {
            _context = new Context();
        }

        public void SalvarProduto(Produto produto)
        {
            try
            {
                if (ValidarProduto(produto))
                {
                    _context.Salvar<Produto>(produto);
                    _context.Commit().GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void ExcluirProduto(int id)
        {
            try
            {
                Produto produto = SelecionaProduto(id);
                if (ValidarExcluirProduto(produto))
                {
                    _context.Excluir<Produto>(produto);
                    _context.Commit().GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Produto> ListarProduto()
        {
            return _context.Produtos.ToList();
        }

        public Produto SelecionaProduto(int id)
        {
            return _context.Produtos.FirstOrDefault(x => x.Id == id);
        }

        private bool ValidarExcluirProduto(Produto produto)
        {
            bool validado = true;
            var listProdutoPedido = _context.ItensPedido.Where(w => w.IdProduto == produto.Id).ToList();
            if(listProdutoPedido.Count > 0)
            {
                validado = false;
                throw new Exception("Não é possível excluir o produto, pois há pedido para ele.");
            }
            return validado;
        }

        private bool ValidarProduto(Produto produto)
        {
            bool valida = true;
            if (produto == null)
            {
                valida = false;
                throw new Exception("Produto inválido");
            }

            if (produto.Id == 0)
            {
                valida = false;
                throw new Exception("Informe um Id válido");
            }

            if (string.IsNullOrEmpty(produto.Nome))
            {
                valida = false;
                throw new Exception("Informe um nome válido");
            }

            if (Enum.IsDefined(typeof(OrigemProduto), produto.Origem))
            {
                valida = false;
                throw new Exception("Informe uma origem válido");
            }

            return valida;
        }
    }
}
