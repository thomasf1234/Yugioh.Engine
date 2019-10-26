
using System.Collections.Generic;
using Yugioh.Engine.Entities;

namespace Yugioh.Engine.Models
{
  public class Card
    {
        public enum Orientations : int { FaceUp, FaceDown }
        public enum Positions : int { Vertical, Horizontal }

        public Orientations Orientation { get; set; }
        public Positions Position { get; set; }
        public readonly UserCard UserCard;
        public Player Owner { get; set; }
        public IList<Counter> Counters { get; set; }

        public Card(UserCard _userCard, Player _owner)
        {
            this.UserCard = _userCard;
            this.Owner = _owner;
            this.Counters = new List<Counter>();
        }

        public bool IsFaceUp()
        {
            return this.Orientation == Orientations.FaceUp;
        }

        public bool IsFaceDown()
        {
            return this.Orientation == Orientations.FaceDown;
        }

        public bool IsVertical()
        {
            return this.Position == Positions.Vertical;
        }

        public bool IsHorizontal()
        {
            return this.Position == Positions.Horizontal;
        }

        public void SetVertical()
        {
            this.Position = Positions.Vertical;
        }

        public void SetHorizontal()
        {
            this.Position = Positions.Horizontal;
        }

        public void SetFaceUp()
        {
            this.Orientation = Orientations.FaceUp;
        }

        public void SetFaceDown()
        {
            this.Orientation = Orientations.FaceDown;
        }
    }
}
