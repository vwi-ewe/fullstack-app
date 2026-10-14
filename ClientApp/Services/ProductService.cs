using System.Net.Http.Json;
using System.Text.Json;

namespace ClientApp.Services;

public record Category(int Id, string Name);

public record Product(int Id, string Name, decimal Price, int Stock, Category? Category);

/// <summary>
/// Loads products from the back-end API. The result is cached for the lifetime of the app
/// so navigating between pages does not trigger redundant API calls (Copilot-suggested optimization).
/// </summary>
public class ProductService(HttpClient http)
{
    private const string Endpoint = "/api/productlist";
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(10);

    private Product[]? _cache;
    private Task<Product[]>? _inFlight;

    public Task<Product[]> GetProductsAsync(bool forceRefresh = false)
    {
        if (forceRefresh)
        {
            _cache = null;
        }

        if (_cache is not null)
        {
            return Task.FromResult(_cache);
        }

        // Share one in-flight request between concurrent callers.
        return _inFlight ??= LoadAsync();
    }

    private async Task<Product[]> LoadAsync()
    {
        try
        {
            using var cts = new CancellationTokenSource(Timeout);
            var products = await http.GetFromJsonAsync<Product[]>(Endpoint, cts.Token) ?? [];
            _cache = products;
            return products;
        }
        finally
        {
            _inFlight = null;
        }
    }
}
