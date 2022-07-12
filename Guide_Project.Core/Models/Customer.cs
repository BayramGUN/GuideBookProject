using Microsoft.EntityFrameworkCore;

namespace Guide_Project.Core.Models;
public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SurName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int CommercialActivityId { get; set; }
    public CommercialActivity CommercialActivity { get; set; } = new CommercialActivity();
}