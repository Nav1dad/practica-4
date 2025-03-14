using Microsoft.EntityFrameworkCore;
using Practica4.Models;

namespace Practica4.Data
{
    public class AgendaDbContext : DbContext
    {
        public AgendaDbContext(DbContextOptions<AgendaDbContext> options) : base(options)
        { 
        }

        public DbSet<Estado> Estados { get; set; }
        public DbSet<Evento> Eventos { get; set; }
    }
}
