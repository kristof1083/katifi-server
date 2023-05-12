using KatifiWebServer.Data.Base;
using KatifiWebServer.Models.DatabaseModels;

namespace KatifiWebServer.Services
{
    public interface IAddressService : IEntityBaseRepository<Address>
    {
        public Task<int> EntityExists(Address address);

        public Task<int> ConnectedEntitysNumber(int id);

        public Task<int> HandleInternalAddressUpdate(Address possNewAddress);

        public Task<bool> HasChurchInLocation(int addressid);

        public Task DeleteUnusedAddresses();
    }
        
}
