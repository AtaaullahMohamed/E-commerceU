﻿using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class GenericRepository<T>(StoreContext context) : IGenericRepository<T> where T : BaseEntity
    {
        public void Add(T entity)
        {
        context.Set<T>().Add(entity); 
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            var query=context.Set<T>().AsQueryable();
            query =spec.ApplyCriteria(query);
            return await query.CountAsync();
        }

        public bool Exists(int id)
        {
           return context.Set<T>().Any(x => x.Id == id); 

        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await context.Set<T>().FindAsync(id);

        }

        public async Task<T?> GetEntityWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
        return await context.Set<T>().ToListAsync();  
        
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();

        }

        public void Remove(T entity)
        {
            context.Set<T>().Remove(entity);

        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync()>0;
        }

        public void Update(T entity)
        {
            context.Set<T>().Attach(entity);
            context.Entry(entity).State= EntityState.Modified;
        }


        public async Task<IReadOnlyList<string>> GetDistinctBrandsAsync()
        {
            return await context.Set<Product>()
                                 .Select(p => p.Brand)
                                 .Distinct()
                                 .ToListAsync();
        }

        // Implementing logic to get distinct types
        public async Task<IReadOnlyList<string>> GetDistinctTypesAsync()
        {
            return await context.Set<Product>()
                                 .Select(p => p.Type)
                                 .Distinct()
                                 .ToListAsync();
        }



        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluater<T>.GetQuery(context.Set<T>().AsQueryable(), spec);
        }
    }
}
