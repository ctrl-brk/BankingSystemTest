using BankingSystem.Models;

namespace BankingSystem.Services;

public interface IUserAccountService
{
    UserAccountModel CreateAccount(string accountNumber);
    UserAccountModel Deposit(UserAccountModel account, double amount);
    UserAccountModel Withdraw(UserAccountModel account, double amount);
}
