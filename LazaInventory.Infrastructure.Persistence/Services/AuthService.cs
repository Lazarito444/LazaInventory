using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LazaInventory.Core.Application.Dtos.Auth;
using LazaInventory.Core.Application.Interfaces.Services;
using LazaInventory.Core.Domain.Options;
using LazaInventory.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LazaInventory.Infrastructure.Persistence.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly JWTOptions _jwtOptions;

    public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IOptions<JWTOptions> jwtOptions)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
    {
        LoginResponse loginResponse = new LoginResponse();

        AppUser? appUser = await _userManager.FindByNameAsync(loginRequest.UserName);
        
        if (appUser == null)
        {
            loginResponse.HasError = true;
            loginResponse.Error = $"There's no user with username '{loginRequest.UserName}'";
            return loginResponse;
        }
        
        SignInResult signInResult = await _signInManager.PasswordSignInAsync(loginRequest.UserName, loginRequest.Password, true, false);

        if (!signInResult.Succeeded)
        {
            loginResponse.HasError = true;
            loginResponse.Error = $"Wrong credentials for user '{loginRequest.UserName}'";
            return loginResponse;
        }

        string jsonToken = GenerateToken(appUser);

        loginResponse.HasError = false;
        loginResponse.Id = appUser.Id;
        loginResponse.UserName = appUser.UserName;
        loginResponse.Email = appUser.Email;
        loginResponse.JsonWebToken = jsonToken;
        
        return loginResponse;
    }

    public async Task<RegisterResponse> SignInAsync(RegisterRequest registerRequest)
    {
        RegisterResponse registerResponse = new RegisterResponse();

        if (registerRequest.Password != registerRequest.ConfirmPassword)
        {
            registerResponse.HasError = true;
            registerResponse.Error = "The passwords are not identical";
            return registerResponse;
        }

        AppUser? userWithSameUserName = await _userManager.FindByNameAsync(registerRequest.UserName);

        if (userWithSameUserName != null)
        {
            registerResponse.HasError = true;
            registerResponse.Error = $"The username '{registerRequest.UserName}' is already taken";
            return registerResponse;
        }

        AppUser? userWithSameEmail = await _userManager.FindByEmailAsync(registerRequest.Email);

        if (userWithSameEmail != null)
        {
            registerResponse.HasError = true;
            registerResponse.Error = $"The email '{registerRequest.Email}' is already taken";
            return registerResponse;
        }

        AppUser appUser = new AppUser
        {
            UserName = registerRequest.UserName,
            Email = registerRequest.Email,
            Name = registerRequest.Name,
            LastName = registerRequest.LastName,
            NormalizedEmail = registerRequest.Email.ToUpper(),
            EmailConfirmed = true,
            DateOfBirth = registerRequest.DateOfBirth,
            NormalizedUserName = registerRequest.UserName.ToUpper(),
        };

        IdentityResult result = await _userManager.CreateAsync(appUser, registerRequest.Password);

        if (!result.Succeeded)
        {
            registerResponse.HasError = true;
            registerResponse.Error = $"A wild error appeared! Please try again later";
            return registerResponse;
        }
        
        registerResponse.HasError = false;
        return registerResponse;
    }

    private string GenerateToken(AppUser appUser)
    {
        Claim[] claims =
        [
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Aud, _jwtOptions.Audience),
            new Claim(JwtRegisteredClaimNames.Sub, appUser.UserName!),
            new Claim("uid", appUser.Id.ToString())
        ];

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            claims: claims,
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}