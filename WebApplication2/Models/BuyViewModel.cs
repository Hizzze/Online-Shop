using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models;

public class BuyViewModel
{
    [Required]
    public HashSet<Item> items = new HashSet<Item>();
    [Required]
    public string email;
    
    [Required]
    public string name;
    
    [Required]
    public string lastName;
    
    [Required]
    public string phone;
    
    public string postalCode;
    
    public string address;
    
    public string APM;
    
    public decimal totalPrice;
    
    public string status;
}