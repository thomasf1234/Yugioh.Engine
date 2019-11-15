// TODO : Flesh out
using System.Collections.Generic;

using Yugioh.Engine.Exceptions;
using Yugioh.Engine.Models.Cards;

namespace Yugioh.Engine.Models
{
  public class CardPack
  {
    public enum States : int { Sealed = 0, Opened = 1 };
    public IList<Card> Cards { get; }
    private States _state;

    public CardPack(IList<Card> _cards)
    {
      this.Cards = _cards;
      this._state = States.Sealed;
    }

    public IList<Card> Open()
    {
      if (IsSealed())
      {
        IList<Card> cards = new List<Card>();

        foreach (Card card in this.Cards)
        {
          cards.Add(card);
        }

        // Empty our pack
        this.Cards.Clear();

        // Set state to Opened;
        this._state = States.Opened;

        return cards;
      }
      else
      {
        throw new CardPackAlreadyOpenedException();
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
