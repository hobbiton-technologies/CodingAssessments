using CodingAssessment.Users;
using Microsoft.EntityFrameworkCore;

namespace CodingAssessment;

public class PostgresDbContext : DbContext
{
    public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    public DbSet<Package> Packages => Set<Package>();
    public DbSet<Benefit> Benefits => Set<Benefit>();


    public async Task<string> Seed()
    {
        if (await Packages.AnyAsync() && await Users.AnyAsync() && await Transactions.AnyAsync() && await Benefits.AnyAsync())
        {
            return "Database already seeded";
        }


        var package = new Package
        {
            Name = "Basic Travel Insurance",
            Description = "This is a basic travel insurance package that provides coverage for medical emergencies and trip cancellations.",
            Premium = 50.99,
            SupportingDocumentUrl = "https://www.africau.edu/images/default/sample.pdf",
            Benefits = new List<Benefit>
            {
                new()
                {
                    Name = "Medical Emergency Coverage",
                    Description =
                        "This benefit provides coverage for medical emergencies that occur while traveling, including hospital stays and emergency medical transportation."
                },
                new()
                {
                    Name = "Trip Cancellation Coverage",
                    Description = "This benefit provides coverage for trip cancellations due to medical emergencies, natural disasters, and other covered reasons."
                }
            }
        };


        Packages.Add(package);

        var users = new List<string> { "Vincent", "Paul", "Mulenga", "Kembo", "Situmbeko" }.Select(x => new User
        {
            UserName = x,
            Email = $"{x}@gmail.com",
            ApiKey = Guid.NewGuid().ToString(),
        });

        Users.AddRange(users);

        await SaveChangesAsync();

        var transactions = Enumerable.Range(1, 500_000).Select(x => new Transaction
        {
            Amount = new Random().Next(1, 1000),
            Date = RandomDateBetween(new DateTime(2019, 1, 1), new DateTime(2022, 1, 1)).Date,
            UserId = new Random().Next(1, 5),
            Type = new Random().Next(1, 3) == 1 ? "deposit" : "withdraw",
        });

        await Transactions.AddRangeAsync(transactions);

        await SaveChangesAsync();


        DateTime RandomDateBetween(DateTime start, DateTime end)
        {
            var range = end - start;
            var randTimeSpan = new TimeSpan((long)(new Random().NextDouble() * range.Ticks));
            return start + randTimeSpan;
        }

        return "Done";
    }
}