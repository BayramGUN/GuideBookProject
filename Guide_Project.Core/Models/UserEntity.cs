using Microsoft.AspNetCore.Identity;

namespace Guide_Project.Core.Models;

public class UserEntity : IdentityUser<int>
{
    public string Department { get; set; } = string.Empty;
}