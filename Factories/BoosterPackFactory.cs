using System.Linq;
using System.Collections.Generic;
using System;

using Yugioh.Engine.Entities;
using Yugioh.Engine.Models;
using Yugioh.Engine.Constants;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Yugioh.Engine.Factories
{
  public class BoosterPackFactory : IBoosterPackFactory
  {
    private readonly ILogger<BoosterPackFactory> _logger;
    public BoosterPackFactory(ILoggerFactory loggerFactory)
    {
      this._logger = loggerFactory.CreateLogger<BoosterPackFactory>();
    }

    public BoosterPack Build(BaseBoosterPack baseBoosterPack)
    {      
      IList<UserCard> boosterPackUserCards = new List<UserCard>();
      
      if (baseBoosterPack.IsStarterDeck())
      {
        foreach (BaseBoosterPackCard baseBoosterPackCard in baseBoosterPack.BaseBoosterPackCards)
        {
          UserCard userCard = new UserCard();
          userCard.BaseCard = baseBoosterPackCard.BaseCard;
          userCard.Rarity = baseBoosterPackCard.Rarity;
          userCard.BaseCardId = baseBoosterPackCard.BaseCardId;
          userCard.RarityId = baseBoosterPackCard.RarityId;
          boosterPackUserCards.Add(userCard);

          this._logger.LogDebug($"Adding {baseBoosterPackCard.BaseCard.Name} ({baseBoosterPackCard.Rarity.Name}) to starter deck {baseBoosterPack.Name}");
        }
      }
      else
      {
        var baseBoosterPackCardsGroupedByRarity = baseBoosterPack.BaseBoosterPackCards.GroupBy(basePackCard => basePackCard.Rarity);

        var specialGroupings = baseBoosterPackCardsGroupedByRarity.Where(g => g.Key.IsSpecial()).OrderBy(g => g.Key.Ratio);
        var nonSpecialGroupings = baseBoosterPackCardsGroupedByRarity.Where(g => !g.Key.IsSpecial()).OrderBy(g => g.Key.Ratio);

        IList<BaseBoosterPackCard> chosenNonSpecialBaseBoosterPackCards = new List<BaseBoosterPackCard>();

        Random random = new Random();

        for (int i = 0; i < 4; ++i)
        {
          double nonSpecialSelection = random.NextDouble();
          this._logger.LogDebug($"nonSpecialSelection: {nonSpecialSelection}");

          foreach (IGrouping<Rarity, BaseBoosterPackCard> nonSpecialGroupOrderedByRatio in nonSpecialGroupings)
          {
            Rarity rarity = nonSpecialGroupOrderedByRatio.Key;

            if (rarity.Ratio >= nonSpecialSelection)
            {
              this._logger.LogDebug($"Name: {rarity.Name}, Ratio {rarity.Ratio}, Special {rarity.Special}");
              int index = random.Next(nonSpecialGroupOrderedByRatio.Count());
              BaseBoosterPackCard chosenNonSpecialBaseBoosterPackCard = nonSpecialGroupOrderedByRatio.ToList()[index];
              UserCard chosenNonSpecialUserCardCard = new UserCard();
              chosenNonSpecialUserCardCard.BaseCard = chosenNonSpecialBaseBoosterPackCard.BaseCard;
              chosenNonSpecialUserCardCard.BaseCardId = chosenNonSpecialBaseBoosterPackCard.BaseCardId;
              chosenNonSpecialUserCardCard.Rarity = rarity;
              chosenNonSpecialUserCardCard.RarityId = rarity.RarityId;
              boosterPackUserCards.Add(chosenNonSpecialUserCardCard);

              this._logger.LogDebug($"Chose: {chosenNonSpecialBaseBoosterPackCard.BaseCard.Name}");
              break;
            }
          }
        }

        double specialSelection = random.NextDouble();
        this._logger.LogDebug($"specialSelection: {specialSelection}");

        foreach (IGrouping<Rarity, BaseBoosterPackCard> specialGroupOrderedByRatio in specialGroupings)
        {
          Rarity rarity = specialGroupOrderedByRatio.Key;

          if (rarity.Ratio >= specialSelection)
          {
            this._logger.LogDebug($"Name: {rarity.Name}, Ratio {rarity.Ratio}, Special {rarity.Special}");
            int index = random.Next(specialGroupOrderedByRatio.Count());
            BaseBoosterPackCard chosenSpecialBaseBoosterPackCard = specialGroupOrderedByRatio.ToList()[index];
            UserCard chosenSpecialUserCardCard = new UserCard();
            chosenSpecialUserCardCard.BaseCardId = chosenSpecialBaseBoosterPackCard.BaseCardId;
            chosenSpecialUserCardCard.BaseCard = chosenSpecialBaseBoosterPackCard.BaseCard;
            chosenSpecialUserCardCard.RarityId = rarity.RarityId;
            chosenSpecialUserCardCard.Rarity = rarity;
            boosterPackUserCards.Add(chosenSpecialUserCardCard);

            this._logger.LogDebug($"Chose: *{chosenSpecialBaseBoosterPackCard.BaseCard.Name}*");
            break;
          }
        }
      }

      BoosterPack boosterPack = new BoosterPack(baseBoosterPack, boosterPackUserCards);

      return boosterPack;
    }
  }
}
