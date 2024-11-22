using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SecuredApi.AddController;

public class ProtectedController : ControllerBase
{
    public ProtectedController()
    {

    }

    [HttpGet("TestAuth")]
    [Authorize]
    public IActionResult TestAuth()
    {
        return Ok("Authenticated.");
    }
}