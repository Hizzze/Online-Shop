namespace WebApplication2;

public class MainControllerItems
{
    public List<Item> items = new List<Item>();
    private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
    public MainControllerItems()
    {
        items = Database.getItemsList();
    }
    public void onReload()
    {
        items = Database.getItemsList();
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
