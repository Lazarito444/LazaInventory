using LazaInventory.Core.Application.Dtos.Auth;

namespace LazaInventory.Core.Application.Interfaces.Services;

public interface IAuthService
{
    Task SignOutAsync();
    Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
    Task<RegisterResponse> SignInAsync(RegisterRequest registerRequest);
}