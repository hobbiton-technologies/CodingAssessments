using Microsoft.EntityFrameworkCore;

namespace CodingAssessment.Users;

public class UserService
{

    private readonly PostgresDbContext _postgresDbContext;

    public UserService(PostgresDbContext postgresDbContext)
    {
        _postgresDbContext = postgresDbContext;
    }

    public async Task<string> CreateUser(string email)
    {
        var existing = await _postgresDbContext.Users.FirstOrDefaultAsync(x => x.Email == email);

        if (existing != null)
        {
            existing.ApiKey = Guid.NewGuid().ToString();
            await _postgresDbContext.SaveChangesAsync();
            return existing.ApiKey;
        }


        var user = new User
        {
            Email = email,
            UserName = email,
            ApiKey = Guid.NewGuid().ToString()
        };

        _postgresDbContext.Users.Add(user);
        await _postgresDbContext.SaveChangesAsync();

        return user.ApiKey;
    }

    public async Task<string> GetApiKey(string email)
    {
        var user = await _postgresDbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
        if (user == null)
        {
            throw new Exception($"User with email {email} not found");
        }

        return user.ApiKey;
    }


    public async Task<User> GetUser(string apiKey)
    {
        var user = await _postgresDbContext.Users.FirstOrDefaultAsync(x => x.ApiKey == apiKey);
        if (user == null)
        {
            throw new Exception($"User with api key {apiKey} not found");
        }

        return user;
    }

}