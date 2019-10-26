using System;

namespace Yugioh.Engine.Models.Events
{
  public class NormalSummonOrSetEvent : Event
  {
    public Player Player { get; }
    public Card Card { get; }
    public Zone Zone { get; }
    public bool Set { get; }

    public NormalSummonOrSetEvent(DateTime _timestamp, Turn _turn, Player _player, Card _card, Zone _zone, bool _set) : base(_timestamp, _turn)
    {
      this.Player = _player;
      this.Card = _card;
      this.Zone = _zone;
      this.Set = _set;
    }

    public bool IsSet()
    {
      return this.Set == true;
    }

    public bool IsNormalSummon()
    {
      return !IsSet();
    }
  }
}
