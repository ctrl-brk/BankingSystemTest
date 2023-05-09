using BankingSystem.Controllers;
using BankingSystem.Models;
using BankingSystem.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BankingSystem.Tests;

public class AccountControllerTests
{
    private readonly AccountController _accountController = new(new DbServiceMock(), new UserAccountService());

    [Fact]
    public void GetAll_Success()
    {
        var result = _accountController.GetUserAccounts("email1") as Ok<IEnumerable<UserAccountModel>>;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(2, result.Value.Count());
    }

    [Fact]
    public void GetAll_NotFound()
    {
        var result = _accountController.GetUserAccounts("WrongEmail") as NotFound<string>;

        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public void GetAccount_Success()
    {
        var result = _accountController.GetUserAccount("email1", "123") as Ok<UserAccountModel>;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public void GetAccount_UserNotFound()
    {
        var result = _accountController.GetUserAccount("WrongEmail", "123") as NotFound<string>;

        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public void GetAccount_AccountNotFound()
    {
        var result = _accountController.GetUserAccount("email1", "567") as NotFound<string>;

        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public void CreateAccount_Success()
    {
        var result = _accountController.CreateAccount(new AccountController.AccountRequest{ Email = "email1", AccountNumber = "1234"}) as Ok<UserAccountModel>;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public void CreateAccount_UserNotFound()
    {
        var result = _accountController.CreateAccount(new AccountController.AccountRequest{ Email = "WrongEmail", AccountNumber = "1234"}) as NotFound<string>;

        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public void CreateAccount_Dup()
    {
        var result = _accountController.CreateAccount(new AccountController.AccountRequest{ Email = "email1", AccountNumber = "123"}) as Conflict<string>;

        Assert.NotNull(result);
        Assert.Equal(409, result.StatusCode);
    }

    [Fact]
    public void DeleteAccount_Success()
    {
        var result = _accountController.DeleteAccount(new AccountController.AccountRequest{ Email = "email1", AccountNumber = "123"}) as Ok;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }
    
    [Fact]
    public void DeleteAccount_UserNotFound()
    {
        var result = _accountController.DeleteAccount(new AccountController.AccountRequest{ Email = "WrongEmail", AccountNumber = "123"}) as NotFound<string>;

        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public void DeleteAccount_AccountNotFound()
    {
        var result = _accountController.DeleteAccount(new AccountController.AccountRequest{ Email = "email1", AccountNumber = "1234"}) as NotFound<string>;

        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public void Deposit_Success()
    {
        var result = _accountController.Deposit(new AccountController.AccountRequest{ Email = "email1", AccountNumber = "123", Amount = 1}) as Ok<UserAccountModel>;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }
    
    [Fact]
    public void Deposit_UserNotFound()
    {
        var result = _accountController.Deposit(new AccountController.AccountRequest{ Email = "WrongEmail", AccountNumber = "123", Amount = 1}) as NotFound<string>;

        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public void Deposit_AccountNotFound()
    {
        var result = _accountController.Deposit(new AccountController.AccountRequest{ Email = "email1", AccountNumber = "1234", Amount = 1}) as NotFound<string>;

        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
    }
    
    [Fact]
    public void Deposit_AmountRequired()
    {
        var result = _accountController.Deposit(new AccountController.AccountRequest{ Email = "email1", AccountNumber = "123"}) as BadRequest<string>;

        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
    }

    [Fact]
    public void Deposit_10000()
    {
        var result = _accountController.Deposit(new AccountController.AccountRequest{ Email = "email1", AccountNumber = "123", Amount = 10_000}) as Ok<UserAccountModel>;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }
    
    [Fact]
    public void Deposit_ZeroAmount()
    {
        var result = _accountController.Deposit(new AccountController.AccountRequest{ Email = "email1", AccountNumber = "123", Amount = 0}) as BadRequest<string>;

        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
    }

    [Fact]
    public void Deposit_NegativeAmount()
    {
        var result = _accountController.Deposit(new AccountController.AccountRequest{ Email = "email1", AccountNumber = "123", Amount = -1}) as BadRequest<string>;

        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
    }

    [Fact]
    public void Deposit_GreaterThan10000()
    {
        var result = _accountController.Deposit(new AccountController.AccountRequest{ Email = "email1", AccountNumber = "123", Amount = 10_000.01}) as BadRequest<string>;

        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
    }

    [Fact]
    public void Withdraw_Success()
    {
        var result = _accountController.Withdraw(new AccountController.AccountRequest{ Email = "email1", AccountNumber = "123", Amount = 100}) as Ok<UserAccountModel>;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public void Withdraw_AmountRequired()
    {
        var result = _accountController.Withdraw(new AccountController.AccountRequest{ Email = "email1", AccountNumber = "123"}) as BadRequest<string>;

        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
    }

    [Fact]
    public void Withdraw_ZeroAmount()
    {
        var result = _accountController.Withdraw(new AccountController.AccountRequest{ Email = "email1", AccountNumber = "123", Amount = 0}) as BadRequest<string>;

        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
    }

    [Fact]
    public void Withdraw_NegativeAmount()
    {
        var result = _accountController.Withdraw(new AccountController.AccountRequest{ Email = "email1", AccountNumber = "123", Amount = -1}) as BadRequest<string>;

        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
    }

    [Fact]
    public void Withdraw_MoreThan90()
    {
        var result = _accountController.Withdraw(new AccountController.AccountRequest{ Email = "email1", AccountNumber = "123", Amount = 9_001}) as BadRequest<string>;

        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
    }
    
    [Fact]
    public void Withdraw_LessThat100()
    {
        var result = _accountController.Withdraw(new AccountController.AccountRequest{ Email = "email1", AccountNumber = "456", Amount = 101}) as BadRequest<string>;

        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
    }
}
