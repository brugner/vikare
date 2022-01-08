using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Mvc;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IAirportService, AirportService>();
builder.Services.AddSingleton<IAirportRepository, AirportRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptions();
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

builder.Logging.ClearProviders();
builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "corsPolicy", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().WithMethods("GET");
    });
});

// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseIpRateLimiting();
app.UseSerilogRequestLogging();
app.UseCors("corsPolicy");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/api/error");
}

/// <summary>
/// Get all the airports.
/// </summary>
app.MapGet("/api/airports", ([FromServices] IAirportService service, int? page, int? pageSize, string? search, bool? excludeMetadata) =>
{
    var listParams = new AirportListParams(page, pageSize, search, excludeMetadata);

    return service.GetAll(listParams);
});

/// <summary>
/// Get an airport by Id.
/// </summary>
app.MapGet("/api/airports/{id}", ([FromServices] IAirportService service, [FromRoute] int id) =>
{
    return service.GetById(id);
});

/// <summary>
/// Get a list of the closest airports relative to a set of coordinates.
/// </summary>
app.MapGet("/api/airports/closest", ([FromServices] IAirportService service, double lat, double lng, int? count, char? unit) =>
{
    var airportsByDistanceParams = new AirportsByDistanceParams(Direction.Closest, lat, lng, count, unit);

    return service.GetAirportsByDistance(airportsByDistanceParams);
});

/// <summary>
/// Get a list of the farthest airports relative to a set of coordinates.
/// </summary>
app.MapGet("/api/airports/farthest", ([FromServices] IAirportService service, double lat, double lng, int? count, char? unit) =>
{
    var airportsByDistanceParams = new AirportsByDistanceParams(Direction.Farthest, lat, lng, count, unit);

    return service.GetAirportsByDistance(airportsByDistanceParams);
});

/// <summary>
/// Returns error data.
/// </summary>
app.Map("/api/error", () =>
{
    return Results.Problem();
});

try
{
    Log.Information("Starting web host");
    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
