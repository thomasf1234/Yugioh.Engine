using System.Collections.Generic;

using Yugioh.Engine.Entities;

namespace Yugioh.Engine.Models
{
    public class FieldSide
    {
        public Deck MainDeck { get; set; }
        public Deck ExtraDeck { get; set; }
        public IList<UserCard> Graveyard { get; set; }
        public IList<UserCard> MonsterZones { get; set; }
        public IList<UserCard> SpellTrapZones { get; set; }
        public IList<UserCard> PendulumZones { get; set; }
        public UserCard FieldSpell { get; set; }

        public FieldSide()
        {
            this.MainDeck = null;
            this.ExtraDeck = null;
            this.Graveyard = new List<UserCard>();
            this.MonsterZones = new List<UserCard>(5) {null, null, null, null, null};
            this.SpellTrapZones = new List<UserCard>(5) {null, null, null, null, null};
            this.PendulumZones = new List<UserCard>(2) {null, null};
            this.FieldSpell = null;
        }
    }
}
