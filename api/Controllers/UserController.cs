using System.Data;
using BankingSystem.Models;
using BankingSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IDbService _dbService;

    public UserController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [HttpGet]
    public IResult GetAll()
    {
        return Results.Ok(_dbService.GetUsers());
    }

    [HttpGet("{email}")]
    public IResult GetUser(string email)
    {
        var user = _dbService.GetUser(email);
        return user is null ? Results.NotFound(email) : Results.Ok(user);
    }
    
    [HttpPost]
    public IResult Post(UserRequest userRequest)
    {
        try
        {
            return Results.Created($"/User/{userRequest.Email}", _dbService.AddUser(userRequest.Name, userRequest.Email));
        }
        catch (DuplicateNameException e)
        {
            return Results.Conflict(e.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public IResult Put(Guid id, UserRequest userRequest)
    {
        try
        {
            return Results.Accepted($"/User/{userRequest.Email}", _dbService.UpdateUser(new UserModel(id, userRequest.Name, userRequest.Email)));
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
    public IResult Delete([FromBody] string email)
    {
        try
        {
            _dbService.DeleteUser(email);
            return Results.Ok();
        }
        catch (KeyNotFoundException e)
        {
            return Results.NotFound(e.Message);
        }
    }
    
    public new class UserRequest
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
