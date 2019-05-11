using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace NotBook.Service.Hash
{
    public class HmacHashService : IHashService
    {
        public void CreateHash(string text, out byte[] hash, out byte[] salt)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(text));

            using (var hmac = new HMACSHA512())
            {
                salt = hmac.Key;
                hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(text));
            }
        }

        public bool VerifyHash(string text, byte[] hash, byte[] salt)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(text));
            if (hash.Length != 64)
                throw new ArgumentException("Invalid length of password hash (64 bytes expected).", nameof(hash));
            if (salt.Length != 128)
                throw new ArgumentException("Invalid length of password salt (128 bytes expected).",
                    nameof(salt));

            using (var hmac = new HMACSHA512(salt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(text));
                if (computedHash.Where((t, i) => t != hash[i]).Any()) return false;
            }

            return true;
        }
    }
}