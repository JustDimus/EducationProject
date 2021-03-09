using EducationProject.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Security.Claims;

namespace EducationProject.Infrastructure.BLL.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private IHttpContextAccessor httpContext;

        public AuthorizationService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContext = httpContextAccessor;
        }

        public int GetAccountId()
        {
            var accountIdValue = this.httpContext.HttpContext.User.Claims
                .Where(p => p.Type == ClaimTypes.NameIdentifier)
                .First()
                .Value;

            return int.Parse(accountIdValue);
        }
    }
}
