using System;
using System.Collections.Generic;

namespace Yugioh.Engine.Entities
{
    public partial class UserCard
    {
        public long UserCardId { get; set; }
        public long? UserId { get; set; }
        public long? BaseCardId { get; set; }
        public long? ArtworkId { get; set; }
        public User User { get; set; }
        public BaseCard BaseCard { get; set; }
        public Artwork Artwork { get; set; }
    }
}
