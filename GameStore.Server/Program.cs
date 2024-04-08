using GameStore.Server.Data;
using GameStore.Server.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => options.AddDefaultPolicy(policy => 
{
  policy.WithOrigins("http://localhost:5033")
    .AllowAnyHeader()
    .AllowAnyMethod();
}));

var dbConnection = builder.Configuration.GetConnectionString("GameStoreContext");

builder.Services.AddSqlServer<GameStoreContext>(dbConnection);

var app = builder.Build();

app.UseCors();

var group = app.MapGroup("/games").WithParameterValidation();

// GET /games
group.MapGet("/", async (GameStoreContext context) => 
  await context.Games.AsNoTracking().ToListAsync());

// GET /games/{id}
group.MapGet("/{id:int}", async (int id, GameStoreContext context) => 
{
  var game = await context.Games.FindAsync(id);

  if (game is null) return Results.NotFound();

  return Results.Ok(game);
}).WithName("GetGameById");

// POST /games
group.MapPost("/", async (Game game, GameStoreContext context) => 
{
  context.Games.Add(game);

  await context.SaveChangesAsync();

  return Results.CreatedAtRoute("GetGameById", new { id = game.Id }, game);
});

// PUT /games/{id}
group.MapPut("/{id:int}", async (int id, Game updated, GameStoreContext context) => 
{
  var rowsAffected = await context.Games.Where(game => game.Id == id)
    .ExecuteUpdateAsync(updates => 
      updates.SetProperty(game => game.Name, updated.Name)
        .SetProperty(game => game.Genre, updated.Genre)
        .SetProperty(game => game.Price, updated.Price)
        .SetProperty(game => game.ReleaseDate, updated.ReleaseDate));

  return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
});

// DELETE /games/{id}
group.MapDelete("/{id:int}", async (int id, GameStoreContext context) => 
{
  var rowsAffected = await context.Games.Where(game => game.Id == id)
    .ExecuteDeleteAsync();

  return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
});

app.Run();
