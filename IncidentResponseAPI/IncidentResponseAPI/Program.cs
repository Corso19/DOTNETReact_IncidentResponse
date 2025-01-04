using DotNetEnv;
using IncidentResponseAPI.Models;
using IncidentResponseAPI.Repositories.Interfaces;
using IncidentResponseAPI.Repositories.Implementations;
using IncidentResponseAPI.Scheduling;
using IncidentResponseAPI.Services;
using IncidentResponseAPI.Services.Implementations;
using IncidentResponseAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Quartz;

using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables from .env file
Env.Load();

// Debug logging to verify environment variable loading
var connectionString = Environment.GetEnvironmentVariable("DefaultConnection")
    ?? throw new InvalidOperationException("The ConnectionString property has not been initialized.");

// Add Quartz services
builder.Services.AddQuartz(q =>
{
    // Register the EventsProcessingJob
    var jobKey = new JobKey("EventsProcessingJob");
    q.AddJob<EventsProcessingJob>(opts => opts.WithIdentity(jobKey));
    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("EventsProcessingTrigger")
        .StartNow()
        .WithCronSchedule("0 0/5 * * * ?")); // Schedule to run every 5 minutes
});

// Add services to the container.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddControllers();
builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
builder.Services.AddScoped<IEventsRepository, EventsRepository>();
builder.Services.AddScoped<IEventsService, EventsService>();
builder.Services.AddScoped<IAttachmentRepository, AttachmentRepository>();
builder.Services.AddScoped<ISensorsRepository, SensorsRepository>();
builder.Services.AddScoped<ISensorsService, SensorsService>();
builder.Services.AddScoped<IRecommendationsRepository, RecommendationsRepository>();
builder.Services.AddScoped<IRecommendationsService, RecommendationsService>();
builder.Services.AddScoped<IIncidentsRepository, IncidentsRepository>();
builder.Services.AddScoped<IIncidentsService, IncidentsService>();
builder.Services.AddScoped<IIncidentEventRepository, IncidentEventRepository>();
builder.Services.AddScoped<IIncidentEventService, IncidentEventService>();
builder.Services.AddScoped<IEventsProcessingService, EventsProcessingService>();
builder.Services.AddScoped<IConfigurationValidator, ConfigurationValidator>();
builder.Services.AddSingleton<GraphAuthProvider>();
builder.Services.AddScoped<IGraphAuthService, GraphAuthService>();
builder.Services.AddScoped<IIncidentDetectionService, IncidentDetectionService>();
//Adding database context
builder.Services.AddDbContext<IncidentResponseContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
    }));

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policyBuilder => policyBuilder.WithOrigins("http://localhost:3000")
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


