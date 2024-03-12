using DotnetAPI.Models;

namespace DotnetAPI.Data
{

    public class UserRepository : IUserRepository
    {

        DataContextEF _entityFramework;
        public UserRepository(IConfiguration configuration)
        {
            _entityFramework = new DataContextEF(configuration);
        }

        public bool SaveChanges()
        {
            return _entityFramework.SaveChanges() > 0;
        }

        public void Add<T>(T entity)
        {
            if (entity != null) _entityFramework.Add(entity);
        }

        public void Remove<T>(T entity)
        {
            if (entity != null) _entityFramework.Remove(entity);
        }


        public IEnumerable<User> GetUsers()
        {
            return _entityFramework.Users.ToList<User>();

        }

        public IEnumerable<UserSalary> GetUserSalaries()
        {
            return _entityFramework.UserSalary.ToList<UserSalary>();

        }

        public IEnumerable<UserJobInfo> GetUserJobInfo()
        {
            return _entityFramework.UserJobInfo.ToList<UserJobInfo>();

        }

        public User GetUser(int userId)
        {
            User? user = _entityFramework.Users.Where(u => u.UserId == userId).FirstOrDefault();
            if (user == null)
            {
                return new User();
            }
            return user;
        }

        public UserSalary GetUserSalary(int userId)
        {
            UserSalary? userSalary = _entityFramework.UserSalary.Where(u => u.UserId == userId).FirstOrDefault();
            if (userSalary == null)
            {
                return new UserSalary();
            }
            return userSalary;
        }

        public UserJobInfo GetUserJobInfo(int userId)
        {
            UserJobInfo? userJobInfo = _entityFramework.UserJobInfo.Where(u => u.UserId == userId).FirstOrDefault();
            if (userJobInfo == null)
            {
                return new UserJobInfo();
            }
            return userJobInfo;
        }

    }

}