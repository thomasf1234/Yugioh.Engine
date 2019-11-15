using System.IO;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using Yugioh.Engine.Entities;
using Yugioh.Engine.Factories;
using Yugioh.Engine.Models;

namespace Yugioh.Engine.Services
{
  public class SaveService : ISaveService
  {
    private readonly ILogger<SaveService> _logger;
    private readonly ISaveFactory _saveFactory;
    public SaveService(ILoggerFactory loggerFactory, ISaveFactory saveFactory)
    {
      this._logger = loggerFactory.CreateLogger<SaveService>();
      this._saveFactory = saveFactory;
    }

    public Player Load(string playerName)
    {
      Player player = null;
      string loadPath = Path.Combine(Directory.GetCurrentDirectory(), "Out", $"{playerName}.json");

      using (StreamReader streamReader = File.OpenText(loadPath))
      {
        JsonSerializer serializer = new JsonSerializer();
        Save save = (Save) serializer.Deserialize(streamReader, typeof(Save));

        
        this._logger.LogInformation($"Loaded Player '{playerName}' from {loadPath}");
      }

      return player;
    }

    public void Save(Player player)
    {
      Save save = this._saveFactory.Build(player);
      string saveJson = JsonConvert.SerializeObject(save);

      string savePath = Path.Combine(Directory.GetCurrentDirectory(), "Out", $"{save.PlayerName}.json");

      using (StreamWriter streamWriter = new StreamWriter(savePath))
      {
        foreach (char c in saveJson)
        {
          streamWriter.Write(c);
        }
      }

      this._logger.LogInformation($"Saved Player '{player.Name}' to {savePath}");
    }
  }
}
