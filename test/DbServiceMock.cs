using System.Data;
using BankingSystem.Models;
using BankingSystem.Services;

namespace BankingSystem.Tests;

public class DbServiceMock : IDbService
{
    public readonly List<UserModel> Users;

    public DbServiceMock()
    {
        Users = new List<UserModel> {new("User1", "Email1"), new("User2", "Email2")};
        Users[0].Accounts.Add(new UserAccountModel {AccountNumber = "123", Amount = 10_000});
        Users[0].Accounts.Add(new UserAccountModel {AccountNumber = "456", Amount = 100});
        Users[1].Accounts.Add(new UserAccountModel {AccountNumber = "789"});
        Users[1].Accounts.Add(new UserAccountModel {AccountNumber = "012"});
    }
    public IEnumerable<UserModel> GetUsers()
    {
        return Users;
    }

    public UserModel? GetUser(string email)
    {
        return Users.FirstOrDefault(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }

    public UserModel? GetUser(Guid id)
    {
        return Users.FirstOrDefault(x => x.Id == id);
    }

    public UserModel AddUser(string name, string email)
    {
        if (GetUser(email) is not null)
            throw new DuplicateNameException($"Duplicate email {email}");
        
        Users.Add(new UserModel(name, email));
        return Users.Last();
    }

    public UserModel UpdateUser(UserModel model)
    {
        var user = GetUser(model.Id);
        if (user is null)
            throw new KeyNotFoundException($"User {model.Id} not found");

        if (!user.Email.Equals(model.Email, StringComparison.OrdinalIgnoreCase) &&
            Users.FirstOrDefault(x => x.Email.Equals(model.Email, StringComparison.OrdinalIgnoreCase) && x.Id != model.Id) is not null)
        {
            throw new DuplicateNameException($"Email {model.Email} already registered");
        }

        user.Name = model.Name;
        user.Email = model.Email;

        return user;
    }

    public void DeleteUser(string email)
    {
        var index = Users.FindIndex(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        
        if (index < 0)
            throw new KeyNotFoundException($"Email {email} not found");
    }

    public IEnumerable<UserAccountModel> GetUserAccounts(string email)
    {
        var user = GetUser(email);
        if (user is null)
            throw new KeyNotFoundException($"User {email} not found");

        return user.Accounts;
    }

    public UserAccountModel? GetUserAccount(string email, string accNum)
    {
        var user = GetUser(email);
        if (user is null)
            throw new KeyNotFoundException($"User {email} not found");

        return user.Accounts.FirstOrDefault(x => x.AccountNumber == accNum);
    }

    public UserAccountModel? GetUserAccount(UserModel user, string accNum)
    {
        return user.Accounts.FirstOrDefault(x => x.AccountNumber == accNum);
    }

    public UserAccountModel AddUserAccount(string email, UserAccountModel account)
    {
        var user = GetUser(email);
        
        if (user is null)
            throw new KeyNotFoundException($"User {email} not found");
        
        if (user.Accounts.FirstOrDefault(x => x.AccountNumber == account.AccountNumber) is not null)
            throw new DuplicateNameException($"Account {account.AccountNumber} already registered");
            
        user.Accounts.Add(account);
        return account;
    }

    public void DeleteUserAccount(string email, string accNum)
    {
        var user = GetUser(email);
        
        if (user is null)
            throw new KeyNotFoundException($"User {email} not found");
        
        var account = GetUserAccount(user, accNum);

        if (account is null)
            throw new KeyNotFoundException($"Account {accNum} not found");

        user.Accounts.Remove(account);
    }
}
