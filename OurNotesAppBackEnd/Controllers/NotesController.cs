using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OurNotesAppBackEnd.Controllers;

[Authorize]
[ApiController]
public class NotesController : ControllerBase
{
    [HttpGet]
    [Route("api/[controller]")]
    public IActionResult GetNotes()
    {
        return Ok("Some Notes Here");
    }
}