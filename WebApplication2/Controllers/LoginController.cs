using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication2.Controllers;

public class LoginController : Controller
{
    private readonly MainControllerUsers mainControllerUsers;

    public LoginController(MainControllerUsers mainControllerUsers)
    {
        this.mainControllerUsers = mainControllerUsers;
    }
    [HttpGet]
    public IActionResult Login()
    {
        return View(); // Показывает страницу с формой входа
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        if (await Database.verifyUserData(email, password))
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, email) };
            var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync("CookieAuth", claimsPrincipal);
            mainControllerUsers.addUserToList(email);
            return RedirectToAction("Index", "Home"); // Перенаправление после успешного входа
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View();
        }
    }
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("CookieAuth");
        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var user = await mainControllerUsers.getUserInfo(User.Identity.Name);
        if (user == null)
        {
            return NotFound();
        }
        var model = new UserViewModel
        {
            Email = user.email,
            Name = user.name,
            Phone = user.phone,
            Address = user.address,
            PostalCode = user.postalCode,

        };
        return View(model);
    }
    
}