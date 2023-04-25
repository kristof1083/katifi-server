using KatifiWebServer.Data;
using KatifiWebServer.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<MicrosoftEFContext>(opt => opt.UseSqlServer(
    builder.Configuration.GetConnectionString("LocalConnection")));

builder.Services.AddAutoMapper(typeof(MapperConfig));
builder.Services.AddScoped<MicrosoftEFContext>();

builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IChurchService, ChurchService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICommunityService, CommunityService>();
builder.Services.AddScoped<IMessService, MessService>();
builder.Services.AddScoped<IEventService, EventService>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Title = "API testing",
        Version = "v1"
    });
});

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

DBInitializer.Seed(app);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

Console.WriteLine($"Tunnel URL: {Environment.GetEnvironmentVariable("VS_TUNNEL_URL")}");