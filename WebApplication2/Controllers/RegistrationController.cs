using Microsoft.AspNetCore.Authorization;
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
    public async Task<IActionResult> Registration(RegistrationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (await Database.IsUserRegistered(model.Email))
        {
            ModelState.AddModelError(string.Empty, "Użytkownik o podanym adresie email już istnieje.");
            return View(model);
            
        }
        else
        {
            Account account = new Account(model.Email, model.Password);
            account.registerAccount();
            return View(model);
        }
    }

   
}