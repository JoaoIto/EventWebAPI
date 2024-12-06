using Microsoft.EntityFrameworkCore;
using EventWebAPI.Models;
using System.Collections.Generic;

namespace EventWebAPI.Data
{
    
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Participacao> Participacoes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }

}
