using Microsoft.EntityFrameworkCore;
//using KatifiWebServer.Models;

namespace KatifiWebServer.Data
{
    public partial class MicrosoftEFContext : DbContext
    {
        public MicrosoftEFContext(DbContextOptions<MicrosoftEFContext> options) : base(options) { }



    }
}
