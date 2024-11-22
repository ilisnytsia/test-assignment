using ILIS.Football.Assignment.BusinessLogic;
using ILIS.Football.Assignment.Infrastructure;
using ILIS.Football.Assignment.Infrastructure.Models;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();

builder.Services.AddControllersWithViews();

builder.Services.Configure<FootballApiClientOptions>(
    builder.Configuration.GetSection("ApiClients:FootballApiClient")
);

builder.Services.AddHttpClient<IFootballGamesApiClient, FootballGamesApiClient>((sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<FootballApiClientOptions>>().Value;
    client.BaseAddress = new Uri(options.Url);
    client.DefaultRequestHeaders.Add("X-Auth-Token", options.Token);
});

builder.Services.AddScoped<IFootballGamesService, FootballGamesService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
