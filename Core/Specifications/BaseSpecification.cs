﻿using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class BaseSpecification<T>(Expression<Func<T, bool>>? criteria) : ISpecification<T>//Primary constructor
    {
        protected BaseSpecification() : this(null) { }
        public Expression<Func<T, bool>>? Criteria => criteria;

        public Expression<Func<T, object>>? OrderBy {  get; private set; }

        public Expression<Func<T, object>>? OrderByDesc { get;  private set; }

        public int Take { get; private set; }

        public int Skip {  get; private set; }     

        public bool IsPagingEnabled {  get; private set; }

        public IQueryable<T> ApplyCriteria(IQueryable<T> query)
        {
            
            if(criteria != null)
            {
                query = query.Where(criteria);
            }
            return query;

        }

        protected void AddOrderBy(Expression<Func<T, object>>?OrderByExpression)
        {
            OrderBy = OrderByExpression;
        }
        protected void AddOrderByDescending(Expression<Func<T, object>>? OrderByDescExpression)
        {
            OrderByDesc = OrderByDescExpression;
        }

        protected void ApplyPaging(int skip , int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }
    }
}
