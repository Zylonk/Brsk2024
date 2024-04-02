using brskweb.Model;

namespace brskweb.ModelDTO
{
    public class UserDTO
    {
        public int UserId { get; set; }

        public string Username { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public int RoleId { get; set; }

        public string? Login { get; set; }
        public static User ConvertToUser(UserDTO dto)
        {
            User user = new User();
            user.UserId = dto.UserId;
            user.RoleId = dto.RoleId;
            user.Login = dto.Login;
            user.PasswordHash = dto.PasswordHash;
            user.Username = dto.Username;
            return user;
        }
    }
}
