namespace WebApplication2;

public class Item
{
    public string name;
    public decimal price;
    public int count;
    public string pathImage;
    
    public Item(string name, decimal price, int count, string pathImage)
    {
        this.name = name;
        this.price = price;
        this.count = count;
        this.pathImage = pathImage;
    }

    public static void loadItems()
    {
        
    }
}