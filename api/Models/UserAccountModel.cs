namespace BankingSystem.Models;

/// <summary>
/// We'll take a different approach here without any ID (you shouldn't do this in real life!) just because we can
/// </summary>
public class UserAccountModel
{
    public string AccountNumber { get; set; } = null!;
    public double Amount { get; set; }
}
