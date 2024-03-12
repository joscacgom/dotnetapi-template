using DotnetAPI.Data;
using Microsoft.AspNetCore.Mvc;
using DotnetAPI.Models;
using DotnetAPI.Dtos;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    DataContextDapper dataContextDapper;
    public UserController(IConfiguration configuration)
    {
        dataContextDapper = new DataContextDapper(configuration);
    }

    [HttpGet("TestConnection", Name = "TestConnection")]
    public DateTime TestConnection()
    {
        return dataContextDapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }

    [HttpGet("GetUser/{userId}", Name = "GetUser")]
    public User? GetUser(int userId)
    {
        return dataContextDapper.LoadDataSingle<User>($"SELECT * FROM TutorialAppSchema.Users WHERE UserId = {userId}");
    }

    [HttpGet("GetUsers", Name = "GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        return dataContextDapper.LoadData<User>("SELECT * FROM TutorialAppSchema.Users");

    }

    [HttpPost("AddUser", Name = "AddUser")]
    public IActionResult AddUser(UserDTO user)
    {
        bool result = dataContextDapper.ExecuteData("INSERT INTO TutorialAppSchema.Users (FirstName , LastName , Email , Gender,  Active ) VALUES (@FirstName, @LastName, @Email, @Gender, @Active) ", new
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Gender = user.Gender,
            Active = user.Active,
        });
        return result ? Ok() : BadRequest();
    }

    [HttpPut("UpdateUser", Name = "UpdateUser")]
    public IActionResult UpdateUser(User user)
    {
        bool result = dataContextDapper.ExecuteData("UPDATE TutorialAppSchema.Users SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Gender = @Gender, Active = @Active WHERE UserId = @UserId",
            new
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Gender = user.Gender,
                Active = user.Active,
                UserId = user.UserId
            });
        return result ? Ok() : BadRequest();
    }

    [HttpDelete("DeleteUser/{userId}", Name = "DeleteUser")]
    public IActionResult DeleteUser(int userId)
    {
        bool result = dataContextDapper.ExecuteData("DELETE FROM TutorialAppSchema.Users WHERE UserId = @UserId", new { UserId = userId });
        return result ? Ok() : BadRequest();
    }

    [HttpGet("GetUserSalary/{userId}", Name = "GetUserSalary")]
    public UserSalary? GetUserSalary(int userId)
    {
        return dataContextDapper.LoadDataSingle<UserSalary>($"SELECT * FROM TutorialAppSchema.UserSalary WHERE UserId = {userId}");
    }

    [HttpGet("GetUserSalaries", Name = "GetUserSalaries")]
    public IEnumerable<UserSalary> GetUserSalaries()
    {
        return dataContextDapper.LoadData<UserSalary>("SELECT * FROM TutorialAppSchema.UserSalary");

    }

    [HttpPost("AddUserSalary", Name = "AddUserSalary")]
    public IActionResult AddUserSalary(UserSalary userSalary)
    {
        bool result = dataContextDapper.ExecuteData("INSERT INTO TutorialAppSchema.UserSalary (UserId , Salary) VALUES (@UserId, @Salary) ", new
        {
            UserId = userSalary.UserId,
            Salary = userSalary.Salary,
        });
        return result ? Ok() : BadRequest();
    }

    [HttpPut("UpdateUserSalary", Name = "UpdateUserSalary")]
    public IActionResult UpdateUserSalary(UserSalary userSalary)
    {
        bool result = dataContextDapper.ExecuteData("UPDATE TutorialAppSchema.UserSalary SET Salary = @Salary WHERE UserId = @UserId",
            new
            {
                Salary = userSalary.Salary,
                UserId = userSalary.UserId
            });
        return result ? Ok() : BadRequest();
    }

    [HttpDelete("DeleteUserSalary/{userId}", Name = "DeleteUserSalary")]
    public IActionResult DeleteUserSalary(int userId)
    {
        bool result = dataContextDapper.ExecuteData("DELETE FROM TutorialAppSchema.UserSalary WHERE UserId = @UserId", new { UserId = userId });
        return result ? Ok() : BadRequest();
    }

    [HttpGet("GetUserJobInfo/{userId}", Name = "GetUserJobInfo")]
    public UserJobInfo? GetUserJobInfo(int userId)
    {
        return dataContextDapper.LoadDataSingle<UserJobInfo>($"SELECT * FROM TutorialAppSchema.UserJobInfo WHERE UserId = {userId}");
    }

    [HttpGet("GetUserJobInfos", Name = "GetUserJobInfos")]
    public IEnumerable<UserJobInfo> GetUserJobInfos()
    {
        return dataContextDapper.LoadData<UserJobInfo>("SELECT * FROM TutorialAppSchema.UserJobInfo");

    }

    [HttpPost("AddUserJobInfo", Name = "AddUserJobInfo")]
    public IActionResult AddUserJobInfo(UserJobInfo userJobInfo)
    {
        bool result = dataContextDapper.ExecuteData("INSERT INTO TutorialAppSchema.UserJobInfo (UserId , JobTitle , Department) VALUES (@UserId, @JobTitle, @Department) ", new
        {
            UserId = userJobInfo.UserId,
            JobTitle = userJobInfo.JobTitle,
            Department = userJobInfo.Department,
        });
        return result ? Ok() : BadRequest();
    }

    [HttpPut("UpdateUserJobInfo", Name = "UpdateUserJobInfo")]
    public IActionResult UpdateUserJobInfo(UserJobInfo userJobInfo)
    {
        bool result = dataContextDapper.ExecuteData("UPDATE TutorialAppSchema.UserJobInfo SET JobTitle = @JobTitle, Department = @Department WHERE UserId = @UserId",
            new
            {
                JobTitle = userJobInfo.JobTitle,
                Department = userJobInfo.Department,
                UserId = userJobInfo.UserId
            });
        return result ? Ok() : BadRequest();
    }

    [HttpDelete("DeleteUserJobInfo/{userId}", Name = "DeleteUserJobInfo")]
    public IActionResult DeleteUserJobInfo(int userId)
    {
        bool result = dataContextDapper.ExecuteData("DELETE FROM TutorialAppSchema.UserJobInfo WHERE UserId = @UserId", new { UserId = userId });
        return result ? Ok() : BadRequest();
    }
}