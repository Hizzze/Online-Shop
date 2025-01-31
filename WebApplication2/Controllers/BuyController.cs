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
        var user = await users.getUserInfo(User.Identity.Name);
        decimal price = user.cart.Sum(item => item.price * item.userCount);
        var model = new BuyViewModel()
        {
            email = user.email,
            items = user.cart,
            totalPrice = price
        };
        return View(model);
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> createOrder(BuyViewModel model)
    {
        var user = await users.getUserInfo(User.Identity.Name);
        if (ModelState.IsValid)
        {
            if (user == null)
            {
                ModelState.AddModelError("", "User not found.");
                return View(model);
            }
            decimal price = user.cart.Sum(item => item.price * item.userCount);
            /*if (await user.createOrder(new Order(user.cart, model.email, model.name, model.lastName,
                    model.phone, model.postalCode, model.address, model.APM, price, "Processing")))*/
            if (await itemsObject.onBuyItem(user, new Order(user.cart, model.email, model.name, model.lastName,
                    model.phone, model.postalCode, model.address, model.APM, price, "Processing")))
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        else
        {
            if (user != null)
            {
                decimal price = user.cart.Sum(item => item.price * 2);
                model.email = user.email;
                model.items = user.cart;
                model.totalPrice = price;
            }
        }
        return View(model);

    }
}