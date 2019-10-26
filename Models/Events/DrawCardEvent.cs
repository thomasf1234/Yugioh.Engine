using System;

namespace Yugioh.Engine.Models.Events
{
  public class DrawCardEvent : Event
  {
    public Player Player { get; }

    public DrawCardEvent(DateTime _timestamp, Turn _turn, Player player) : base(_timestamp, _turn)
    {
      this.Player = player;
    }
  }
}
