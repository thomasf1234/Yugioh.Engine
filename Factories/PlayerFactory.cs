using Microsoft.Extensions.Logging;

using Yugioh.Engine.Models;

namespace Yugioh.Engine.Factories
{
  public class PlayerFactory : IPlayerFactory
  {
    private readonly ILogger<PlayerFactory> _logger;
    public PlayerFactory(ILoggerFactory loggerFactory)
    {
      this._logger = loggerFactory.CreateLogger<PlayerFactory>();
    }
    
    public Player Build(Save save)
    {
      Player player = new Player();

      player.Name = save.PlayerName;
      player.DuelistPoints = save.PlayerDuelistPoints;
      player.Cards = this.

    }
  }
}
