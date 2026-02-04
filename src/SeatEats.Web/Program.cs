using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using SeatEats.Application.Interfaces;
using SeatEats.Application.Services;
using SeatEats.Web.Components;
using SeatEats.Web.Data;
using SeatEats.Web.Hubs;
using SeatEats.Web.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Configure forwarded headers for Railway proxy
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

// Add PostgreSQL
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
    ?? "Host=maglev.proxy.rlwy.net;Port=54380;Database=railway;Username=postgres;Password=EuScwOWSozZuFhOvKXRDfKJFbYmGNert";
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add SignalR
builder.Services.AddSignalR();

// Register application services
builder.Services.AddScoped<IMenuRepository, EfMenuRepository>();
builder.Services.AddScoped<IOrderRepository, EfOrderRepository>();
builder.Services.AddScoped<IOrderNotificationService, SignalROrderNotificationService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<MenuService>();

var app = builder.Build();

// Auto-migrate and seed database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.MenuItems.Any())
    {
        db.MenuItems.AddRange(
            SeatEats.Domain.Entities.MenuItem.Create("Big Mac", "Two all-beef patties, special sauce, lettuce, cheese, pickles, onions on a sesame seed bun", 5.99m, "Burgers"),
            SeatEats.Domain.Entities.MenuItem.Create("Quarter Pounder with Cheese", "Fresh beef patty with cheese, onions, pickles, ketchup, and mustard", 6.49m, "Burgers"),
            SeatEats.Domain.Entities.MenuItem.Create("McChicken", "Crispy chicken patty with mayo and shredded lettuce", 4.49m, "Chicken"),
            SeatEats.Domain.Entities.MenuItem.Create("10 Piece McNuggets", "Tender chicken McNuggets with your choice of dipping sauce", 6.99m, "Chicken"),
            SeatEats.Domain.Entities.MenuItem.Create("Large Fries", "Golden, crispy world-famous fries", 3.99m, "Sides"),
            SeatEats.Domain.Entities.MenuItem.Create("Medium Fries", "Golden, crispy world-famous fries", 2.99m, "Sides"),
            SeatEats.Domain.Entities.MenuItem.Create("Coca-Cola Large", "Ice-cold Coca-Cola", 2.99m, "Drinks"),
            SeatEats.Domain.Entities.MenuItem.Create("Sprite Large", "Ice-cold Sprite", 2.99m, "Drinks"),
            SeatEats.Domain.Entities.MenuItem.Create("McFlurry Oreo", "Creamy vanilla soft serve with Oreo cookie pieces", 4.49m, "Desserts"),
            SeatEats.Domain.Entities.MenuItem.Create("Apple Pie", "Warm, crispy pie with sweet apple filling", 2.49m, "Desserts")
        );
        db.SaveChanges();
    }
}

// Use forwarded headers (must be first)
app.UseForwardedHeaders();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseAntiforgery();

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapStaticAssets();
app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Map SignalR hub
app.MapHub<OrderHub>("/orderhub");

app.Run();
