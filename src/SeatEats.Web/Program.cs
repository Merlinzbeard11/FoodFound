using Microsoft.AspNetCore.HttpOverrides;
using SeatEats.Application.Interfaces;
using SeatEats.Application.Services;
using SeatEats.Web.Components;
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

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add SignalR
builder.Services.AddSignalR();

// Register application services
builder.Services.AddSingleton<IMenuRepository, InMemoryMenuRepository>();
builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
builder.Services.AddScoped<IOrderNotificationService, SignalROrderNotificationService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<MenuService>();

var app = builder.Build();

// Use forwarded headers (must be first)
app.UseForwardedHeaders();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Map SignalR hub
app.MapHub<OrderHub>("/orderhub");

app.Run();
