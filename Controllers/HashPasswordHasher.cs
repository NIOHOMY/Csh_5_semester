

using Microsoft.AspNetCore.Identity;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HashPasswordHasher : IPasswordHasher<IdentityUser>
    {
        public string HashPassword(IdentityUser user, string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public PasswordVerificationResult VerifyHashedPassword(IdentityUser user, string hashedPassword, string providedPassword)
        {
            if (BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword))
            {
                return PasswordVerificationResult.Success;
            }

            return PasswordVerificationResult.Failed;
        }
    }

}
