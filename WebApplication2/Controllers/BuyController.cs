using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

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
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> createOrder()
    {
        var model = new BuyViewModel()
        {
            email = User.Identity.Name
        };
        return View(model);
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> createOrder(BuyViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await users.getUserInfo(User.Identity.Name);
            if (user == null)
            {
                ModelState.AddModelError("", "User not found.");
                return View(model);
            }
            user.createOrder(new Order(model.email, model.name, model.lastName, model.phone, model.postalCode, model.address, model.APM, 150, "Processing"));
        }

        return Redirect("https://google.com");
    }
}