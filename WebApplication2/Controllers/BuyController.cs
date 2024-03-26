using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers;

public class BuyController : Controller
{
    
    private MainControllerItems itemsObject;

    public BuyController(MainControllerItems itemsObject)
    {
        this.itemsObject = itemsObject;
    }
    
    
    public  IActionResult BuyItem()
    {
         return View();
    }
}