using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class SpecificationEvaluater<T>where T: BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> query,ISpecification<T> spec)
        {
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            if (spec.OrderByDesc !=null)
            {

                query = query.OrderByDescending(spec.OrderByDesc);

            }


            return query;
        }
    }
}
