namespace LazaInventory.Core.Application.Dtos.Auth;

public class LoginResponse
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public bool IsVerified { get; set; } = true;
    public bool HasError { get; set; }
    public string? Error { get; set; }
    public string? JsonWebToken { get; set; }
}