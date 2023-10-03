using Microsoft.EntityFrameworkCore;
using WebTicket.ViewModel;

namespace WebTicket.Concrete
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Pagaduria> Pagaduria { get; set;}
        public DbSet<ControlTicket> ControlTicket { get; set; }
        public DbSet<Unidades> Unidades { get; set; }
        public DbSet<TipoDeFila> TipoDeFila { get; set; }

        public DbSet<Ticket> Ticket { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pagaduria> ().HasKey(k => new { k.CodigoPagaduria });
            modelBuilder.Entity<ControlTicket>().HasKey(k => new { k.IdControlTicket });
            modelBuilder.Entity<Unidades>().HasKey(k => new { k.CodigoUnidades });
            modelBuilder.Entity<TipoDeFila>().HasKey(k => new { k.IdFila });
            modelBuilder.Entity<Ticket>().HasKey(k => new { k.IdTicket });

        }
    }
}
