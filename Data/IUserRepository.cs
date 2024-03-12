using DotnetAPI.Models;

namespace DotnetAPI.Data
{
    public interface IUserRepository
    {
        bool SaveChanges();
        void Add<T>(T entity);
        void Remove<T>(T entity);
        IEnumerable<User> GetUsers();
        IEnumerable<UserSalary> GetUserSalaries();
        IEnumerable<UserJobInfo> GetUserJobInfo();
        User GetUser(int userId);
        UserSalary GetUserSalary(int userId);
        UserJobInfo GetUserJobInfo(int userId);

    }

}