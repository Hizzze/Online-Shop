using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication2;

public class User
{
    public string email;
    public string? name;
    public string? phone;
    public string? address;
    public string? postalCode;
    public HashSet<Item> cart = new HashSet<Item>();
    public List<Order> orders = new List<Order>();
    public User(string email)
    {
        this.email = email;
    }
    public User(string email, string? name, string? phone, string? address, string? postalCode)
    {
        this.email = email;
        this.name = name;
        this.phone = phone;
        this.address = address;
        this.postalCode = postalCode;
        
    }
    public override bool Equals(object? obj)
    {
        return obj is User user && email == user.email;
    }
    public override int GetHashCode()
    {
        return email.GetHashCode();
    }
    
    public async Task addItemToCart(Item itemCon, string name, int count)
    {
        var item = cart.FirstOrDefault(i => i.name == name);
        if (item != null)
        {
            item.setCount(count);
            await Database.updateItemInCart(email, name, count);
        }
        else if(item == null)
        {
            await Logger.LogAsync(email + " " + name + " " + count + " " + itemCon.getPrice());
            cart.Add(new Item(name, itemCon.getPrice(), count, itemCon.getPathImage(), itemCon.getDescription()));
            await Database.addItemToCart(email, name, count, itemCon.getPrice());
        }
    }

    public async Task createOrder(Order order)
    {
        orders.Add(order);
        Database.makeOrder(order.email, order.name, order.lastName, 
            order.phone, order.postalCode, order.address, order.APM, "test", 5, 150);
    }
}