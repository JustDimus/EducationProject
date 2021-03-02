﻿using EducationProject.BLL.Interfaces;
using EducationProject.Core.DAL.EF;
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
        
        private IRepository<AccountDBO> accounts;

        public AuthorizationService(IRepository<AccountDBO> accountMapping)
        {
            accounts = accountMapping;

            authorizedAccounts = new Dictionary<string, int>();
        }

        public string AuthorizeAccount(string login, string password)
        {
            var accountId = accounts.Get(a => a.Email == login && a.Password == password, a => a.Id);

            if(accountId == 0)
            {
                return null;
            }

            string token = $"{DateTime.Now}/{login}";

            authorizedAccounts.Add(token, accountId);

            return token;
        }

        public int AuthenticateAccount(string token)
        {
            if(token is null)
            {
                return 0;
            }

            return authorizedAccounts.GetValueOrDefault(token);
        }

        public bool DeauthorizeAccount(string token)
        {
            if(token == null)
            {
                return false;
            }

            return authorizedAccounts.Remove(token);
        }
    }
}
