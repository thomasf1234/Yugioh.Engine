using System;

namespace Yugioh.Engine.Models.Events
{
  public class DestroyedCardEvent : Event
  {
    public Player Player { get; }
    public Card Card { get; }

    public Zone Zone { get; }

    public DestroyedCardEvent(DateTime _timestamp, Turn _turn, Player _player, Card _card, Zone _zone) : base(_timestamp, _turn)
    {
      this.Player = _player;
      this.Card = _card;
      this.Zone = _zone;
    }
  }
}
