using System;
using System.Collections.Generic;

namespace Yugioh.Engine.Entities
{
    public partial class User
    {
        public long UserId { get; set; }
        public string Username { get; set; }
        public byte[] EncryptedPassword { get; set; }
        public long? Dp { get; set; }

        public virtual ICollection<UserCard> UserCards { get; set; }
        public virtual ICollection<UserDeck> UserDecks { get; set; }
    }
}
