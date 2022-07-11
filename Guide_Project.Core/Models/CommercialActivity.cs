namespace Guide_Project.Core.Models;

public class CommercialActivity
{
    public string Id { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public Customer Customer { get; set; } = new Customer();
    public int CustomerId { get; set; } 
    public string? Employment { get; set; }
}