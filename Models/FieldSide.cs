using System.Collections.Generic;
using Yugioh.Engine.Entities;

namespace Yugioh.Engine.Models
{
    public class FieldSide
    {
        public Deck MainDeck { get; set; }
        public Deck ExtraDeck { get; set; }
        public IList<Card> Graveyard { get; set; }
        public IList<Card> MonsterZones { get; set; }
        public IList<Card> SpellTrapZones { get; set; }
        public IList<Card> PendulumZones { get; set; }
        public Card FieldSpell { get; set; }

        public FieldSide()
        {
            this.MainDeck = null;
            this.ExtraDeck = null;
            this.Graveyard = new List<Card>();
            this.MonsterZones = new List<Card>(5) {null, null, null, null, null};
            this.SpellTrapZones = new List<Card>(5) {null, null, null, null, null};
            this.PendulumZones = new List<Card>(2) {null, null};
            this.FieldSpell = null;
        }
    }
}
