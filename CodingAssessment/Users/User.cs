namespace CodingAssessment.Users;

public class User
{
    public int Id { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }

    public ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();

    public required string ApiKey { get; set; }
}

public class Transaction
{
    public int Id { get; set; }
    public required int UserId { get; set; }
    public double Amount { get; set; }
    public required string Type { get; set; }
    public required DateTime Date { get; set; }
}