using System.Security.Cryptography;

namespace TaskManagerApi.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100_000;

        public string Hash(string password)
        {
            ArgumentException.ThrowIfNullOrEmpty(password);

            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256,
                HashSize);

            return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        public bool Verify(string password, string passwordHash)
        {
            if (string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(passwordHash))
            {
                return false;
            }

            string[] parts = passwordHash.Split('.', 3);

            if (parts.Length != 3 ||
                !int.TryParse(parts[0], out int iterations))
            {
                return false;
            }

            try
            {
                byte[] salt = Convert.FromBase64String(parts[1]);

                byte[] expectedHash =
                    Convert.FromBase64String(parts[2]);

                byte[] actualHash =
                    Rfc2898DeriveBytes.Pbkdf2(
                        password,
                        salt,
                        iterations,
                        HashAlgorithmName.SHA256,
                        expectedHash.Length);

                return CryptographicOperations.FixedTimeEquals(
                    actualHash,
                    expectedHash);
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}