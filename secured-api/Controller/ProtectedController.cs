using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SecuredApi.AddController;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProtectedController : ControllerBase
{
    [HttpGet(nameof(TestAuth))]
    public IActionResult TestAuth() => Ok("Authenticated.");

    [HttpGet(nameof(GetTodos))]
    public IActionResult GetTodos()
    {
        var listTodo = new List<object>
        {
            new { id = 1, name = "Buy groceries" },
            new { id = 2, name = "Clean the house" },
            new { id = 3, name = "Read a book" },
        };

        return Ok(listTodo);
    }

    [AllowAnonymous]
    [HttpGet(nameof(GetLog))]
    public async Task<IActionResult> GetLog()
    {
        if (System.IO.File.Exists(FileLogger.LogFilePath))
        {
            var logs = await System.IO.File.ReadAllTextAsync(FileLogger.LogFilePath);
            return Ok(logs);
        }

        return NotFound("Log file not found.");
    }
}