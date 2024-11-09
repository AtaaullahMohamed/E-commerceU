using Core.Entities;
using E_commerceUdemy.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    public class BuggyController : BaseApiController
    {

        [HttpGet("unauthorized")]
        public IActionResult GetUnathorized()
        {
            return Unauthorized();
        }

        [HttpGet("badrequest")]
        public IActionResult GetBadRequest()
        {
            return BadRequest("Not a good request");
        }

        [HttpGet("notfound")]
        public IActionResult GetNotFound()
        {
            return NotFound();
        }

        [HttpGet("internalerror")]
        public IActionResult GetInternalError()
        {
            throw new Exception("This is a test exception");
        }

        [HttpPost("validationerror")]
        public IActionResult GetValidationError(CreateProductDTO product)
        {
            return Ok();
        }


        [Authorize]
        [HttpGet("secret")]
        public IActionResult GetSecret()
        {
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok("Hello " + name + " With Id of " + id);
        }


    }
}
