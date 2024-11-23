using ILIS.Football.Assignment.BusinessLogic;
using ILIS.Football.Assignment.Infrastructure;
using ILIS.Football.Assignment.Infrastructure.Models;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();

//Options
builder.Services.Configure<FootballApiClientOptions>(
    builder.Configuration.GetSection("ApiClients:FootballApiClient")
);

builder.Services.Configure<CompetitionsConfig>(builder.Configuration.GetSection("Competitions"));

//Infrastructure
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IMemoryCacheManager, MemoryCacheManager>();

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<IFootballGamesApiClient, FootballGamesApiClient>((sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<FootballApiClientOptions>>().Value;
    client.BaseAddress = new Uri(options.Url);
    client.DefaultRequestHeaders.Add("X-Auth-Token", options.Token);
});

//BL
builder.Services.AddScoped<IFootballGamesService, FootballGamesService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Assignment API v1");
    c.DocumentTitle = "Football API Documentation";
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
