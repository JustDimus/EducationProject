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


        public ServiceResultParser()
        {
            this.blMessages = new Dictionary<string, string>();

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
