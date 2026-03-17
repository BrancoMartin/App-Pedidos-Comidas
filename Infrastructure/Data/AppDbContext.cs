using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Carrito> Carrito { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemCarrito> ItemsCarrito { get; set; }
        public DbSet<ItemPedido> ItemsPedido { get; set; }
        public DbSet<MetodoPago> MetodoPagos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Usuario>().HasData(
            //    new Usuario { Id = 99, Nombre = "Admin", Apellido = "Db", Contraseña = "secreti123", Telefono = "5493416203456", Rol = 0});

            modelBuilder.Entity<Carrito>()
                .HasOne(c => c.Usuario)
                .WithMany(u => u.Carritos)
                .HasForeignKey(c => c.UsuarioId);

            modelBuilder.Entity<Categoria>()
                .HasMany(c => c.Productos)
                .WithOne(p => p.Categoria)
                .HasForeignKey(p => p.CategoriaId);

            modelBuilder.Entity<ItemCarrito>()
                .HasOne(ic => ic.Carrito)
                .WithMany(c => c.Items)
                .HasForeignKey(ic => ic.CarritoId);

            modelBuilder.Entity<ItemPedido>()
                .Property(ic => ic.PrecioUnitario)
                .HasPrecision(18, 2);

            modelBuilder.Entity<ItemPedido>()
                .HasOne(ip => ip.Pedido)
                .WithMany(p => p.ItemsPedido)
                .HasForeignKey(ip => ip.PedidoId);

            modelBuilder.Entity<MetodoPago>()
                .HasMany(mp => mp.Pagos)
                .WithOne(p => p.MetodoPago)
                .HasForeignKey(p => p.MetodoPagoId);

            modelBuilder.Entity<Pago>()
                .HasOne(p => p.Pedido)
                .WithMany(pe => pe.Pagos)
                .HasForeignKey(p => p.PedidoId);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.PrecioTotal)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Usuario)
                .WithMany(u => u.Pedidos)
                .HasForeignKey(p => p.UsuarioId);

            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Producto>()
                .HasMany(p => p.ItemsCarrito)
                .WithOne(ic => ic.Producto)
                .HasForeignKey(ic => ic.ProductoId);

            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Carritos)
                .WithOne(c => c.Usuario)
                .HasForeignKey(c => c.UsuarioId);


        }
    }
}

