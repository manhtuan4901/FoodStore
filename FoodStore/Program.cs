using FoodStore.Helpers;
using FoodStore.Models;
using FoodStore.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure MongoDB settings
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDB"));

// Register services
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<ProductService>();
builder.Services.AddSingleton<CategoryService>();
builder.Services.AddSingleton<CartService>();
builder.Services.AddSingleton<OrderService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Add session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Ensure admin user is created
await EnsureAdminUserCreated(app.Services);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "admin",
        pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();

async Task EnsureAdminUserCreated(IServiceProvider services)
{
    using (var scope = services.CreateScope())
    {
        var userService = scope.ServiceProvider.GetRequiredService<UserService>();
        var mongoDbSettings = scope.ServiceProvider.GetRequiredService<IOptions<MongoDBSettings>>();
        var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
        var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
        var users = database.GetCollection<User>("Users");

        var adminExists = users.Find(user => user.Username == "admin").Any();
        if (!adminExists)
        {
            var adminUser = new User
            {
                Username = "admin",
                Email = "admin@example.com",
                Fullname = "Administrator",
                Roles = new List<string> { "Admin" }
            };

            // Use HashingHelper to hash the password
            adminUser.Password = HashingHelper.HashPassword("admin");

            await users.InsertOneAsync(adminUser);
        }
    }
}
