using Microsoft.EntityFrameworkCore;

namespace CodingAssessment;



public class DatabaseContext: DbContext
{
    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Package> Packages => Set<Package>();
    public DbSet<Benefit> Benefits => Set<Benefit>();
}
