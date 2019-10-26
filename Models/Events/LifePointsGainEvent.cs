using System;

namespace Yugioh.Engine.Models.Events
{
  public class LifePointsGainEvent : Event
  {
    public Player Player { get; }
    public int Amount { get; }

    public LifePointsGainEvent(DateTime _timestamp, Turn _turn, Player _player, int _amount) : base(_timestamp, _turn)
    {
      this.Player = _player;
      this.Amount = _amount;
    }
  }
}
