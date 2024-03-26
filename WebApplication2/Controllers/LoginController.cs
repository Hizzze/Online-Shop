using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using System.Security.Claims;

namespace WebApplication2.Controllers;

public class LoginController : Controller
{
    [HttpGet]
    public IActionResult Login()
    {
        return View(); // Показывает страницу с формой входа
    }

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        
        if (true)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username)
                
            };

            var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync("CookieAuth", claimsPrincipal);

            return RedirectToAction("Index", "Home"); // Перенаправляет пользователя после успешного входа
        }

        return View(); // В случае ошибки возвращаемся на страницу входа
    }
}