using System;

namespace Yugioh.Engine.Entities
{
    public partial class UserDeckCard
    {
        public long UserDeckCardId { get; set; }
        public long? UserDeckId { get; set; }
        public long? UserCardId { get; set; }
        public string SubDeck { get; set; }
        public UserDeck UserDeck { get; set; }
        public UserCard UserCard { get; set; }
    }
}
