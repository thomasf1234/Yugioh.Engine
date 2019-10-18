using System;
using System.Collections.Generic;

namespace Yugioh.Engine.Entities
{
    public partial class Card
    {
        public long CardId { get; set; }
        public string DbName { get; set; }
        public string CardType { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public int? Level { get; set; }
        public int? Rank { get; set; }
        public int? PendulumScale { get; set; }
        public string CardAttribute { get; set; }
        public string Property { get; set; }
        public string Attack { get; set; }
        public string Defense { get; set; }
        public string SerialNumber { get; set; }
        public string Description { get; set; }

        public virtual ICollection<MonsterType> MonsterTypes { get; set; }
        public virtual ICollection<Artwork> Artworks { get; set; }
    }
}
