using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers;

public class AuthorizationController : Controller
{
    // GET
    public IActionResult Auth()
    {
        return View();
    }
    
   
}