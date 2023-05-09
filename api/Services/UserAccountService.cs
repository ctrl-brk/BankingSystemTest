using BankingSystem.Exceptions;
using BankingSystem.Models;

namespace BankingSystem.Services;

public class UserAccountService : IUserAccountService
{
    public UserAccountModel CreateAccount(string accountNumber)
    {
        return new UserAccountModel { AccountNumber = accountNumber, Amount = 0 };
    }
    
    public UserAccountModel Deposit(UserAccountModel account, double amount)
    {
        if (amount is <= 0 or > 10_000)
            throw new AccountException("Amount must be greater than zero and no more than 10,000");

        account.Amount += amount;

        return account;
    }

    public UserAccountModel Withdraw(UserAccountModel account, double amount)
    {
        if (amount <= 0)
            throw new AccountException("Amount must be greater than zero");
    
        if (amount > account.Amount * 0.9)
            throw new AccountException("Cannot withdraw more than 90%");
        
        if (account.Amount - amount < 100)
            throw new AccountException("Account balance after withdrawal cannot be less than 100");

        account.Amount -= amount;

        return account;
    }
}
