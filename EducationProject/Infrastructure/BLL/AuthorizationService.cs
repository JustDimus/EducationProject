using EducationProject.BLL.Interfaces;
using EducationProject.Core.Models;
using EducationProject.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.BLL
{
    public class AuthorizationService
    {
        private Dictionary<string, int> authorizedAccounts;
        
        private IRepository<Account> accounts;

        public AuthorizationService(IRepository<Account> accountMapping)
        {
            accounts = accountMapping;

            authorizedAccounts = new Dictionary<string, int>();
        }

        public string AuthorizeAccount(string email, string password)
        {
            var accountId = accounts.Get(a => a.Email == email && a.Password == password, a => a.Id);

            if(accountId == 0)
            {
                return null;
            }

            string token = $"{DateTime.Now}/{email}";

            authorizedAccounts.Add(token, accountId);

            return token;
        }

        public int AuthenticateAccount(string token)
        {
            return token == null ? 0 : authorizedAccounts.GetValueOrDefault(token);
        }

        public bool DeauthorizeAccount(string token)
        {
            return token == null ? false : authorizedAccounts.Remove(token);
        }
    }
}
