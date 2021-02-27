using EducationProject.BLL.Interfaces;
using EducationProject.DAL.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Infrastructure.BLL.EF
{
    /*
    public abstract class BaseConverter<TIn, TOut> : IMapper<TIn, TOut>
    {
        private IRepository<TIn> mapping;

        public BaseConverter(IRepository<TIn> baseMapping)
        {
            this.mapping = baseMapping;
        }

        public abstract TOut Get(TIn entity);

        public IEnumerable<TOut> Get(Expression<Func<TIn, bool>> condition, int pageNumber = 0, int pageSize = 30)
        {
            return this.mapping.Get(condition, pageNumber, pageSize).Select(bm => Get(bm));
        }

        public IEnumerable<TOut> Get(IEnumerable<TIn> collection)
        {
            return collection.Select(bm => Get(bm));
        }
    }
    */
}
