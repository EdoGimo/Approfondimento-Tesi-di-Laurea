using System.Data.Entity;

namespace Geolocalizzazione.Models
{
    public class PGDbContext : DbContext
    {
        public PGDbContext() : base(nameOrConnectionString: "DefaultConnectionString") { }
        public virtual DbSet<Indirizzo> Ind { get; set; }
    }
}