namespace WebApplication2;

public class MainControllerUsers
{
    public HashSet<User> userList = new HashSet<User>();

    public async Task addUserToList(string email)
    {
        var (name, phone,address,postalCode) = await Database.getUserInfoDatabase(email);
        var user = new User(email, name, phone, address, postalCode);
        await user.loadCart();
        userList.Add(user);
    }
    
    public async Task<User> getUserInfo(string email)
    {
        var user = userList.FirstOrDefault(u => u.email == email);
        if (user == null)
        {
            await addUserToList(email);
            user = userList.FirstOrDefault(u => u.email == email);
        }
        return user;
    }


}