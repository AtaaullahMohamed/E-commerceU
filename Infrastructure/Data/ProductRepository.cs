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
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext context;

        public ProductRepository(StoreContext context)
        {
            this.context = context;
        }

        public void AddProductAsync(Product product)
        {
            context.Products.Add(product);

        }

        public void DeleteProductAsync(Product product)
        {
            context.Products.Remove(product);    
        
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await context.Products.FindAsync(id);

        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort)
        {
            var query = context.Products.AsQueryable();

            

            if (!string.IsNullOrWhiteSpace(brand))
            {
                query=query.Where(b => b.Brand == brand);
            }
            if (!string.IsNullOrWhiteSpace(type))
            {
                query=query.Where(t=>t.Type == type);
            }

            query = sort switch
            {
                "priceAsc" => query.OrderBy(x => x.Price),
                "priceDesc" => query.OrderByDescending(x => x.Price),
                            _=> query.OrderBy(x => x.Name) //default value for sorting

            };

            return await query.ToListAsync();
        }

        public bool ProductExists(int id)
        {
            
            return context.Products.Any(x => x.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync()>0;
        }

        public void UpdateProductAsync(Product product)
        {
            context.Entry(product).State = EntityState.Modified;

        }

        public async Task<IReadOnlyList<string>> GetBrandsAsync()
        {
            return await context.Products.Select(x => x.Brand).Distinct().ToListAsync();

        }

        public async Task<IReadOnlyList<string>> GetTypesAsync()
        {
            return await context.Products.Select(x => x.Type).Distinct().ToListAsync();
        }
    }
}
