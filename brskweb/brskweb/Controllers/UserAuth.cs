using brskweb.Class;
using brskweb.Model;
using brskweb.ModelDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace brskweb.Controllers
{
    [Authorize]
    [ApiController]
    public class UserAuth : ControllerBase
    {
        private readonly IConfiguration _config;

        public UserAuth(IConfiguration config)
        {
            _config = config;
        }
        
        [AllowAnonymous]
        [Route("api/auth")]
        [HttpPost]
        public ActionResult AuthUser(AuthDTO user)
        {
            if (user != null)
            {
                var listUser = GetrContext.Context.Users.FirstOrDefault(x => x.Login == user.Login && x.PasswordHash == user.PasswordHash);
                if (listUser != null)
                {
                    var token = GenerateToken(listUser.Username, listUser.UserId, listUser.RoleId);
                    Tokens.Token = token;
                    return Ok(token);
                }
                return BadRequest("Неверный логин или пароль");
            }
            return BadRequest("Ошибка");
        }
        [AllowAnonymous]
        [Route("api/registr")]
        [HttpPost]
        public ActionResult RegistrationUser(UserDTO user)
        {
            if (user != null && GetrContext.Context.Users.FirstOrDefault(x => x.Login == user.Login) == null) {
                user.UserId = GetrContext.Context.Users.ToList().Count() + 1;
                GetrContext.Context.Users.Add(UserDTO.ConvertToUser(user));
                GetrContext.Context.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }
       
        private string GenerateToken(string userName, int id, int role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier,id.ToString()),
            new Claim(ClaimTypes.GivenName,userName),
            new Claim(ClaimTypes.Role, role.ToString())
            };
            var token = new JwtSecurityToken(
                _config.GetSection("Jwt:Issuer").Value, _config.GetSection("Jwt:Audience").Value,
                claims,
                expires: DateTime.Now.AddMinutes(55),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
