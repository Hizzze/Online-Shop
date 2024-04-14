using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication2;

public class User
{
    public string email;
    public string? name;
    public string? lastName;
    public string? phone;
    public string? address;
    public string? postalCode;
    public string? APM;
    public HashSet<Item> cart = new HashSet<Item>();
    public List<Order> orders = new List<Order>();
    public User(string email)
    {
        this.email = email;
    }
    public User(string email, string? name, string? lastName, string? phone, string? address, string? postalCode, string? APM)
    {
        this.email = email;
        this.name = name;
        this.lastName = lastName;
        this.phone = phone;
        this.address = address;
        this.postalCode = postalCode;
        this.APM = APM;
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
                item.name = await MainControllerItems.getItemNameById(item.id);
                item.pathImage = await MainControllerItems.getItemPathImageById(item.id);
                item.description = await MainControllerItems.getItemDescriptionById(item.id);
            }
            cart = items;
        }
    }

    public async Task loadOrders()
    {
        orders = await Database.getOrdersForUser(email);
    }
    public async Task removeFromCart(Item item)
    {
        await Database.removeFromUserCart(email, item.id);
        cart.Remove(item);
    }
    public async Task addItemToCart(Item itemCon, int id, int count)
    {
        var item = cart.FirstOrDefault(i => i.id == id);
        if (item != null)
        {
            item.setCount(count);
            await Database.updateItemInCart(email, id, count);
        }
        else if(item == null)
        {
            await Logger.LogAsync(email + " " + name + " " + count + " " + itemCon.getPrice());
            cart.Add(new Item(itemCon.getId(), itemCon.getName(), itemCon.getPrice(), count, itemCon.getPathImage(), itemCon.getDescription()));
            await Database.addItemToCart(email, id, count, itemCon.getPrice());
        }
    }
    public async Task createOrder(Order order)
    {
        if (!name.Equals(order.name) || !lastName.Equals(order.lastName) || !phone.Equals(order.phone)
            || !postalCode.Equals(order.postalCode) || !address.Equals(order.address) || !APM.Equals(order.APM))
        {
            name = order.name;
            lastName = order.lastName;
            phone = order.phone;
            postalCode = order.postalCode;
            address = order.address;
            APM = order.APM;
            await Database.updateUserInfo(email, name, lastName, phone, postalCode, address, APM);
        }
        await Database.CreateOrderWithItemsAsync(order.email, order.name, order.lastName, order.phone, order.postalCode, 
            order.address, order.APM, order.totalPrice, order.status, order.items);
        loadOrders();
    }
}