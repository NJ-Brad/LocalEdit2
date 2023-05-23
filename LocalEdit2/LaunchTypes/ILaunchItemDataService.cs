namespace LocalEdit2.LaunchTypes
{
    public interface ILaunchItemDataService
    {
        Task<IEnumerable<LaunchItem>> GetAllItems();
        Task<LaunchItem> GetItemDetails(int id);
    }
}

