// // // TODO : Flesh out
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Logging;

// using Yugioh.Engine.Constants;
// using Yugioh.Engine.Entities;
// using Yugioh.Engine.Exceptions;
// using Yugioh.Engine.Factories;

// namespace Yugioh.Engine.Models
// {
//   public class Shop
//   {
//     private readonly ILogger<Shop> _logger;
//     public IList<CardPackBox> CardPackBoxes { get; }
//     public Shop(ILoggerFactory loggerFactory)
//     {
//       this._logger = loggerFactory.CreateLogger<Shop>();
//       this.CardPackBoxes =  new List<CardPackBox>();
//     }

//     public void Stock(IBoosterPackFactory boosterPackFactory, string baseBoosterPackName)
//     {
//       BaseBoosterPack baseBoosterPack = this._dbContext.BaseBoosterPack.Where(bbp => bbp.Name == baseBoosterPackName)
//         .Include(bp => bp.BaseBoosterPackCards).ThenInclude(bpc => bpc.Rarity)
//         .Include(bp => bp.BaseBoosterPackCards).ThenInclude(bpc => bpc.BaseCard).ThenInclude(bc => bc.MonsterTypes)
//         .First();
//       int boosterBoxLimit = baseBoosterPack.IsStarterDeck() ? 1 : 24;

//       BoosterBox boosterBox = new BoosterBox(baseBoosterPack.Name, Convert.ToInt32(baseBoosterPack.Cost), boosterBoxLimit);

//       while (!boosterBox.HasReachedLimit())
//       {
//         this._logger.LogInformation($"Building boosterpack from source {baseBoosterPack.Name}");
//         BoosterPack boosterPack = boosterPackFactory.Build(baseBoosterPack);
//         boosterBox.AddPack(boosterPack);
//       }

//       this.BoosterBoxes.Add(boosterBox);
//     }

//     public CardPack Buy(User user, BoosterBox boosterBox)
//     {
//       if (IsOpen())
//       {
//         if (user.Dp >= boosterBox.PackCost)
//         {
//           if (boosterBox.HasPacks())
//           {
//             BoosterPack boosterPack = boosterBox.ShiftPack();

//             this._logger.LogInformation($"{user.Username} bought {boosterBox.PackName} so reducing DP by ({boosterBox.PackCost})");
//             user.DuelistPoints -= boosterBox.PackCost;

//             return boosterPack;
//           }
//           else
//           {
//             throw new BoosterBoxEmptyException();
//           }
//         }
//         else
//         {
//           throw new NotEnoughDpException(boosterBox.PackCost, user.Dp);
//         }
//       }
//       else
//       {
//         throw new ShopClosedException();
//       }
//     }

//     public bool IsOpen()
//     {
//       bool beforeLunch = DateTime.Now.Hour >= 9 && DateTime.Now.Hour < 12;
//       bool afterLunch = DateTime.Now.Hour >= 13 && DateTime.Now.Hour < 21;

//       // return beforeLunch || afterLunch;
//       return true;
//     }

//     public bool IsClosed()
//     {
//       return !IsOpen();
//     }
//   }
// }
