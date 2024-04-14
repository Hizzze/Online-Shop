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

    public async Task loadCart()
    {
        if (!cart.Any())
        {
            var items = await Database.getUserCart(email);
            foreach (var item in items)
            {
                item.pathImage = await MainControllerItems.getItemPathImageByName(item.name);
                item.description = await MainControllerItems.getItemDescriptionByName(item.name);
            }
            cart = items;
        }
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
            cart.Add(new Item(itemCon.getId(), name, itemCon.getPrice(), count, itemCon.getPathImage(), itemCon.getDescription()));
            await Database.addItemToCart(email, name, count, itemCon.getPrice());
        }
    }

    public async Task createOrder(Order order)
    {
        /*Database.makeOrder(order.email, order.name, order.lastName, 
            order.phone, order.postalCode, order.address, order.APM, order.totalPrice);
        foreach (var item in order.items)
        {
            await Database.makeOrderItems(order.id, item.getId(), item.getUserCount());
        }*/
        await Database.CreateOrderWithItemsAsync(order.email, order.name, order.lastName, order.phone, order.postalCode, 
            order.address, order.APM, order.totalPrice, order.status, order.items);
    }
}