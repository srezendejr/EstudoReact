using EstudoReact.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudoReact.Data
{
    public class Context:DbContext
    {
        public Context()
        { }
        public Context(DbContextOptions<Context> options) : base(options)
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        {

            optionsBuilder.UseSqlServer("Server=localhost;Database=EstudoReact;Integrated Security=True;TrustServerCertificate=True;");

        }
        public virtual DbSet<Produto> Produtos { get; set; }
        public virtual DbSet<Comprador> Compradores { get; set; }
        public virtual DbSet<Pedido> Pedidos { get; set; }
        public virtual DbSet<ItemPedido> ItensPedido { get; set; }
        public virtual DbSet<Cidade> Cidades { get; set; }
        public virtual DbSet<Estado> Estados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Mapping.Map(modelBuilder);
        }

        public async Task CommitAsync()
        {
            await base.SaveChangesAsync();
        }
        public void Commit()
        {
            base.SaveChanges();
        }

        public void Salvar<T>(T entity) where T : class
        {
            Set<T>().Attach(entity);
            Entry(entity).State = EntityState.Added;
        }

        public void Alterar<T>(T entity) where T : class
        {
            Set<T>().Attach(entity);
            Entry(entity).State = EntityState.Modified;
        }

        public void Excluir<T>(T entity) where T : class
        {
            Set<T>().Attach(entity);
            Entry(entity).State = EntityState.Deleted;
        }
    }
}
