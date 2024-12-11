using Microsoft.AspNetCore.Identity;

namespace LazaInventory.Infrastructure.Persistence.Entities;

public class AppUser : IdentityUser<int>
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public DateOnly DateOfBirth { get; set; }
}