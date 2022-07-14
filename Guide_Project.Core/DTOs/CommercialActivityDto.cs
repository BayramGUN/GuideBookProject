using System.Text.Json.Serialization;
using Guide_Project.Core.Models;

namespace Guide_Project.Core.Models;

public class CommercialActivityDto
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public int CustomerId { get; set; }
    public string Employment { get; set; } = string.Empty;
}