using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models;

public class BuyViewModel
{
  
    [Required]
    public HashSet<Item> items { get; set; }
    
    [Required]
    public string email { get; set; }
    
    [Required]
    public string name { get; set; }
    
    [Required]
    public string lastName { get; set; }
    
    [Required]
    public string phone { get; set; }
    
    public string postalCode { get; set; }
    
    public string address { get; set; }
    
    public string APM { get; set; }
    
    public decimal totalPrice { get; set; }
    
    public string status { get; set; }
}