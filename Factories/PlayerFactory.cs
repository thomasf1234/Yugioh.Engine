using Microsoft.Extensions.Logging;

using Yugioh.Engine.Models;
using Yugioh.Engine.Services;

namespace Yugioh.Engine.Factories
{
  public class PlayerFactory : IPlayerFactory
  {
    private readonly ILogger<PlayerFactory> _logger;
    private readonly ICardService _cardService;
    public PlayerFactory(ILoggerFactory loggerFactory, ICardService cardService)
    {
      this._logger = loggerFactory.CreateLogger<PlayerFactory>();
      this._cardService = cardService;
    }
    
    public Player Build(Save save)
    {
      Player player = new Player();

      player.Name = save.PlayerName;
      player.DuelistPoints = save.PlayerDuelistPoints;
      player.Cards = this._cardService.Get(save.PlayerCardNumbers);
      player.ActiveDeckName = save.PlayerActiveDeckName;

      return player;
    }
  }
}
