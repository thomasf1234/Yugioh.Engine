using System;

namespace Yugioh.Engine.Models.Events
{
  public class ShuffleDeckEvent : Event
  {
    public Player Player { get; }

    public ShuffleDeckEvent(DateTime _timestamp, Turn _turn, Player player) : base(_timestamp, _turn)
    {
      this.Player = player;
    }
  }
}
