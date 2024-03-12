using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;

namespace DotnetAPI.Helpers
{
    public class AuthHelper
    {
        private readonly IConfiguration _configuration;
        public AuthHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

    public byte[] GetPasswordHash(string password, byte[] passwordSalt)
    {
        string passwordSaltString = _configuration.GetSection("AppSettings:PasswordKey").Value + Convert.ToBase64String(passwordSalt);

        return KeyDerivation.Pbkdf2(
            password: password,
            salt: Encoding.ASCII.GetBytes(passwordSaltString),
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 1000000,
            numBytesRequested: 256 / 8
        );
    }

    public string CreateToken(int userId)
    {

        Claim[] claims = new Claim[]
        {
                new Claim("userId", userId.ToString())
        };

        string? token = _configuration.GetSection("AppSettings:TokenKey").Value;
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token != null ? token : ""));

        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = creds
        };

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(securityToken);

    }
}

}