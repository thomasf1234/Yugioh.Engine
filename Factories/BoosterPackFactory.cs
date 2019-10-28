// TODO : Flesh out

using Yugioh.Engine.Entities;
using Yugioh.Engine.Models;

using System.Linq;
using System.Collections.Generic;
using System;
using Yugioh.Engine.Constants;

namespace Yugioh.Engine.Factories
{
  public static class BoosterPackFactory
  {
    public static BoosterPack Build(BaseBoosterPack baseBoosterPack)
    {
      var baseBoosterPackCardsGroupedByRarity = baseBoosterPack.BaseBoosterPackCards.GroupBy(basePackCard => basePackCard.Rarity);

      var specialGroupings = baseBoosterPackCardsGroupedByRarity.Where(g => g.Key.IsSpecial()).OrderBy(g => g.Key.Ratio);
      var nonSpecialGroupings = baseBoosterPackCardsGroupedByRarity.Where(g => !g.Key.IsSpecial()).OrderBy(g => g.Key.Ratio);

      IList<UserCard> boosterPackUserCards = new List<UserCard>();
      IList<BaseBoosterPackCard> chosenNonSpecialBaseBoosterPackCards = new List<BaseBoosterPackCard>();

      Random random = new Random();

      for (int i = 0; i < 4; ++i)
      {
        double nonSpecialSelection = random.NextDouble();
        // Console.WriteLine($"nonSpecialSelection: {nonSpecialSelection}");

        foreach (IGrouping<Rarity, BaseBoosterPackCard> nonSpecialGroupOrderedByRatio in nonSpecialGroupings)
        {
          Rarity rarity = nonSpecialGroupOrderedByRatio.Key;

          if (rarity.Ratio >= nonSpecialSelection)
          {
            // Console.WriteLine($"Name: {rarity.Name}, Ratio {rarity.Ratio}, Special {rarity.Special}");
            int index = random.Next(nonSpecialGroupOrderedByRatio.Count());
            BaseBoosterPackCard chosenNonSpecialBaseBoosterPackCard = nonSpecialGroupOrderedByRatio.ToList()[index];
            UserCard chosenNonSpecialUserCardCard = new UserCard();
            chosenNonSpecialUserCardCard.BaseCardId = chosenNonSpecialBaseBoosterPackCard.BaseCardId;
            chosenNonSpecialUserCardCard.Rarity = rarity;
            boosterPackUserCards.Add(chosenNonSpecialUserCardCard);

            // Console.WriteLine($"Chose: {chosenNonSpecialBaseBoosterPackCard.BaseCardId}");
            break;
          }
        }
      }

      double specialSelection = random.NextDouble();
      // Console.WriteLine($"specialSelection: {specialSelection}");

      foreach (IGrouping<Rarity, BaseBoosterPackCard> specialGroupOrderedByRatio in specialGroupings)
      {
        Rarity rarity = specialGroupOrderedByRatio.Key;

        if (rarity.Ratio >= specialSelection)
        {
          // Console.WriteLine($"Name: {rarity.Name}, Ratio {rarity.Ratio}, Special {rarity.Special}");
          int index = random.Next(specialGroupOrderedByRatio.Count());
          BaseBoosterPackCard chosenSpecialBaseBoosterPackCard = specialGroupOrderedByRatio.ToList()[index];
          UserCard chosenSpecialUserCardCard = new UserCard();
          chosenSpecialUserCardCard.BaseCardId = chosenSpecialBaseBoosterPackCard.BaseCardId;
          chosenSpecialUserCardCard.Rarity = rarity;
          boosterPackUserCards.Add(chosenSpecialUserCardCard);

          // Console.WriteLine($"Chose: *{chosenSpecialBaseBoosterPackCard.BaseCardId}*");
          break;
        }
      }

      BoosterPack boosterPack = new BoosterPack(boosterPackUserCards);

      return boosterPack;
    }
  }
}
