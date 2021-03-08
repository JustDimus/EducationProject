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

        private string defaultErrorMessage;

        public ServiceResultParser(string defaultErrorMessage = "Неизвестная ошибка")
        {
            this.blMessages = new Dictionary<string, string>();

            this.defaultErrorMessage = defaultErrorMessage;
        }

        public string this[string value]
        {
            get
            {
                if(!blMessages.ContainsKey(value))
                {
                    return defaultErrorMessage;
                }
                else
                {
                    return blMessages[value];
                }
            }
        }
    }
}
