namespace WebApplication2.Models;

public class UserViewModel
{
    public string Email { get; set; }
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? PostalCode { get; set; }
    public string? APM { get; set; }

    public List<Order> Orders = new List<Order>();
}