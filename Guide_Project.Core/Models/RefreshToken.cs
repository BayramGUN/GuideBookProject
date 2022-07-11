namespace Guide_Project.Core.Models;
public class RefreshToken
{
    public string UserId { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
}