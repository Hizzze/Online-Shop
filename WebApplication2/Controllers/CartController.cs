using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers;

public class CartController : Controller
{
    private readonly MainControllerUsers users;
    private readonly Item item;
    private readonly MainControllerItems items;
    public CartController(MainControllerUsers mainControllerUsers, MainControllerItems mainControllerItems)
    {
        users = mainControllerUsers;
        items = mainControllerItems;
    }
    
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Cart()
    {
        var user = await users.getUserInfo(User.Identity.Name);
        if (user == null)
        {
            return NotFound();
        }
        var model = new CartViewModel()
        {
            item = user.cart
        };
        return View(model);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await users.getUserInfo(User.Identity.Name);
        if (user == null)
        {
            return NotFound();
        }
        var itemToRemove = user.cart.FirstOrDefault(i => i.id == id);
        if (itemToRemove != null)
        {
            await user.removeFromCart(itemToRemove);
        }

        return RedirectToAction("Cart", "Cart");
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> addToCart(int id, int count)
    {
        var user = await users.getUserInfo(User.Identity.Name);
        await user.addItemToCart(items.items.FirstOrDefault(i => i.id == id),id, count);
        return RedirectToAction("Index", "Home");
    }
}