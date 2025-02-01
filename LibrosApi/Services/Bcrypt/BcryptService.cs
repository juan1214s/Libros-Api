using Microsoft.AspNetCore.Identity;

namespace LibrosApi.Services.Bcrypt
{
    public class BcryptService
    {
        public readonly int _workFactor;
        
        public BcryptService(int workFactor = 12)
        {
            _workFactor = workFactor;
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, _workFactor);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
