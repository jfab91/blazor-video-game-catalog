using GameStore.Client.Models;

namespace GameStore.Client;

public static class GameClient
{
    private static readonly List<Game> games =
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

    public static Game[] GetGames() => [.. games];

    public static void AddGame(Game game) 
    {
      game.Id = games.Max(g => g.Id) + 1;
      games.Add(game);
    }

    public static Game GetGame(int id) 
    {
      return games.Find(game => game.Id == id) ?? throw new Exception("Could not find game");
    }

    public static void UpdateGame(Game updatedGame) 
    {
      var existingGame = GetGame(updatedGame.Id);

      existingGame.Name = updatedGame.Name;
      existingGame.Genre = updatedGame.Genre;
      existingGame.Price = updatedGame.Price;
      existingGame.ReleaseDate = updatedGame.ReleaseDate;
    }

    public static void DeleteGame(int id) 
    {
      var existingGame = GetGame(id);
      games.Remove(existingGame);
    }
}