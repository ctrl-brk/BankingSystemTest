using System.Data;
using BankingSystem.Exceptions;
using BankingSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IDbService _dbService;
    private readonly IUserAccountService _accountService;

    public AccountController(IDbService dbService, IUserAccountService accountService)
    {
        _dbService = dbService;
        _accountService = accountService;
    }

    [HttpGet("{email}")]
    public IResult GetUserAccounts(string email)
    {
        try
        {
            return Results.Ok(_dbService.GetUserAccounts(email));
        }
        catch (KeyNotFoundException e)
        {
            return Results.NotFound(e.Message);
        }
    }

    [HttpGet("{email}/{accNum}")]
    public IResult GetUserAccount(string email, string accNum)
    {
        try
        {
            var acc = _dbService.GetUserAccount(email, accNum);
            return acc is null ? Results.NotFound($"Account {accNum} not found") : Results.Ok(acc);
        }
        catch (KeyNotFoundException e)
        {
            return Results.NotFound(e.Message);
        }
    }
    
    [HttpPost]
    public IResult CreateAccount(AccountRequest request)
    {
        try
        {
            var account = _accountService.CreateAccount(request.AccountNumber);
            return Results.Ok(_dbService.AddUserAccount(request.Email, account));
        }
        catch (KeyNotFoundException e)
        {
            return Results.NotFound(e.Message);
        }
        catch (DuplicateNameException e)
        {
            return Results.Conflict(e.Message);
        }
    }

    [HttpDelete]
    public IResult DeleteAccount(AccountRequest request)
    {
        try
        {
            _dbService.DeleteUserAccount(request.Email, request.AccountNumber);
            return Results.Ok();
        }
        catch (KeyNotFoundException e)
        {
            return Results.NotFound(e.Message);
        }
    }

    [HttpPost("deposit")]
    public IResult Deposit(AccountRequest request)
    {
        try
        {
            if (request.Amount is null)
                return Results.BadRequest("Amount is required");

            var account = _dbService.GetUserAccount(request.Email, request.AccountNumber);
            return account is null
                ? Results.NotFound($"Account {request.AccountNumber} not found")
                : Results.Ok(_accountService.Deposit(account, request.Amount.Value));
        }
        catch (KeyNotFoundException e)
        {
            return Results.NotFound(e.Message);
        }
        catch (AccountException e)
        {
            return Results.BadRequest(e.Message);
        }
    }

    [HttpPost("withdraw")]
    public IResult Withdraw(AccountRequest request)
    {
        try
        {
            if (request.Amount is null)
                return Results.BadRequest("Amount is required");

            var account = _dbService.GetUserAccount(request.Email, request.AccountNumber);
            return account is null
                ? Results.NotFound($"Account {request.AccountNumber} not found")
                : Results.Ok(_accountService.Withdraw(account, request.Amount.Value));
        }
        catch (KeyNotFoundException e)
        {
            return Results.NotFound(e.Message);
        }
        catch (AccountException e)
        {
            return Results.BadRequest(e.Message);
        }
    }
    
    public class AccountRequest
    {
        public string Email { get; set; } = string.Empty;
        public string AccountNumber  { get; set; } = string.Empty;
        public double? Amount  { get; set; } =  null;
    }
}
