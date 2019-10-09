using BlazorMud.Contracts.Security;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace BlazorMud.BusinessLogic.Security
{
    /// <summary>
    /// Provides functionality for password hashing.
    /// </summary>
    /// <remarks>
    /// Implementation as shown at https://stackoverflow.com/questions/4181198/how-to-hash-a-password/10402129#10402129
    /// </remarks>
    public sealed class PasswordHasher : IPasswordHasher
    {
        private const int Iterations = 10000;
        private const int SaltSize = 16;
        private const int PasswordSize = 20;

        public string CreateHashedPassword(string password)
        {
            // Create the salt value with a cryptographic PRNG
            byte[] salt = new byte[SaltSize];
            new RNGCryptoServiceProvider().GetBytes(salt);

            // Create the Rfc2898DeriveBytes and get the hash value
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            byte[] hash = pbkdf2.GetBytes(PasswordSize);

            // Combine the salt and password bytes for later use
            var hashBytes = new byte[SaltSize + PasswordSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, PasswordSize);

            // Turn the combined salt+hash into a string
            return Convert.ToBase64String(hashBytes);
        }

        public bool IsSamePassword(string password, string hashedPassword)
        {
            // Extract the bytes
            var hashBytes = Convert.FromBase64String(hashedPassword);

            // Get the salt
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            // Compute the hash on the password the user entered
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            var hash = pbkdf2.GetBytes(PasswordSize);

            // Check, if hashed bytes are equal to password bytes
            return hash.SequenceEqual(hashBytes.Skip(salt.Length));
        }
    }
}
