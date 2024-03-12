using System.Data;
using System.Security.Cryptography;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace DotnetAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly DataContextDapper _context;
        private readonly AuthHelper _authHelper;
        public AuthController(IConfiguration configuration)
        {
            _context = new DataContextDapper(configuration);
            _authHelper = new AuthHelper(configuration);
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register(UserRegistrationDTO userForRegistration)
        {
            if (userForRegistration.Password == userForRegistration.Password2)
            {
                string sqlCheckUserExists = "SELECT Email FROM TutorialAppSchema.Auth WHERE Email = '" +
                    userForRegistration.Email + "'";

                IEnumerable<string> existingUsers = _context.LoadData<string>(sqlCheckUserExists);
                if (existingUsers.Count() == 0)
                {
                    byte[] passwordSalt = new byte[128 / 8];
                    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                    {
                        rng.GetNonZeroBytes(passwordSalt);
                    }

                    byte[] passwordHash = _authHelper.GetPasswordHash(userForRegistration.Password, passwordSalt);

                    string sqlAddAuth = @"
                        INSERT INTO TutorialAppSchema.Auth  ([Email],
                        [PasswordHash],
                        [PasswordSalt]) VALUES ('" + userForRegistration.Email +
                        "', @PasswordHash, @PasswordSalt)";

                    List<SqlParameter> sqlParameters = new List<SqlParameter>();

                    SqlParameter passwordSaltParameter = new SqlParameter("@PasswordSalt", SqlDbType.VarBinary);
                    passwordSaltParameter.Value = passwordSalt;

                    SqlParameter passwordHashParameter = new SqlParameter("@PasswordHash", SqlDbType.VarBinary);
                    passwordHashParameter.Value = passwordHash;

                    sqlParameters.Add(passwordSaltParameter);
                    sqlParameters.Add(passwordHashParameter);
                    if (_context.ExecuteWithParameters(sqlAddAuth, sqlParameters))
                    {

                        string sqlAddUser = "INSERT INTO TutorialAppSchema.Users (Email, FirstName, LastName) VALUES ('" +
                            userForRegistration.Email + "', '" + userForRegistration.FirstName + "', '" +
                            userForRegistration.LastName + "')";
                        if (_context.ExecuteData(sqlAddUser, userForRegistration))
                        {
                            return Ok();
                        }
                        throw new Exception("Failed to add user.");
                    }
                    throw new Exception("Failed to register user.");
                }
                throw new Exception("User with this email already exists!");
            }
            throw new Exception("Passwords do not match!");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(UserLoginDTO user)
        {
            string sqlForHashAndSalt = $"SELECT PasswordHash, PasswordSalt FROM TutorialAppSchema.Auth WHERE Email = '{user.Email}'";
            UserLoginConfirmationDTO? userWithHashAndSalt = _context.LoadDataSingle<UserLoginConfirmationDTO>(sqlForHashAndSalt);

            if (userWithHashAndSalt == null)
            {
                return BadRequest("User does not exist");
            }
            byte[] passwordHash = _authHelper.GetPasswordHash(user.Password, userWithHashAndSalt.PasswordSalt);

            for (int i = 0; i < passwordHash.Length; i++)
            {
                if (passwordHash[i] != userWithHashAndSalt.PasswordHash[i])
                {
                    return StatusCode(401, "Incorrect password");
                }
            }

            int userId = _context.LoadDataSingle<int>($"SELECT UserId FROM TutorialAppSchema.Users WHERE Email = '{user.Email}'");
            return Ok(new Dictionary<string, string>{
                {"token", _authHelper.CreateToken(userId)}
            });
        }

        [HttpGet("RefreshToken")]
        public string RefreshToken()
        {
            string sql = $"SELECT UserId FROM TutorialAppSchema.Users WHERE UserId = {User.FindFirst("userId")?.Value}";
            int userId = _context.LoadDataSingle<int>(sql);
            Console.WriteLine(userId);
            return _authHelper.CreateToken(userId);
        }

    }
}