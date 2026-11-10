using Microsoft.AspNetCore.Http.HttpResults;
using MyApi;

// WebApplication.CreateBuilder sets up the "builder", which is where you
// configure the app before it starts: services, logging, settings, etc.
// It also reads configuration (appsettings.json, environment variables)
// and sets up the web server (Kestrel) for you.
var builder = WebApplication.CreateBuilder(args);

// Register services with the dependency injection (DI) container.
// AddHealthChecks() makes the health check feature available.
// We'll expose it as an endpoint below, and later tools (Docker,
// Kubernetes, Azure) can call it to ask "is this app alive?"
builder.Services.AddHealthChecks();

// Build() turns the configured builder into the actual app.
// After this point, you define endpoints rather than register services.
var app = builder.Build();

// A simple in-memory list to act as our "database".
// It lives in RAM, so data is lost whenever the app restarts.
// Fine for learning; you'd swap this for a real database later.
var todos = new List<Todo>();

// MapGet: "when someone sends an HTTP GET request to this URL path,
// run this function and return the result."
// Here, GET / returns a plain text string with a 200 OK status.
// The () => ... syntax is a lambda: a small inline function.
app.MapGet("/", () => "Hello from MyApi! v2 with CI/CD");

// GET /todos returns the whole list. ASP.NET Core automatically
// serializes the List<Todo> to JSON for you.
app.MapGet("/todos", () => todos);

// MapPost: handles HTTP POST requests, typically used to create things.
// The (Todo todo) parameter is the magic part: ASP.NET Core sees that the
// request has a JSON body, deserializes it into a Todo object, and passes
// it in. No manual parsing needed. This is called "model binding".
app.MapPost("/todos", (Todo todo) => 
{
    todos.Add(todo);

    // Results.Created returns HTTP 201 Created, with a Location header
    // pointing to where the new item would live, and the item in the body.
    return Results.Created($"/todos/{todo.Id}", todo);
});

// Exposes the health check at GET /health. It returns "Healthy" with a
// 200 status if the app is running properly. This is the endpoint your
// monitoring and deployment tooling will poll later.
app.MapHealthChecks("/health");

// Starts the web server and blocks here, listening for requests
// until the app is shut down.
app.Run();