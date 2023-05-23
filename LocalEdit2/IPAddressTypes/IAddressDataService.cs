namespace LocalEdit2.IPAddressTypes
{
    public interface IAddressDataService
    {
        Task<AddressInfo> GetIPAddressInfo();
    }
}
