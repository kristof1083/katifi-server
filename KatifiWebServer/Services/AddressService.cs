using KatifiWebServer.Data;
using KatifiWebServer.Data.Base;
using KatifiWebServer.Models.DatabaseModels;

namespace KatifiWebServer.Services
{
    public class AddressService : EntityBaseRepository<Address>, IAddressService
    {
        public AddressService(MicrosoftEFContext context) : base(context)
        {
        }

        public override async Task<bool> MeetsTheConstraints(Address address)
        {
            bool basebool = await base.MeetsTheConstraints(address);
            if (basebool && !string.IsNullOrWhiteSpace(address.CountryCode) && address.CountryCode.Length <= 3 && !string.IsNullOrWhiteSpace(address.County) && !string.IsNullOrWhiteSpace(address.City))
            {
                if(address.PostCode == null || (address.PostCode != null && address.PostCode >= 1000 && address.PostCode < 10000))
                {
                    return await Task.FromResult(true);
                }
            }
            return await Task.FromResult(false);
        }
    }
}
