using KatifiWebServer.Data;
using KatifiWebServer.Data.Base;
using KatifiWebServer.Models.DatabaseModels;

namespace KatifiWebServer.Services
{
    public class MessService : EntityBaseRepository<Mess>, IMessService
    {
        public MessService(MicrosoftEFContext context) : base(context)
        {
        }

        public override async Task<bool> MeetsTheConstraints(Mess mess)
        {
            bool basebool = await base.MeetsTheConstraints(mess);
            if (basebool && !string.IsNullOrWhiteSpace(mess.Day))
            {
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}
