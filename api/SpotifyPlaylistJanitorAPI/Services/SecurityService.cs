using Microsoft.IdentityModel.Tokens;
using SpotifyPlaylistJanitorAPI.Models.Auth;
using SpotifyPlaylistJanitorAPI.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SpotifyPlaylistJanitorAPI.Services
{
    /// <summary>
    /// Service that handling security artifacts i.e Web Tokens, Encrypted passwords, encrytable/decryptable strings.
    /// </summary>
    public class SecurityService : ISecurityService
    {
        private const int refreshTokenExpiryHours = 1;
        private const int keySize = 64;
        private const int iterations = 350000;
        private HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        public SecurityService() { }

        /// <summary>
        /// Generate a JSON Web Token for the supplied User Model.
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="now"></param>
        /// <param name="issuer"></param>
        /// <param name="audience"></param>
        /// <param name="key"></param>
        ///<returns>Returns a JWT <see cref = "string" />.</returns>
        public string GenerateJSONWebToken(UserModel userInfo, DateTime now, string issuer, string audience, string key)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userInfo.Username),
                new Claim(ClaimTypes.Role, userInfo.Role)
            };

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: now.AddHours(refreshTokenExpiryHours),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Generate a random string to use as a Refresh Token
        /// </summary>
        /// <returns></returns>
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        /// <summary>
        /// Hash the supplied password to safely store.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="key"></param>
        ///<returns>Returns a <see cref = "string" />.</returns>
        public string HashPasword(string password, string key)
        {
            var salt = Encoding.ASCII.GetBytes(key);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize);
            return Convert.ToHexString(hash);
        }

        /// <summary>
        /// Compare supplied password to stored hashed password.
        /// </summary>
        /// <param name="password">User password.</param>
        /// <param name="hashedPassword">Stored hashed password to compare.</param>
        /// <param name="key"></param>
        ///<returns>Returns a <see cref = "bool" />.</returns>
        public bool VerifyPassword(string password, string hashedPassword, string key)
        {
            var salt = Encoding.ASCII.GetBytes(key);
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                iterations,
                hashAlgorithm,
                keySize);
            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hashedPassword));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="SecurityTokenException"></exception>
        public ClaimsPrincipal? GetPrincipalFromToken(string token, string key)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principal;
        }

        /// <summary>
        /// Encrypt a string using a supplied key.
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string EncryptString(string plainText, string key)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        /// <summary>
        /// Decrypt a string using a supplied key.
        /// </summary>
        /// <param name="secureString"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string DecryptString(string secureString, string key)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(secureString);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
