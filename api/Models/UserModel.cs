namespace BankingSystem.Models;

public class UserModel
{
    /// <summary>
    /// Ideally we need to use Id everywhere but to simplify swagger usage, we will mostly use email in this example
    /// </summary>
    public Guid Id { get; private set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public List<UserAccountModel> Accounts { get; set; } = new();

    public UserModel(Guid id, string name, string email)
    {
        Id = id;
        Name = name;
        Email = email;
    }
    
    public UserModel(string name, string email) : this(Guid.NewGuid(), name, email)
    {
    }
}
