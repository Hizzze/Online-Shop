using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
namespace WebApplication2.Controllers;

public class RegistrationController : Controller
{
    
    public IActionResult Registration()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Registration(RegistrationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        Account account = new Account(model.Email, model.Password);
        account.registerAccount();
        return View(model);
    }
   
}