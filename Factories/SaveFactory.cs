
using System;

using Microsoft.Extensions.Logging;

using Yugioh.Engine.Entities;
using Yugioh.Engine.Models;
using Yugioh.Engine.Models.Cards;

namespace Yugioh.Engine.Factories
{
  public class SaveFactory : ISaveFactory
  {
    private readonly ILogger<SaveFactory> _logger;
    public SaveFactory(ILoggerFactory loggerFactory)
    {
      this._logger = loggerFactory.CreateLogger<SaveFactory>();
    }

    public Save Build(Player player)
    {
      Save save = new Save();

      save.PlayerName = player.Name;
      save.PlayerDuelistPoints = player.DuelistPoints;
      
      foreach (Models.Cards.Card card in player.Cards)
      {
        save.PlayerCardNumbers.Add(card.Number);
      }

      return save;
    }
  }
}