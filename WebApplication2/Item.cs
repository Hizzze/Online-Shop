namespace WebApplication2;

public class Item
{
    private string name;
    private double price;
    private int count;
    private string pathImage;

    public Item(string name, double price, int count, string pathImage)
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