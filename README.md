# InventoryHub

A small full-stack inventory app: a Blazor WebAssembly front-end (`ClientApp`) that displays products from an ASP.NET Core Minimal API (`ServerApp`).

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

## Run

Start the API, then the client, in two terminals:

```powershell
dotnet run --project ServerApp --launch-profile http
dotnet run --project ClientApp --launch-profile http
```

Open <http://localhost:5073/fetchproducts>.

## API

| Method | Route              | Description                                |
| ------ | ------------------ | ------------------------------------------ |
| GET    | `/api/productlist` | Returns products with a nested `category`. |

Example response:

```json
[
  { "id": 1, "name": "Laptop", "price": 1200.50, "stock": 25,
    "category": { "id": 101, "name": "Electronics" } }
]
```

## Project structure

- `ServerApp` - Minimal API with CORS and output caching (60 s).
- `ClientApp` - Blazor WebAssembly UI; `ProductService` caches the product list.

See [REFLECTION.md](REFLECTION.md) for the development reflection.
