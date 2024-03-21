namespace WebApplication2;

public class Item
{
    public string name;
    public decimal price;
    public int count;
    public string pathImage;
    public string description;
    
    public Item(string name, decimal price, int count, string pathImage, string description)
    {
        this.name = name;
        this.price = price;
        this.count = count;
        this.pathImage = pathImage;
        this.description = description;
    }

    public static void loadItems()
    {
        
    }
}