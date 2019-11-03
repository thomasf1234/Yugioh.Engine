// TODO : Flesh out
using System;
using System.Collections.Generic;

using Yugioh.Engine.Entities;
using Yugioh.Engine.Exceptions;

namespace Yugioh.Engine.Models
{
  public class BoosterPack
  {
    public enum States : int { Sealed = 0, Opened = 1 };
    public BaseBoosterPack BaseBoosterPack { get; }
    public IList<UserCard> UserCards { get; }
    private States _state;

    public BoosterPack(BaseBoosterPack _baseBoosterPack, IList<UserCard> _userCards)
    {
      this.BaseBoosterPack = _baseBoosterPack;
      // Ensure the cards no longer belong to a user
      foreach (UserCard userCard in _userCards)
      {
        userCard.UserId = null;
      }

      this.UserCards = _userCards;
      this._state = States.Sealed;
    }

    public IList<UserCard> Open()
    {
      if (IsSealed())
      {
        IList<UserCard> userCards = new List<UserCard>();

        foreach (UserCard userCard in this.UserCards)
        {
          userCards.Add(userCard);
        }

        // Empty our pack
        this.UserCards.Clear();

        // Set state to Opened;
        this._state = States.Opened;

        return userCards;
      }
      else
      {
        throw new BoosterPackAlreadyOpenedException();
      }
    }

    public bool IsSealed()
    {
      return this._state == States.Sealed;
    }

    public bool IsOpened()
    {
      return this._state == States.Opened;
    }
  }
}
