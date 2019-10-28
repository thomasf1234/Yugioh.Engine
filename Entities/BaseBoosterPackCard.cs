using System;
using System.Collections.Generic;

namespace Yugioh.Engine.Entities
{
    public partial class BaseBoosterPackCard
    {
        public long BaseBoosterPackCardId { get; set; }
        public long? BaseBoosterPackId { get; set; }
        public long? BaseCardId { get; set; }
        public long? RarityId { get; set; }

        public virtual BaseCard BaseCard { get; set; }
        public virtual Rarity Rarity { get; set; }
    }
}
