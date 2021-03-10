using Microsoft.Extensions.Configuration;
using MvcInterface.ServiceResultController.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcInterface.ServiceResultController.Implementations
{
    public class ServiceResultParser : IServiceResultParser
    {
        private Dictionary<string, string> blMessages;

        public ServiceResultParser(IConfiguration configuration)
        {
            blMessages = configuration.GetSection("BlMessages").GetChildren().ToDictionary(p => p.Key, p => p.Value);
        }

        public string this[string value]
        {
            get
            {
                if(!blMessages.ContainsKey(value))
                {
                    return value;
                }
                else
                {
                    return blMessages[value];
                }
            }
        }
    }
}
