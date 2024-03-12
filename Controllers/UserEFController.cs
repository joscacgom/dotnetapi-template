using DotnetAPI.Data;
using Microsoft.AspNetCore.Mvc;
using DotnetAPI.Models;
using DotnetAPI.Dtos;
using AutoMapper;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserControllerEF : ControllerBase
{
    IUserRepository _userRepository;
    IMapper _mapper;
    public UserControllerEF(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, User>()));
    }

    [HttpGet("GetUserEF/{userId}", Name = "GetUserEF")]
    public User GetUser(int userId)
    {
        return _userRepository.GetUser(userId);
    }

    [HttpGet("GetUsersEF", Name = "GetUsersEF")]
    public IEnumerable<User> GetUsers()
    {
        return _userRepository.GetUsers();

    }

    [HttpPost("AddUserEF", Name = "AddUserEF")]
    public IActionResult AddUser(UserDTO user)
    {
        User userDb = _mapper.Map<User>(user);

        _userRepository.Add<User>(userDb);
        return _userRepository.SaveChanges() ? Ok() : BadRequest();
    }

    [HttpPut("UpdateUserEF", Name = "UpdateUserEF")]
    public IActionResult UpdateUser(User user)
    {
        User userDb = _userRepository.GetUser(user.UserId);

        userDb.FirstName = user.FirstName;
        userDb.LastName = user.LastName;
        userDb.Email = user.Email;
        userDb.Gender = user.Gender;
        userDb.Active = user.Active;

        return _userRepository.SaveChanges() ? Ok() : BadRequest();
    }

    [HttpDelete("DeleteUserEF/{userId}", Name = "DeleteUserEF")]
    public IActionResult DeleteUser(int userId)
    {
        User userDb = _userRepository.GetUser(userId);

        _userRepository.Remove<User>(userDb);
        return _userRepository.SaveChanges() ? Ok() : BadRequest();
    }

    [HttpGet("GetUserSalaryEF/{userId}", Name = "GetUserSalaryEF")]
    public UserSalary GetUserSalary(int userId)
    {
        UserSalary userSalary = _userRepository.GetUserSalary(userId);

        return userSalary;
    }

    [HttpGet("GetUserSalariesEF", Name = "GetUserSalariesEF")]
    public IEnumerable<UserSalary> GetUserSalaries()
    {
        return _userRepository.GetUserSalaries();

    }

    [HttpPost("AddUserSalaryEF", Name = "AddUserSalaryEF")]
    public IActionResult AddUserSalary(UserSalary userSalary)
    {
        _userRepository.Add<UserSalary>(userSalary);
        return _userRepository.SaveChanges() ? Ok() : BadRequest();
    }

    [HttpPut("UpdateUserSalaryEF", Name = "UpdateUserSalaryEF")]
    public IActionResult UpdateUserSalary(UserSalary userSalary)
    {
        UserSalary userSalaryDb = _userRepository.GetUserSalary(userSalary.UserId);

        userSalaryDb.Salary = userSalary.Salary;

        return _userRepository.SaveChanges() ? Ok() : BadRequest();
    }

    [HttpDelete("DeleteUserSalaryEF/{userId}", Name = "DeleteUserSalaryEF")]
    public IActionResult DeleteUserSalary(int userId)
    {
        UserSalary userSalaryDb = _userRepository.GetUserSalary(userId);

        _userRepository.Remove<UserSalary>(userSalaryDb);
        return _userRepository.SaveChanges() ? Ok() : BadRequest();
    }

    [HttpGet("GetUserJobInfoEF/{userId}", Name = "GetUserJobInfoEF")]
    public UserJobInfo GetUserJobInfo(int userId)
    {
        UserJobInfo userJobInfo = _userRepository.GetUserJobInfo(userId);

        return userJobInfo;
    }

    [HttpGet("GetUserJobInfosEF", Name = "GetUserJobInfosEF")]
    public IEnumerable<UserJobInfo> GetUserJobInfos()
    {
        return _userRepository.GetUserJobInfo();

    }

    [HttpPost("AddUserJobInfoEF", Name = "AddUserJobInfoEF")]
    public IActionResult AddUserJobInfo(UserJobInfo userJobInfo)
    {
        _userRepository.Add<UserJobInfo>(userJobInfo);
        return _userRepository.SaveChanges() ? Ok() : BadRequest();
    }

    [HttpPut("UpdateUserJobInfoEF", Name = "UpdateUserJobInfoEF")]
    public IActionResult UpdateUserJobInfo(UserJobInfo userJobInfo)
    {
        UserJobInfo userJobInfoDb = _userRepository.GetUserJobInfo(userJobInfo.UserId);

        userJobInfoDb.JobTitle = userJobInfo.JobTitle;
        userJobInfoDb.Department = userJobInfo.Department;

        return _userRepository.SaveChanges() ? Ok() : BadRequest();
    }

    [HttpDelete("DeleteUserJobInfoEF/{userId}", Name = "DeleteUserJobInfoEF")]
    public IActionResult DeleteUserJobInfo(int userId)
    {
        UserJobInfo userJobInfoDb = _userRepository.GetUserJobInfo(userId);

        _userRepository.Remove<UserJobInfo>(userJobInfoDb);
        return _userRepository.SaveChanges() ? Ok() : BadRequest();
    }

}