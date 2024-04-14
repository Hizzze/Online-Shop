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
    public async Task<bool> onBuyItem(string name, int count)
    {
        await semaphore.WaitAsync();
        try
        {
            foreach (var item in items)
            {
                if (item.getName() == name)
                {
                    if (item.getCount() >= count)
                    {
                        if(await Task.Run(() => Database.onBuyItem(name, count)));
                        {
                            item.setCount(item.getCount() - count);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        catch (Exception ex)
        {
            await Logger.LogAsync("Error on buy item: " + ex.Message, Logger.LogLevel.Error);
            throw;
        }
        finally
        {
            semaphore.Release();
        }
    }
}
