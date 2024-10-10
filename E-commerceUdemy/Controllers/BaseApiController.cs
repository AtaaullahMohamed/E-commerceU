﻿using Core.Entities;
using Core.Interfaces;
using E_commerceUdemy.RequestHelpers;
using Microsoft.AspNetCore.Mvc;

namespace E_commerceUdemy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
      protected async Task<ActionResult> CreatePagedResult<T>(IGenericRepository<T>repo,ISpecification<T>spec,int pageIndex,int pageSize)where T : BaseEntity
        {
            var items = await repo.ListAsync(spec);
            var count = await repo.CountAsync(spec);

            var pagination = new Pagination<T>(pageIndex,pageSize,count,items);

            return Ok(pagination);
        }
    }
}