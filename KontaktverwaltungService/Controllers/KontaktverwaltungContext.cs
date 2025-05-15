using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using KontaktverwaltungLibrary.Models;


namespace KontaktverwaltungService
{
    public class KontaktverwaltungContext : DbContext
    {
        public DbSet<Kontakt> Kontakt { get; set; }

        public KontaktverwaltungContext(DbContextOptions options) : base(options)
        { }
    }
}
