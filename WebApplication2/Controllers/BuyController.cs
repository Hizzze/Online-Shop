using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers;

public class BuyController : Controller
{
    
    private MainControllerItems itemsObject;
    private MainControllerUsers users;
    public BuyController(MainControllerItems itemsObject, MainControllerUsers users)
    {
        this.itemsObject = itemsObject;
        this.users = users;
    }
    
    
    public  IActionResult BuyItem()
    {
         return View();
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> addToCart(string name, int count)
    {
        
        var user = await users.getUserInfo(User.Identity.Name);
        await user.addItemToCart(itemsObject.items.FirstOrDefault(i => i.name == name),name, count);
        return RedirectToAction("Cart", "Cart");
    }
}