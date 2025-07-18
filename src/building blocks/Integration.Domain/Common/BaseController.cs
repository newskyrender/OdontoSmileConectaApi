using Microsoft.AspNetCore.Mvc;

namespace Integration.Domain.Common
{
    public class BaseController : ControllerBase
    {
        protected IActionResult HandleResult<T>(BaseResponse<T> response)
        {
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}
