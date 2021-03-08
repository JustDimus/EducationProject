using EducationProject.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace EducationProject.Infrastructure.BLL.PasswordHasher
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            var sha256 = SHA256.Create();

            var bytes = Encoding.UTF8.GetBytes(password);

            var hash = sha256.ComputeHash(bytes);

            var hashedPassword = Convert.ToBase64String(hash);

            return hashedPassword;
        }
    }
}
