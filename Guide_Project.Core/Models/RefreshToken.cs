namespace Guide_Project.Core.Models;
public class RefreshToken
{
    public int UserId { get; set; }
    public string Code { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
}