# InventoryHub – Reflection

## Overview
InventoryHub is a Blazor WebAssembly front-end (`ClientApp`) backed by a Minimal API (`ServerApp`).
The product list page (`/fetchproducts`) loads products from `GET /api/productlist`.

## How Copilot helped
- **Integration code:** Generated the `HttpClient` call in `OnInitializedAsync`, JSON deserialization into `Product`, and timeout/error handling. It also pointed out that the client's `HttpClient` base address must target the API's port, not the client's own address.
- **Debugging:** Updated the route to `/api/productlist`, added the CORS policy, and added `try/catch` handling for malformed JSON. It also showed that `ERR_CONNECTION_REFUSED` means the API was not running, not a CORS or JSON problem.
- **JSON structure:** Replaced anonymous objects with typed records (`Product`, `Category`) so the response has a consistent, nested shape (`id`, `name`, `price`, `stock`, `category`).
- **Performance:** Suggested output caching on the API (60 seconds) and a client-side `ProductService` that caches results and shares one in-flight request, removing redundant API calls.

## Challenges
- Cross-origin calls between two projects on different ports (CORS and base address).
- A file-locking build error because the API was still running; stopping the process fixed it.
- Some automated edits overwrote whole files, so I had to verify the results after each change.

## Performance testing
Open browser DevTools → Network tab on `/fetchproducts`:
1. First load: one request to `/api/productlist`.
2. Navigate to another page and back: no new request (client cache).
3. Click **Refresh**: a new request is sent; the API serves it from output cache for up to 60 seconds (compare response times).

## Lessons learned
- Copilot works best with specific prompts and the relevant file context.
- Always review and build/test generated code; it can be subtly wrong or overwrite more than intended.
- Typed models and small services make code easier for both humans and Copilot to maintain.
- Error messages (such as network errors in the console) are useful context to give Copilot when debugging.
