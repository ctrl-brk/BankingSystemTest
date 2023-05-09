using BankingSystem.Controllers;
using BankingSystem.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BankingSystem.Tests;

public class UserControllerTests
{
    private readonly UserController _userController;
    private readonly DbServiceMock _dbService;

    public UserControllerTests()
    {
        _dbService = new DbServiceMock();
        _userController = new UserController(_dbService);
    }
    
    [Fact]
    public void GetAll_Success()
    {
        var result = _userController.GetAll() as Ok<IEnumerable<UserModel>>;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(2, result.Value.Count());
    }
    
    [Fact]
    public void GetUser_Success()
    {
        var result = _userController.GetUser("Email1") as Ok<UserModel>;

        Assert.NotNull(result.Value);
        Assert.Equal(200, result.StatusCode);
    }
    
    [Fact]
    public void GetUser_NotFound()
    {
        var result = _userController.GetUser("WrongEmail") as NotFound<string>;

        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
        Assert.Equal("WrongEmail", result.Value);
    }
    
    [Fact]
    public void CreateUser_Success()
    {
        var result = _userController.Post(new UserController.UserRequest{Name = "User3", Email = "Email3"}) as Created<UserModel>;

        Assert.NotNull(result);
        Assert.Equal(201, result.StatusCode);
    }

    [Fact]
    public void CreateUser_Dup()
    {
        var result = _userController.Post(new UserController.UserRequest{Name = "User1", Email = "Email1"}) as Conflict<string>;

        Assert.NotNull(result);
        Assert.Equal(409, result.StatusCode);
    }

    [Fact]
    public void UpdateUser_Success()
    {
        var user = _dbService.Users[0];        
        
        var result = _userController.Put(user.Id, new UserController.UserRequest{Name = "User3", Email = "Email3"}) as Accepted<UserModel>;

        Assert.NotNull(result);
        Assert.Equal(202, result.StatusCode);
        Assert.Equal("User3", result.Value.Name);
        Assert.Equal("Email3", result.Value.Email);
    }

    [Fact]
    public void UpdateUser_Dup()
    {
        var user = _dbService.Users[0];        
        
        var result = _userController.Put(user.Id, new UserController.UserRequest{Name = "User2", Email = "Email2"}) as Conflict<string>;

        Assert.NotNull(result);
        Assert.Equal(409, result.StatusCode);
    }

    [Fact]
    public void UpdateUser_NotFound()
    {
        var result = _userController.Put(Guid.NewGuid(), new UserController.UserRequest{Name = "User3", Email = "Email3"}) as NotFound<string>;

        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public void DeleteUser_Success()
    {
        var result = _userController.Delete("Email1") as Ok;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public void DeleteUser_NotFound()
    {
        var result = _userController.Delete("Email3") as NotFound<string>;

        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
    }
    
}