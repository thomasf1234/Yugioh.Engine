using System;
using System.Collections.Generic;

namespace Yugioh.Engine.Entities
{
    public partial class BoosterPackCard
    {
        public long BoosterPackCardId { get; set; }
        public long? BoosterPackId { get; set; }
        public long? CardId { get; set; }
        public string Rarity { get; set; }
    }
}
