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

    [HttpGet]
    public IActionResult GetTodos()
    {
        var listTodo = new List<(int, string)>
        {
            (1, "Buy groceries"),
            (2, "Clean the house"),
            (3, "Read a book")
        };

        return Ok(listTodo);
    }
}