using Estudo.Model.Enum;
using EstudoReact.Model;
using EstudoReact.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EstudoReact.Service.Services
{
    public class ProdutoService
    {
        Context _context;
        public ProdutoService()
        {
            _context = new Context();
        }

        public async Task SalvarProduto(Produto produto)
        {
            try
            {
                if (ValidarProduto(produto))
                {
                    if (produto.Id == 0)
                        _context.Salvar(produto);
                    else
                        _context.Alterar(produto);
                    await _context.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task ExcluirProduto(int id)
        {
            try
            {
                Produto produto = await SelecionaProduto(id);
                if (ValidarExcluirProduto(produto))
                {
                    _context.Excluir(produto);
                    await _context.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Produto>> ListarProduto()
        {
            return await _context.Produtos.ToListAsync();
        }

        public async Task<Produto> SelecionaProduto(int id)
        {
            return await _context.Produtos.FindAsync(id);
        }

        private bool ValidarExcluirProduto(Produto produto)
        {
            bool validado = true;
            var listProdutoPedido = _context.ItensPedido.Where(w => w.IdProduto == produto.Id).ToList();
            if (listProdutoPedido.Count > 0)
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

            if (string.IsNullOrEmpty(produto.Nome))
            {
                valida = false;
                throw new Exception("Informe um nome válido");
            }

            if (!Enum.IsDefined(typeof(OrigemProduto), produto.Origem))
            {
                valida = false;
                throw new Exception("Informe uma origem válido");
            }

            return valida;
        }
    }
}
