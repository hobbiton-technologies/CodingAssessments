using Microsoft.EntityFrameworkCore;

namespace CodingAssessment;

public class SqliteDbContext : DbContext
{
    public SqliteDbContext(DbContextOptions<SqliteDbContext> options) : base(options)
    {
    }

    public DbSet<Package> Packages => Set<Package>();
    public DbSet<Benefit> Benefits => Set<Benefit>();
}



public class PostgresDbContext : DbContext
{
    public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users => Set<User>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
}