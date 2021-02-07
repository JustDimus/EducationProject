using ADODataContext.DataSets;
using ADODataContext.Interfaces;
using EducationProject.Core;
using System;
using System.Collections.Generic;
using System.Text;
using WorkWithADO;

namespace ADODataContext.DataContext
{
    public class ADOContext
    {
        private Dictionary<Type, IDbSet> entityDictionary;

        private string connectionString;

        public ADOContext(string dbConnectionString)
        {
            connectionString = dbConnectionString;

            entityDictionary = new Dictionary<Type, IDbSet>();
        }

        public IDbSet<T> Entity<T>() where T : BaseEntity
        {
            IDbSet result = null;

            if(entityDictionary.TryGetValue(typeof(T), out result) == false)
            {
                result = new BaseDbSet<T>(connectionString, new Converters.LabdaConverter<T>());
                entityDictionary.Add(typeof(T), result);
            }

            return (IDbSet<T>)result;
        }

        public void Save()
        {
            foreach(var entity in entityDictionary.Values)
            {
                entity.Save();
            }
        }
    }
}
