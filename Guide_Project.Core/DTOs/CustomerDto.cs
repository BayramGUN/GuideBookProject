using System.Text.Json.Serialization;
using Guide_Project.Core.Models;

namespace Guide_Project.Core.DTOs;

public class CustomerDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SurName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public CommercialActivity CommercialActivity { get; set; } = new CommercialActivity();

}