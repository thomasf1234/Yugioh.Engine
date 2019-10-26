using System.Collections.Generic;
using Yugioh.Engine.Models.Zones;

namespace Yugioh.Engine.Models
{
  public class FieldSide
    {
        public Zone DeckZone { get; set; }
        public Zone ExtraDeckZone { get; set; }
        public Zone Graveyard { get; set; }
        public IList<MonsterZone> MonsterZones { get; set; }
        public IList<SpellTrapZone> SpellTrapZones { get; set; }
        public Zone FieldZone { get; set; }

        public FieldSide()
        {
            this.DeckZone = new Zone();
            this.ExtraDeckZone = new Zone();
            this.Graveyard = new Zone();
            this.MonsterZones = new List<MonsterZone>();
            this.SpellTrapZones = new List<SpellTrapZone>();
            this.FieldZone = new Zone();

            for (int i=0; i< 5; ++i)
            {
                this.MonsterZones.Add(new MonsterZone());
                this.SpellTrapZones.Add(new SpellTrapZone());
            }
        }
    }
}
