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

        public DbSet<LlamadaTicket> LlamadaTicket { get; set; }

        public DbSet<SIS_Usuarios> SIS_Usuarios { get; set; }

        public DbSet<OrdenPrioridadTicket> OrdenPrioridadTicket { get; set; }

        public DbSet<Escritorio> Escritorio { get; set; }

        public DbSet<ProgramarIndisponibilidad> ProgramarIndisponibilidad { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pagaduria> ().HasKey(k => new { k.CodigoPagaduria });
            modelBuilder.Entity<ControlTicket>().HasKey(k => new { k.IdControlTicket });
            modelBuilder.Entity<Unidades>().HasKey(k => new { k.CodigoUnidades });
            modelBuilder.Entity<TipoDeFila>().HasKey(k => new { k.IdFila });
            modelBuilder.Entity<Ticket>().HasKey(k => new { k.IdTicket });
            modelBuilder.Entity<LlamadaTicket>().HasKey(k => new { k.IdLlamadaTicket });
            modelBuilder.Entity<SIS_Usuarios>().HasKey(k => new { k.CodigoUsuario });
            modelBuilder.Entity<OrdenPrioridadTicket>().HasKey(k => new { k.IdOrden });
            modelBuilder.Entity<Escritorio>().HasKey(k => new { k.IdEscritorio });
            modelBuilder.Entity<OrdenPrioridadTicket>().ToTable(tb => tb.HasTrigger("RedirigirUnidad"));
            modelBuilder.Entity<ProgramarIndisponibilidad>().HasKey(k => new { k.IdProgramarIndiponibilidad });
        }
    }
}
