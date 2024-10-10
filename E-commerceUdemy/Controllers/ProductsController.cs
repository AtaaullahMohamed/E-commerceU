﻿using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Api.RequestHelpers;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{



    public class ProductsController(IGenericRepository<Product> repo) : BaseApiController
    {
     

        [HttpGet]
       public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery]ProductSpecParams specParams)
        {
            var spec = new ProductSpecification(specParams);
        
        return await CreatePagedResult(repo,spec,specParams.PageIndex,specParams.PageSize);      
        }

        [HttpGet("{id:int}")]

        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await repo.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            repo.Add(product);

            if (await repo.SaveAllAsync())
            {
                return CreatedAtAction("GetProduct", new {id=product.Id},product); 
            }
            
            return BadRequest("Problem Creating Product!");

        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id,Product product)
        {
            if (product.Id !=id || !ProductExists(id))
            {
                return BadRequest("Can't update this product");
            }

            repo.Update(product);
            
            if (await repo.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Problem Updating Product!");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
        var product = repo.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            repo.Remove(await product);
            if (await repo.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Problem Deleting Product!");
        }


        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            return Ok();
        }



        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            return Ok();
        }

        private bool ProductExists(int id)
        {
            return repo.Exists(id);
        }

    }

}
