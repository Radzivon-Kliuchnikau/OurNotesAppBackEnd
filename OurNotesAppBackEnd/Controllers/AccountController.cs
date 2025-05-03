using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OurNotesAppBackEnd.Dtos.Account;
using OurNotesAppBackEnd.Identity;
using OurNotesAppBackEnd.Interfaces;
using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signinManager;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AccountController(
        UserManager<AppUser> userManager, SignInManager<AppUser> signinManager, ITokenService tokenService,
        IMapper mapper)
    {
        _userManager = userManager;
        _signinManager = signinManager;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationModelDto registrationModelDto)
    {
        var user = _mapper.Map<AppUser>(registrationModelDto);
        var alreadyExistedUser =
            await _userManager.Users.FirstOrDefaultAsync(u => u.Email == registrationModelDto.Email.ToLower());
        if (alreadyExistedUser != null)
        {
            return BadRequest("User already exists");
        }

        var createdUserResult = await _userManager.CreateAsync(user, registrationModelDto.Password);
        if (!createdUserResult.Succeeded)
        {
            var errors = createdUserResult.Errors.Select(e => e.Description);

            return BadRequest(new RegistrationResponseDto { Errors = errors });
        }

        var roleResult = await _userManager.AddToRoleAsync(user, "User");
        if (!roleResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);
            var roleErrors = createdUserResult.Errors.Select(e => e.Description);

            return BadRequest(new RegistrationResponseDto { Errors = roleErrors });
        }

        return Created();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == loginRequestDto.Email.ToLower());
        if (user == null)
        {
            return Unauthorized();
        }

        var result = await _signinManager.CheckPasswordSignInAsync(user, loginRequestDto.Password, false);
        if (!result.Succeeded)
        {
            return Unauthorized();
        }

        var loggedInUser = new LoginResponseDto
        {
            UserName = user.UserName,
            Email = user.Email,
            Token = _tokenService.CreateToken(user)
        };

        return Ok(loggedInUser);
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