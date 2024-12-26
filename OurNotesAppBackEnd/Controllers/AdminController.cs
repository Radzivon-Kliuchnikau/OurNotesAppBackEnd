using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OurNotesAppBackEnd.Utils;

namespace OurNotesAppBackEnd.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var usersList = new string[]
        {
            "User One",
            "User Two",
            "User Three"
        };
        
        return Ok(usersList);
    }
}