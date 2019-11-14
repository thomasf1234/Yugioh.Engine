// // TODO : Flesh out
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Yugioh.Engine.Constants;
using Yugioh.Engine.Entities;
using Yugioh.Engine.Exceptions;
using Yugioh.Engine.Factories;

namespace Yugioh.Engine.Models
{
  public class Shop
  {
    private readonly ILogger<Shop> _logger;
    private readonly YugiohContext _dbContext;
    // public int PasswordMachineCost { get; }
    public IList<BoosterBox> BoosterBoxes { get; }
    public Shop(ILoggerFactory loggerFactory, YugiohContext dbContext)
    {
      this._logger = loggerFactory.CreateLogger<Shop>();
      this._dbContext = dbContext;
      this.BoosterBoxes =  new List<BoosterBox>();
      // this.PasswordMachineCost = 1000;
    }

    public void Stock(IBoosterPackFactory boosterPackFactory, string baseBoosterPackName)
    {
      BaseBoosterPack baseBoosterPack = this._dbContext.BaseBoosterPack.Where(bbp => bbp.Name == baseBoosterPackName)
        .Include(bp => bp.BaseBoosterPackCards).ThenInclude(bpc => bpc.Rarity)
        .Include(bp => bp.BaseBoosterPackCards).ThenInclude(bpc => bpc.BaseCard).ThenInclude(bc => bc.MonsterTypes)
        .First();
      int boosterBoxLimit = baseBoosterPack.IsStarterDeck() ? 1 : 24;

      BoosterBox boosterBox = new BoosterBox(baseBoosterPack.Name, Convert.ToInt32(baseBoosterPack.Cost), boosterBoxLimit);

      while (!boosterBox.HasReachedLimit())
      {
        this._logger.LogInformation($"Building boosterpack from source {baseBoosterPack.Name}");
        BoosterPack boosterPack = boosterPackFactory.Build(baseBoosterPack);
        boosterBox.AddPack(boosterPack);
      }

      this.BoosterBoxes.Add(boosterBox);
    }

    public BoosterPack Buy(User user, BoosterBox boosterBox)
    {
      if (IsOpen())
      {
        if (user.Dp >= boosterBox.PackCost)
        {
          if (boosterBox.HasPacks())
          {
            BoosterPack boosterPack = boosterBox.ShiftPack();

            this._logger.LogInformation($"{user.Username} bought {boosterBox.PackName} so reducing DP by ({boosterBox.PackCost})");
            user.Dp -= boosterBox.PackCost;

            return boosterPack;
          }
          else
          {
            throw new BoosterBoxEmptyException();
          }
        }
        else
        {
          throw new NotEnoughDpException(boosterBox.PackCost, user.Dp);
        }
      }
      else
      {
        throw new ShopClosedException();
      }
    }

    public bool IsOpen()
    {
      bool beforeLunch = DateTime.Now.Hour >= 9 && DateTime.Now.Hour < 12;
      bool afterLunch = DateTime.Now.Hour >= 13 && DateTime.Now.Hour < 21;

      // return beforeLunch || afterLunch;
      return true;
    }

    public bool IsClosed()
    {
      return !IsOpen();
    }

    // public UserCard PasswordMachine(User user, string serialNumber)
    // {
    //   if (IsOpen())
    //   {
    //     if (user.Dp >= this.PasswordMachineCost)
    //     {
    //       IList<BaseCard> baseCards = this._dbContext.BaseCard.Where(bc => bc.SerialNumber == serialNumber).ToList();

    //       if (baseCards.Count == 0)
    //       {
    //         this._logger.LogDebug($"Could not find BaseCard with serial number {serialNumber}");
    //         return null;
    //       }
    //       else if (baseCards.Count == 1)
    //       {
    //         BaseCard baseCard = baseCards.First();
    //         this._logger.LogDebug($"serial number {serialNumber} matches {baseCard.Name}");
    //         Rarity commonRarity = this._dbContext.Rarity.Where(r => r.Name == Rarities.Common).First();

    //         UserCard userCard = new UserCard();
    //         userCard.BaseCard = baseCard;
    //         userCard.BaseCardId = baseCard.BaseCardId;
    //         userCard.Rarity = commonRarity;
    //         userCard.RarityId = commonRarity.RarityId;
    //         userCard.UserId = user.UserId;

    //         user.UserCards.Add(userCard);
    //         user.Dp -= this.PasswordMachineCost;

    //         this._logger.LogDebug($"Added {baseCard.DbName} to trunk for {user.Username} and reduce user db by {this.PasswordMachineCost}");

    //         return userCard;
    //       }
    //       else
    //       {
    //         throw new DuplicateSerialNumberException(serialNumber, "Duplicate serial number found through the password machine");
    //       }

    //     }
    //     else
    //     {
    //       throw new NotEnoughDpException(this.PasswordMachineCost, user.Dp);
    //     }
    //   }
    //   else
    //   {
    //     throw new ShopClosedException();
    //   }
    // }
  }
}
