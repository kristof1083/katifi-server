using KatifiWebServer.Data;
using KatifiWebServer.Models.DatabaseModels;
using KatifiWebServer.Services;
using KatifiWebServer.Services.GoogleServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

#region Add Various Services
// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<MicrosoftEFContext>(opt => opt.UseSqlServer(
    builder.Configuration.GetConnectionString("LocalConnection")));

builder.Services.AddAutoMapper(typeof(MapperConfig));
builder.Services.AddScoped<MicrosoftEFContext>();

builder.Services.AddIdentityCore<AppUser>(opt => { opt.Password.RequiredLength = 8; opt.Password.RequireDigit = false;})
    .AddRoles<AppRole>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<MicrosoftEFContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
 {
     options.SaveToken = true;
     options.RequireHttpsMetadata = false;
     options.TokenValidationParameters = new TokenValidationParameters()
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidAudience = configuration["JWT:ValidAudience"],
         ValidIssuer = configuration["JWT:ValidIssuer"],
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
     };
 });

builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IChurchService, ChurchService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICommunityService, CommunityService>();
builder.Services.AddScoped<IMessService, MessService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IParticipantService, ParticipantService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IImageFileService, ImageFileService>();
builder.Services.AddScoped<IGoogleService, GoogleService>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Title = "API testing",
        Version = "v3.4"
    });
});

builder.Services.Configure<FormOptions>(options =>
{
    // Set the limit to 128 MB =~ 20-25 photo per upload
    options.MultipartBodyLengthLimit = configuration.GetValue<long>("FileUploads:FileCollectionSizeLimit");
});
#endregion

var app = builder.Build();

IWebHostEnvironment env = app.Services.GetRequiredService<IWebHostEnvironment>();
if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API testing");
    });
}

#region Initialize Database content
var initializer = new DBInitializer();
initializer.Seed(app);
initializer = null;
#endregion

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

Console.WriteLine($"Tunnel URL: {Environment.GetEnvironmentVariable("VS_TUNNEL_URL")}");