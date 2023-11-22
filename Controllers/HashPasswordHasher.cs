

namespace WebApplication1.Controllers
{
    public class HashPasswordHasher
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyHashedPassword(string providedPassword, string hashedPassword)
        {
            return (BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword));         
        }
    }
}
