namespace LazaInventory.Core.Domain.Options;

public class JWTOptions
{
    public const string JWTSettings = "JWTSettings";
    
    public string Key { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
}