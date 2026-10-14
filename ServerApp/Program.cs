var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

// Optimization (suggested with Copilot): cache the product list so repeated requests don't hit the data source.
builder.Services.AddOutputCache(options =>
    options.AddPolicy("ProductList", policy => policy.Expire(TimeSpan.FromSeconds(60))));

var app = builder.Build();

// Activity 2 (Copilot-assisted): CORS policy so the Blazor client can call the API.
// AllowAnyOrigin is fine for this course project; restrict origins in production.
app.UseCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());

app.UseOutputCache();

// Activity 3 (Copilot-assisted): typed records give a consistent, nested JSON structure.
app.MapGet("/api/productlist", () => Results.Ok(ProductData.GetAll()))
   .CacheOutput("ProductList");

app.Run();

static class ProductData
{
    public static Product[] GetAll() =>
    [
        new(1, "Laptop", 1200.50m, 25, new Category(101, "Electronics")),
        new(2, "Headphones", 50.00m, 100, new Category(102, "Accessories"))
    ];
}

record Category(int Id, string Name);

record Product(int Id, string Name, decimal Price, int Stock, Category Category);
