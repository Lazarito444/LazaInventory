using LazaInventory.Core.Application.Dtos.Auth;
using LazaInventory.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LazaInventory.Presentation.Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest loginRequest)
    {
        LoginResponse loginResponse = await _authService.LoginAsync(loginRequest);
        return Ok(loginResponse);
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignInAsync([FromBody] RegisterRequest registerRequest)
    {
        RegisterResponse registerResponse = await _authService.SignInAsync(registerRequest);
        
        return Ok(registerResponse);
    }

    [HttpPost("sign-out")]
    public async Task<IActionResult> SignOutAsync()
    {
        await _authService.SignOutAsync();
        return NoContent();
    }
}