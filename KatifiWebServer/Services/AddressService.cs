using KatifiWebServer.Data;
using KatifiWebServer.Data.Base;
using KatifiWebServer.Models.DatabaseModels;
using Microsoft.EntityFrameworkCore;

namespace KatifiWebServer.Services
{
    public class AddressService : EntityBaseRepository<Address>, IAddressService
    {
        private readonly MicrosoftEFContext _context;

        public AddressService(MicrosoftEFContext context) : base(context)
        {
            _context = context;
        }

        public override bool MeetsTheConstraints(Address address)
        {
            bool basebool = base.MeetsTheConstraints(address);
            if (basebool && !string.IsNullOrWhiteSpace(address.CountryCode) && address.CountryCode.Length <= 3 && !string.IsNullOrWhiteSpace(address.County) && !string.IsNullOrWhiteSpace(address.City))
            {
                if (address.PostCode == null || (address.PostCode != null && address.PostCode >= 1000 && address.PostCode < 10000))
                {
                    return true;
                }
            }
            return false;
        }


        public async Task<int> EntityExists(Address address)
        {
            var existingaddress = await _context.Set<Address>()
                .AsNoTracking()
                .Where(a => a.CountryCode == address.CountryCode && a.County == address.County && a.City == address.City &&
                    a.Street == address.Street && a.PostCode == address.PostCode && a.HouseNumber == address.HouseNumber)
                .FirstOrDefaultAsync();

            if (existingaddress == null)
                return -1;

            return existingaddress.Id;
        }

        public async Task<int> ConnectedEntitysNumber(int id)
        {
            try
            {
                if (id == 0)
                    return 0;

                var address = await _context.Addresses
                .AsNoTracking()
                .Include(a => a.Users).Include(a => a.Events).Include(a => a.Communities).Include(a => a.Church)
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();

                if (address == null)
                    return 0;

                int count = address.Users.Count + address.Events.Count + address.Communities.Count;
                if (address.Church != null)
                    count++;

                return count;
            }
            catch (Exception ex)
            {
                var m = ex.Message;
                return 0;
            }
        }

        public async Task<bool> HasChurchInLocation(int addressid)
        {
            var address = await _context.Addresses.AsNoTracking().Include(a => a.Church).Where(a=>a.Id == addressid).FirstOrDefaultAsync();
            
            if(address == null || address.Church == null)
                return false;

            return true;
        }

        public async Task<int> HandleInternalAddressUpdate(Address address)
        {
            int pe_addressid = await this.EntityExists(address);
            int cetoold = await this.ConnectedEntitysNumber(address.Id);
            
            if(address.Id != pe_addressid)
            {
                if (pe_addressid > 0)
                {
                    return pe_addressid;
                }
                else
                {
                    if(cetoold == 1)
                    {
                        await base.UpdateAsync(address);
                        return address.Id;
                    }
                    else
                    {
                        address.Id = 0;
                        await this.AddAsync(address);
                        return address.Id;
                    }
                }
            }

            return address.Id;
        }

        public async Task DeleteUnusedAddresses()
        {
            foreach(var address in await _context.Addresses.ToListAsync())
            {
                if(await ConnectedEntitysNumber(address.Id) == 0)
                {
                    await base.DeleteAsync(address.Id);
                }
            }
        }
    }
}
