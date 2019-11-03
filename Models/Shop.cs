// // TODO : Flesh out
using System;
using System.Collections.Generic;
using System.Linq;
using Serilog;

using Yugioh.Engine.Constants;
using Yugioh.Engine.Entities;
using Yugioh.Engine.Exceptions;

namespace Yugioh.Engine.Models
{
  public class Shop
  {
    public int PasswordMachineCost { get; }
    public IList<BoosterBox> BoosterBoxes { get; }
    public Shop(IList<BoosterBox> BoosterBoxes)
    {
      this.BoosterBoxes = BoosterBoxes;
      this.PasswordMachineCost = 1000;
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

            using (var context = new YugiohContext())
            {
              user.Dp -= boosterBox.PackCost;
              context.SaveChanges();
            }

            Log.Debug($"User {user.Username} bought booster pack from the {boosterBox.PackName} box. Reduced user db by {boosterBox.PackCost}");

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

    public UserCard PasswordMachine(User user, string serialNumber)
    {
      if (IsOpen())
      {
        if (user.Dp >= this.PasswordMachineCost)
        {
          using (var context = new YugiohContext())
          {
            IList<BaseCard> baseCards = context.BaseCard.Where(bc => bc.SerialNumber == serialNumber).ToList();

            if (baseCards.Count == 0)
            {
              Log.Debug($"Could not find BaseCard with serial number {serialNumber}");
              return null;
            }
            else if (baseCards.Count == 1)
            {
              BaseCard baseCard = baseCards.First();
              Log.Debug($"serial number {serialNumber} matches {baseCard.Name}");
              Rarity commonRarity = context.Rarity.Where(r => r.Name == Rarities.Common).First();

              UserCard userCard = new UserCard();
              userCard.BaseCard = baseCard;
              userCard.BaseCardId = baseCard.BaseCardId;
              userCard.Rarity = commonRarity;
              userCard.RarityId = commonRarity.RarityId;
              userCard.UserId = user.UserId;

              using (var dbContextTransaction = context.Database.BeginTransaction())
              {
                user.UserCards.Add(userCard);
                // context.UserCard.Add(userCard);
                user.Dp -= this.PasswordMachineCost;

                context.SaveChanges();
                dbContextTransaction.Commit();
              }

              Log.Debug($"Added {baseCard.DbName} to trunk for {user.Username} and reduce user db by {this.PasswordMachineCost}");

              return userCard;
            }
            else
            {
              throw new DuplicateSerialNumberException(serialNumber, "Duplicate serial number found through the password machine");
            }
          }
        }
        else
        {
          throw new NotEnoughDpException(this.PasswordMachineCost, user.Dp);
        }
      }
      else
      {
        throw new ShopClosedException();
      }
    }
  }
}
