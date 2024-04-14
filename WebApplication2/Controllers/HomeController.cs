using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
namespace WebApplication2.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MainControllerItems itemsObject;
        

    public HomeController(ILogger<HomeController> logger, MainControllerItems itemsObject)
    {
        _logger = logger;
        this.itemsObject = itemsObject;
    }
    
    public async Task<IActionResult> Index()
    {
        var model = new HomeViewModel
        {
            Items = itemsObject.items
        };
//12345
        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public async Task<int> Plus(int count)
    {
        return count += 1;
    }
}