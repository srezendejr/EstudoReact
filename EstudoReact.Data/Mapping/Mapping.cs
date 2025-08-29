using EstudoReact.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudoReact.Data
{
    public class Mapping
    {
        public static void Map(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Estado>()
                .ToTable("Estado")
                .HasKey(k => k.Id);
            modelBuilder.Entity<Estado>().Property(p => p.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Cidade>()
                .ToTable("Cidade")
                .HasKey(k => k.Id);
            modelBuilder.Entity<Cidade>().Property(p => p.Id).ValueGeneratedOnAdd();


            modelBuilder.Entity<Comprador>()
                .ToTable("Comprador")
                .HasKey(k => k.Id);
            modelBuilder.Entity<Comprador>().Property(p => p.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Produto>()
                .ToTable("Produto")
                .HasKey(k => k.Id);
            modelBuilder.Entity<Produto>().Property(p => p.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Pedido>()
                .ToTable("Pedido")
                .HasKey(k => k.Id);
            modelBuilder.Entity<Pedido>().Property(p => p.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<ItemPedido>()
                .ToTable("ItemPedido")
                .HasKey(k => new { k.IdPedido, k.Item });
            modelBuilder.Entity<ItemPedido>().Property(p => p.Item).ValueGeneratedOnAdd();
            modelBuilder.Entity<ItemPedido>()
            .Property(p => p.Valor)
            .HasPrecision(18, 2);

            modelBuilder.Entity<Cidade>()
                .HasOne(b => b.UF) 
                .WithMany(a => a.Cidades) 
                .HasForeignKey(b => b.IdUf).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Comprador>()
                .HasOne(b => b.Cidade)
                .WithMany(a => a.Compradores)
                .HasForeignKey(b => b.IdCidade).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Pedido>()
                .HasOne(b => b.Comprador)
                .WithMany(a => a.Pedidos)
                .HasForeignKey(b => b.IdComprador).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
