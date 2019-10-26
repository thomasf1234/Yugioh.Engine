using System;
using System.Collections.Generic;

namespace Yugioh.Engine.Models
{
    public abstract class Event
    {
        public Turn Turn { get; }
        public DateTime Timestamp { get; }
        public enum Types : int {
            ShuffleEvent,
            DrawEvent,
            NormalSummonEvent,
            SpecialSummonEvent,
            SetEvent,
            PositionSwitchEvent,
            FlipSummonEvent,
            EffectActivatedEvent,
        }

        public Event(DateTime _timestamp, Turn _turn)
        {
            this.Timestamp = _timestamp;
            this.Turn = _turn;
        }
    }
}
