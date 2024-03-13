using Application.Core;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
       protected ActionResult HandleResult<T>(Result<T> result)
        {
            if (result == null) return null;
            if (result.IsSuccess && result.Value != null) return Ok(result.Value);

            if (result.IsSuccess && result.Value is null) return NotFound();

            return BadRequest(result.ErrorMessage);


        }
    }
}
