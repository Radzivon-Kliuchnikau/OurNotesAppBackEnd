using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OurNotesAppBackEnd.Identity;
using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signinManager;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signinManager)
    {
        _userManager = userManager;
        _signinManager = signinManager;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = new AppUser()
        {
            Email = registerModel.Email,
            UserName = registerModel.UserName
        };

        var result = await _userManager.CreateAsync(user, registerModel.Password);

        if (result.Succeeded)
        {
            return Ok("User registered successfully");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }

        return BadRequest(ModelState);
    }

    [Authorize]
    [HttpGet("checkAuth")]
    public async Task<IActionResult> CheckAuth()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        var name = User.FindFirstValue(ClaimTypes.Name);

        return Ok(new { Email = email, Name = name });
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signinManager.SignOutAsync();
        return Ok("User logged out successfully");
    }
}