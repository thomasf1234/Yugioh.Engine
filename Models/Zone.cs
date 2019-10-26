
using System.Collections.Generic;
using Yugioh.Engine.Entities;

namespace Yugioh.Engine.Models
{
  public class Zone
    {
        public IList<Card> Cards { get; set; }

        public Zone()
        {
            this.Cards = new List<Card>();
        }

        public bool IsOccupied()
        {
            return this.Cards.Count > 0;
        }

        public void Set(Card card)
        {
            if (IsOccupied())
            {
                this.Cards.Clear();
            }

            this.Cards.Add(card);
        }
    }
}
