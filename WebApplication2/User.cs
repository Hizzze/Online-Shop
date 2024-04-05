namespace WebApplication2;

public class User
{
    public string email;
    public string? name;
    public string? phone;
    public string? address;
    public string? postalCode;
    public List<Item> cart;

    public User(string email)
    {
        this.email = email;
    }
    public User(string email, string? name, string? phone, string? address, string? postalCode)
    {
        this.email = email;
        this.name = name;
        this.phone = phone;
        this.address = address;
        this.postalCode = postalCode;
    }
    public override bool Equals(object? obj)
    {
        return obj is User user && email == user.email;
    }
    public override int GetHashCode()
    {
        return email.GetHashCode();
    }
}