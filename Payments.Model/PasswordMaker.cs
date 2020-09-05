using Payments.Data;
using System;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Reflection;
using System.Configuration;

namespace Payments.Model
{
    static class PasswordMaker
    {
        private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
        private static SHA256Cng sha256 = new SHA256Cng();
        private const int SaltSize = 32;

        public static string GenerateSalt()
        {
            byte[] saltBytes = new byte[SaltSize];
            rngCsp.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        /// <summary>
        /// Хеширует пароль со случайной солью
        /// </summary>
        /// <returns>Кортеж (хэш,соль)</returns>
        public static Tuple<string, string> MakeSaltedHash(SecureString password)
        {
            byte[] saltBytes = new byte[SaltSize];
            rngCsp.GetBytes(saltBytes);
            string hash = MakeHash(password, saltBytes);
            string salt = GenerateSalt();
            return new Tuple<string, string>(hash, salt);
        }

        /// <summary>
        /// Хэширует пароль с заданной байтами солью
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns>Хэш строка</returns>
        public static string MakeHash(SecureString password, byte[] saltBytes)
        {
            byte[] passBytes = Converters.GetBytes(password);
            byte[] saltedPass = Converters.Concat<byte>(passBytes, saltBytes);
            byte[] hashBytes = sha256.ComputeHash(saltedPass);
            string hash = Convert.ToBase64String(hashBytes);
            return hash;
        }

        /// <summary>
        /// Хэширует пароль с заданной строкой солью
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns>Хэш строка</returns>
        public static string MakeHash(SecureString password, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            return MakeHash(password, saltBytes);
        }

        /// <summary>
        /// Проверяет стойкость пароля
        /// </summary>
        /// <param name="password">Пароль</param>
        /// <returns>Можно ли использовать пароль</returns>
        public static bool CheckPassword(SecureString password)
        {
            return password.Length > 3;
        }
    }
}
