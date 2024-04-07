namespace WebApplication2;

public class Order
{
    public string email;
    public string name;
    public string lastName;
    public string phone;
    public string postalCode;
    public string address;
    public string APM;
    public decimal totalPrice;
    public string status;

    public Order(string email, string name, string lastName, string phone, string postalCode, string address, string APM, decimal totalPrice, string status)
    {
        this.email = email;
        this.name = name;
        this.lastName = lastName;
        this.phone = phone;
        this.postalCode = postalCode;
        this.address = address;
        this.APM = APM;
        this.totalPrice = totalPrice;
        this.status = status;
    }
}
