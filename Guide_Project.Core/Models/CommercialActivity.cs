using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Guide_Project.Core.Models;

public class CommercialActivity
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public int CustomerId { get; set; }
    public string Employment { get; set; } = string.Empty;
}