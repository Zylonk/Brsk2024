using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace brskweb.Class
{
    public class TokenService
    {
        public static (string userId, string userName, string userRole) GetTokenClaims()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(Tokens.Token) as JwtSecurityToken;

            if (jwtToken == null)
            {
                // Обработка ошибки, если токен некорректен
                return (null, null, null);
            }

            var userId = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            var userName = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.GivenName)?.Value;
            var userRole = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value;

            return (userId, userName, userRole);
        }
    }
}
