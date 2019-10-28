﻿using System;
using System.Collections.Generic;

namespace Yugioh.Engine.Entities
{
    public partial class UserCard
    {
        public long UserCardId { get; set; }
        public long? UserId { get; set; }
        public long? BaseCardId { get; set; }
        public long? ArtworkId { get; set; }
        public long? RarityId { get; set; }
        public virtual User User { get; set; }
        public virtual BaseCard BaseCard { get; set; }
        public virtual Artwork Artwork { get; set; }
        public virtual Rarity Rarity { get; set; }
    }
}
