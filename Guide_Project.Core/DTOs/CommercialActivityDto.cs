using Guide_Project.Core.Models;

namespace Guide_Project.Core.Models;

public class CommercialActivityDto
{
    public string Id { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int CustomerId { get; set; } 
    public Customer Customer { get; set; } = new Customer();
    public string Employment { get; set; } = string.Empty;
}