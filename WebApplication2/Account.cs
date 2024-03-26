namespace WebApplication2;

public class Account
{
    private string email;
    private string password;

    public Account(string email, string password)
    {
        this.email = email;
        this.password = Hash.Hash.HashPassword(password);
    }

    public string getEmail()
    {
        return email;
    }

    public async Task registerAccount()
    { 
        await Database.RegisterInDatabase(email, password);
    }
    
}