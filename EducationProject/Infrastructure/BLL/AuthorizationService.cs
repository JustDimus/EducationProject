using EducationProject.Core.Models;
using EducationProject.DAL.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.BLL
{
    public class AuthorizationService
    {
        private ConcurrentDictionary<string, int> authorizedAccounts;
        
        private IRepository<Account> accountRepository;

        public AuthorizationService(IRepository<Account> accountMapping)
        {
            this.accountRepository = accountMapping;

            this.authorizedAccounts = new ConcurrentDictionary<string, int>();
        }

        public async Task<string> AuthorizeAccountAsync(string email, string password)
        {
            var accountId = await this.accountRepository.GetAsync(a => a.Email == email && a.Password == password, a => a.Id);

            if (accountId == 0)
            {
                return null;
            }

            string token = $"{DateTime.Now}/{email}";

            this.authorizedAccounts.TryAdd(token, accountId);

            return token;
        }

        public Task<int> AuthenticateAccountAsync(string token)
        {
            return Task.Run(() =>
                token == null ? 0 : this.authorizedAccounts.GetValueOrDefault(token));
        }

        public Task<bool> DeauthorizeAccountAsync(string token)
        {
            return Task.Run(() => 
                token == null ? false : this.authorizedAccounts.TryRemove(token, out _));
        }
    }
}
