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
    [HttpPost]
    public IActionResult AddToCart(string name, int quantity)
    {
        // Ваш код для добавления товара в корзину
        // Используйте name и quantity для добавления соответствующего товара в корзину
        return Ok(); 
    }
}