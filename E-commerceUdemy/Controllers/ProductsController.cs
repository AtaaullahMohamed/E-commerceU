using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Api.RequestHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{



    public class ProductsController(IUnitOfWork unit) : BaseApiController
    {
     

        [HttpGet]
       public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery]ProductSpecParams specParams)
        {
            var spec = new ProductSpecification(specParams);
        
        return await CreatePagedResult(unit.Repository<Product>(),spec,specParams.PageIndex,specParams.PageSize);      
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await unit.Repository<Product>().GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            unit.Repository<Product>().Add(product);

            if (await unit.Repository<Product>().SaveAllAsync())
            {
                return CreatedAtAction("GetProduct", new {id=product.Id},product); 
            }
            
            return BadRequest("Problem Creating Product!");

        }
        [Authorize(Roles = "Admin")]

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id,Product product)
        {
            if (product.Id !=id || !ProductExists(id))
            {
                return BadRequest("Can't update this product");
            }

            unit.Repository<Product>().Update(product);
            
            if (await unit.Complete())
            {
                return NoContent();
            }

            return BadRequest("Problem Updating Product!");
        }

        [Authorize(Roles = "Admin")]

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
        var product = unit.Repository<Product>().GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            unit.Repository<Product>().Remove(await product);
            if (await unit.Complete())
            {
                return NoContent();
            }

            return BadRequest("Problem Deleting Product!");
        }


        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            var brands = await unit.Repository<Product>().GetDistinctBrandsAsync();
            return Ok(brands);
        }



        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            var types = await unit.Repository<Product>().GetDistinctTypesAsync();
            return Ok(types);

        }

        private bool ProductExists(int id)
        {
            return unit.Repository<Product>().Exists(id);
        }

    }

}
