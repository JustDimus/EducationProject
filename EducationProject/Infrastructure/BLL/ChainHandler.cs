using EducationProject.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.BLL
{
    public class ChainHandler : IChainHandler
    {
        private Dictionary<string, IChain> _chains;

        public ChainHandler(IEnumerable<IChain> Chains)
        {
            _chains = new Dictionary<string, IChain>();

            foreach(var i in Chains)
            {
                _chains.Add(i.Name, i);
            }
        }

        public IChain this[string Command]
        {
            get
            {
                if(_chains.ContainsKey(Command))
                {
                    return _chains[Command];
                }
                else
                {
                    return _chains["Error"];
                }
            }
        }
    }
}
