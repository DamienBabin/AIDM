// DnDAdventure.API/Program.cs
using DnDAdventure.AI;
using DnDAdventure.Core.Models;
using DnDAdventure.Core.Repositories;
using DnDAdventure.Core.Services;
using DnDAdventure.Infrastructure.Repositories;
using DnDAdventure.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure repositories
builder.Services.AddSingleton<IRepository<Character>>(provider =>
    new InMemoryRepository<Character>(c => c.Id));
builder.Services.AddSingleton<IRepository<GameState>>(provider =>
    new InMemoryRepository<GameState>(gs => gs.Id));

// Configure HTTP client for AI service
builder.Services.AddHttpClient();

// Configure AI service - use a dummy endpoint for now
string aiEndpoint = builder.Configuration["AI:Endpoint"] ?? "http://localhost:5001/generate";
builder.Services.AddSingleton<AdventureGenerator>(provider =>
{
    var httpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient();
    return new AdventureGenerator(httpClient, aiEndpoint);
});

// Configure game service
builder.Services.AddScoped<IGameService, GameService>();

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();