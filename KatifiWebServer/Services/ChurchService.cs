using KatifiWebServer.Data;
using KatifiWebServer.Data.Base;
using KatifiWebServer.Models.DatabaseModels;

namespace KatifiWebServer.Services
{
    public class ChurchService : EntityBaseRepository<Church>, IChurchService
    {
        public ChurchService(MicrosoftEFContext context) : base(context)
        {
        }

        public override async Task<bool> MeetsTheConstraints(Church church)
        {
            bool basebool = await base.MeetsTheConstraints(church);
            if (basebool && !string.IsNullOrWhiteSpace(church.Name) && church.AdressId >= 338) {
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}
