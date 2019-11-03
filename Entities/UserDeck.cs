using System;
using System.Collections.Generic;

namespace Yugioh.Engine.Entities
{
  public partial class UserDeck
  {
    public long UserDeckId { get; set; }
    public long? UserId { get; set; }
    public string Name { get; set; }
    public User User { get; set; }
    public virtual ICollection<UserDeckCard> UserDeckCards { get; set; }

    public UserDeck()
    {
      this.UserDeckCards = new HashSet<UserDeckCard>();
    }
  }
}
