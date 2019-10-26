using System;

namespace Yugioh.Engine.Models.Events
{
  public class AttackEvent : Event
  {
    public Monster AttackingMonster { get; }
    public Monster AttackedMonster { get; }
    public Player AttackingPlayer { get; }
    public Player AttackedPlayer { get; }

    public AttackEvent(DateTime _timestamp, Turn _turn, Monster _attackingMonster, Monster _attackedMonster, Player _attackingPlayer, Player _attackedPlayer) : base(_timestamp, _turn)
    {
      this.AttackingMonster = _attackingMonster;
      this.AttackedMonster = _attackedMonster;
      this.AttackingPlayer = _attackingPlayer;
      this.AttackedPlayer = _attackedPlayer;
    }
  }
}
