using DotNetEnv;
using IncidentResponseAPI.Models;
using IncidentResponseAPI.Repositories;
using IncidentResponseAPI.Repositories.Implementations;
using IncidentResponseAPI.Services;
using IncidentResponseAPI.Services.Implementations;
using IncidentResponseAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Load environment variables from .env file
Env.Load();



// Debug logging to verify environment variable loading
// Console.WriteLine("Loading environment variables...");
var connectionString = Environment.GetEnvironmentVariable("DefaultConnection")
    ?? throw new InvalidOperationException("The ConnectionString property has not been initialized.");
var applicationId = Environment.GetEnvironmentVariable("APPLICATION_ID")
    ?? throw new InvalidOperationException("The ApplicationId property has not been initialized.");
var tenantId = Environment.GetEnvironmentVariable("TENANT_ID")
    ?? throw new InvalidOperationException("The TenantId property has not been initialized.");
var clientSecret = Environment.GetEnvironmentVariable("CLIENT_SECRET")
    ?? throw new InvalidOperationException("The ClientSecret property has not been initialized.");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IEventsRepository, EventsRepository>();
builder.Services.AddScoped<IEventsService, EventsService>();
builder.Services.AddScoped<ISensorsRepository, SensorsRepository>();
builder.Services.AddScoped<ISensorsService, SensorsService>();
builder.Services.AddScoped<IRecommendationsRepository, RecommendationsRepository>();
builder.Services.AddScoped<IRecommendationsService, RecommendationsService>();
builder.Services.AddScoped<IIncidentsRepository, IncidentsRepository>();
builder.Services.AddScoped<IIncidentsService, IncidentsService>();
builder.Services.AddScoped<IIncidentEventRepository, IncidentEventRepository>();
builder.Services.AddScoped<IIncidentEventService, IncidentEventService>();
//Adding database context
builder.Services.AddDbContext<IncidentResponseContext>(options =>
    options.UseSqlServer(connectionString));


Console.WriteLine("Starting GraphTest...");

var graphTest = new GraphTest();

Console.WriteLine("Testing Fetch Users...");
await graphTest.TestFetchUsers();

Console.WriteLine("\nTesting Fetch Emails...");
await graphTest.TestFetchEmails();

Console.WriteLine("\nTesting Fetch Message Content...");
await graphTest.TestFetchMessageContent();

Console.WriteLine("\nTesting Fetch Attachments...");
await graphTest.TestFetchAttachments();

Console.WriteLine("GraphTest completed.");

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "IncidentResponseAPI", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    // Enable middleware to serve generated Swagger as a JSON endpoint.
    app.UseSwagger();
    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
    // specifying the Swagger JSON endpoint.
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IncidentResponseAPI v1"));
}

// Use CORS policy
app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();


