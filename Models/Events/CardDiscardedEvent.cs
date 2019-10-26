using System;

namespace Yugioh.Engine.Models.Events
{
  public class CardDiscardedEvent : Event
  {
    public Player Player { get; }
    public Card Card { get; }

    public CardDiscardedEvent(DateTime _timestamp, Turn _turn, Player _player, Card _card) : base(_timestamp, _turn)
    {
      this.Player = _player;
      this.Card = _card;
    }
  }
}
