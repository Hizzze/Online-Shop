using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models;

public class BuyViewModel
{
    public HashSet<Item> items { get; set; }
    public string email { get; set; }
    public string name { get; set; }
    public string lastName { get; set; }
    public string phone { get; set; }
    public string postalCode { get; set; }
    public string address { get; set; }
    public string APM { get; set; }
    public decimal totalPrice { get; set; }
    
}