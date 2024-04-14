namespace WebApplication2;

public class Item
{
    public int id;
    public string name;
    public decimal price;
    public int count;
    public int userCount = 0;
    public string pathImage;
    public string description;

    public Item(string name, decimal price, int userCount)
    {
        this.name = name;
        this.price = price;
        this.userCount = userCount;
    }
    public Item(int id, string name, decimal price, int count, string pathImage, string description)
    {
        this.id = id;
        this.name = name;
        this.price = price;
        this.count = count;
        this.pathImage = pathImage;
        this.description = description;
    }
    public override bool Equals(object? obj)
    {
        return obj is Item item && name == item.name;
    }
    public override int GetHashCode()
    {
        return name.GetHashCode();
    }
    public string getName()
    {
        return this.name;
    }

    public decimal getPrice()
    {
        return this.price;
    }

    public int getCount()
    {
        return this.count;
    }

    public void setCount(int count)
    {
        this.count = count;
    }

    public string getPathImage()
    {
        return pathImage;
    }

    public int getId()
    {
        return id;
    }

    public int getUserCount()
    {
        return userCount;
    }
    public string getDescription()
    {
        return description;
    }
}