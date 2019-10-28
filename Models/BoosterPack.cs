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
    private States _state;
    private IList<UserCard> _userCards;

    public BoosterPack(IList<UserCard> userCards)
    {
      // Ensure the cards no longer belong to a user
      foreach (UserCard userCard in userCards)
      {
        userCard.UserId = null;
      }

      this._userCards = userCards;
      this._state = States.Sealed;
    }

    public IList<UserCard> Open()
    {
      if (IsSealed())
      {
        IList<UserCard> userCards = new List<UserCard>();

        foreach (UserCard userCard in this._userCards)
        {
          userCards.Add(userCard);
        }

        // Empty our pack
        this._userCards.Clear();

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
