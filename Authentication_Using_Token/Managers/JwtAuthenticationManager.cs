using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authentication_Using_Token.Managers
{
    public class JwtAuthenticationManager
    {
        public readonly string key;

        private readonly IDictionary<string, string> users = new Dictionary<string, string>()
        {
            {
                "test", "password"
            },
            {
                "test1",
                "pwd"
            }
        };

        public JwtAuthenticationManager(string key)
        {
            this.key = key;
        }
        public string Authenticate(string username, string password)
        {

            if(!users.Any(x=>x.Key == username && x.Value == password))
            {
                return null;
            }

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            var tokenKey = Encoding.UTF8.GetBytes(key);

            var keysize = tokenKey.Length.ToString();

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                   new  Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = handler.CreateToken(tokenDescriptor);

            return handler.WriteToken(token);


           
        }
    }
}
