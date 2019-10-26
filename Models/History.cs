using System;
using System.Collections.Generic;
using Yugioh.Engine.Models.Events;

namespace Yugioh.Engine.Models
{
  public class History
    {
      public IList<Event> Events { get; }

      public History()
      {
        this.Events = new List<Event>();
      }

      public bool HasNormalSummonedOrSet(Turn turn)
      {
        bool normalSummondOrSet = false;

        foreach (Event _event in this.Events)
        {
          if (_event.Turn == turn)
          {
            Type eventType = _event.GetType();
            if (eventType == typeof(NormalSummonOrSetEvent))
            {
              normalSummondOrSet = true;
              break;
            }
          }
        }

        return normalSummondOrSet;
      }

      public bool HasMonsterAttacked(Turn turn, Monster monster)
      {
        bool hasAttacked = false;

        foreach (Event _event in this.Events)
        {
          if (_event.Turn == turn)
          {
            Type eventType = _event.GetType();
            if (eventType == typeof(AttackEvent))
            {
              AttackEvent attackEvent = (AttackEvent) _event;

              if (attackEvent.AttackingMonster == monster)
              {
                hasAttacked = true;
                break;
              }   
            }
          }
        }

        return hasAttacked;
      }
    }
}
