using GameStore.Server.Models;

List<Game> games =
    [
      new Game() 
      {
        Id = 1,
        Name = "The Legend of Zelda: Breath of the Wild",
        Genre = "Adventure",
        Price = 59.99m,
        ReleaseDate = new DateTime(2017, 3, 3)
      },
      new Game() 
      {
        Id = 2,
        Name = "Super Mario Odyssey",
        Genre = "Platformer",
        Price = 59.99m,
        ReleaseDate = new DateTime(2017, 10, 27)
      },
      new Game() 
      {
        Id = 3,
        Name = "Grand Theft Auto V",
        Genre = "Action",
        Price = 29.99m,
        ReleaseDate = new DateTime(2013, 9, 17)
      }
    ];

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => options.AddDefaultPolicy(policy => 
{
  policy.WithOrigins("http://localhost:5033")
    .AllowAnyHeader()
    .AllowAnyMethod();
}));

var app = builder.Build();

app.UseCors();

var group = app.MapGroup("/games").WithParameterValidation();

// GET /games
group.MapGet("/", () => Results.Ok(games));

// GET /games/{id}
group.MapGet("/{id:int}", (int id) => 
{
  var game = games.Find(game => game.Id == id);

  if (game is null) return Results.NotFound();

  return Results.Ok(game);
}).WithName("GetGameById");

// POST /games
group.MapPost("/", (Game game) => 
{
  game.Id = games.Max(game => game.Id) + 1;
  games.Add(game);

  return Results.CreatedAtRoute("GetGameById", new { id = game.Id }, game);
});

// PUT /games/{id}
group.MapPut("/{id:int}", (int id, Game game) => 
{
  var existingGame = games.Find(game => game.Id == id);

  if (existingGame is null) return Results.NotFound();

  existingGame.Name = game.Name;
  existingGame.Genre = game.Genre;
  existingGame.Price = game.Price;
  existingGame.ReleaseDate = game.ReleaseDate;

  return Results.NoContent();
});

// DELETE /games/{id}
group.MapDelete("/{id:int}", (int id) => 
{
  var game = games.Find(game => game.Id == id);

  if (game is null) return Results.NotFound();

  games.Remove(game);

  return Results.NoContent();
});

app.Run();
