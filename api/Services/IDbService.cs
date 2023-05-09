using BankingSystem.Models;

namespace BankingSystem.Services;

public interface IDbService
{
    IEnumerable<UserModel> GetUsers();
    UserModel? GetUser(string email);
    UserModel? GetUser(Guid id);
    UserModel AddUser(string name, string email);
    UserModel UpdateUser(UserModel model);
    void DeleteUser(string email);

    IEnumerable<UserAccountModel> GetUserAccounts(string email);
    UserAccountModel? GetUserAccount(string email, string accNum);
    UserAccountModel? GetUserAccount(UserModel user, string accNum);
    UserAccountModel AddUserAccount(string email, UserAccountModel account);
    void DeleteUserAccount(string email, string accNum);
}
