using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers;

public class CartController : Controller
{
    private readonly MainControllerUsers _mainControllerUsers;
    private readonly Item item;

    public CartController(MainControllerUsers mainControllerUsers)
    {
        _mainControllerUsers = mainControllerUsers;
    }
    
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Cart()
    {
        
        var user = await _mainControllerUsers.getUserInfo(User.Identity.Name);
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
    public async Task<IActionResult> Delete(string name)
    {
        var user = await _mainControllerUsers.getUserInfo(User.Identity.Name);
        if (user == null)
        {
            return NotFound();
        }

        var itemToRemove = user.cart.FirstOrDefault(i => i.name == name);
        if (itemToRemove != null)
        {
            user.cart.Remove(itemToRemove);

        }

        return RedirectToAction("Cart");
    }
}