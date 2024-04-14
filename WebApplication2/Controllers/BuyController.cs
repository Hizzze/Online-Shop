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
        decimal price = user.cart.Sum(item => item.price * 2);
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
        await Logger.LogAsync("1");
        var user = await users.getUserInfo(User.Identity.Name);
        if (ModelState.IsValid == false)
        {
            model.items = user.cart;
            if (ModelState.IsValid == false)
            {
                var modelErrors = ModelState.SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage);
                // Здесь можно использовать общие ошибки валидации модели
                foreach (var error in modelErrors)
                {
                    await Logger.LogAsync($"Ошибка валидации модели: {error}");
                }

                await Logger.LogAsync(model.email + " " + model.name);
            }
           
        }

        /*if (ModelState.IsValid)
        {
            await Logger.LogAsync("valid");
            
            if (user == null)
            {
                ModelState.AddModelError("", "User not found.");
                return View(model);
            }
            await Logger.LogAsync("email: "+ model.email);
            await user.createOrder(new Order(user.cart, model.email, model.name, model.lastName, 
                model.phone, model.postalCode, model.address, model.APM, model.totalPrice, "Processing"));
        }
        else
        {
            ModelState.AddModelError("", "Please fix the following errors:");
            // Переобновить данные в модели перед возвратом представления
            var user = await users.getUserInfo(User.Identity.Name);
            if (user != null)
            {
                decimal price = user.cart.Sum(item => item.price * 2);
                model.email = user.email;
                model.items = user.cart;
                model.totalPrice = price;
            }
        }*/
        return View(model);

    }
}