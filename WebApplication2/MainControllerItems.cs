namespace WebApplication2;

public class MainControllerItems
{
    public List<Item> items = new List<Item>();
    public static List<Item> itemsSync = new List<Item>();
    private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
    public MainControllerItems()
    {
        items = Database.getItemsList();
        itemsSync = items;
    }
    public void onReload()
    {
        items = Database.getItemsList();
        itemsSync = items;
    }

    public static async Task<string> getItemPathImageById(int id)
    {
        Item item = itemsSync.FirstOrDefault(i => i.id == id);
        if (item != null)
        {
            return item.pathImage;
        }
        return null;
    }

    public static async Task<Item> buildItem(Item item)
    {
        var itemSync = itemsSync.FirstOrDefault(i => i.id == item.id);
        if (string.IsNullOrEmpty(item.name))
        {
            await Logger.LogAsync("Name sync: " + itemSync.name);
            item.name = itemSync?.name;
        }

        if (item.price == 0 || item.price == null)
        {
            await Logger.LogAsync("Price sync: " + itemSync.price);
            item.price = itemSync.price;
        }

        if (string.IsNullOrEmpty(item.pathImage))
        {
            item.pathImage = itemSync.pathImage;
        }

        if (string.IsNullOrEmpty(item.description))
        {
            item.description = itemSync.description;
        }
        return item;
    }
    public static async Task<string> getItemNameById(int id)
    {
        Item item = itemsSync.FirstOrDefault(i => i.id == id);
        if (item != null)
        {
            return item.name;
        }
        return null;
    }
    public static async Task<string> getItemDescriptionById(int id)
    {
        Item item = itemsSync.FirstOrDefault(i => i.id == id);
        if (item != null)
        {
            return item.description;
        }
        return null;
    }
    public async Task<bool> onBuyItem(User user, Order order)
    {
        await semaphore.WaitAsync();
        try
        {
            foreach (var itemOrder in order.items)
            {
                var item = items.FirstOrDefault(i => i.id == itemOrder.id);
                if (item.getCount() < itemOrder.getUserCount())
                {
                    return false;
                }
            }
            if (await user.createOrder(order))
            {
                onReload();
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            await Logger.LogAsync("Error on buy item: " + ex.Message, Logger.LogLevel.Error);
            return false;
        }
        finally
        {
            semaphore.Release();
        }
    }
}
