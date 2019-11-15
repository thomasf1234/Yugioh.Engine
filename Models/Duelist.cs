// using System;
// using System.Collections.Generic;

// using Yugioh.Engine.Entities;
// using Yugioh.Engine.Exceptions;

// namespace Yugioh.Engine.Models
// {
//     public class Duelist
//     {
//         public User User { get; set; }
//         public int LifePoints { get; set; }
//         public IList<Card> Hand { get; set; }
//         public IList<Card> RemovedFromPlay { get; set; }
//         public Player Opponent { get; set; }
//         public FieldSide FieldSide { get; set; }
//         public Deck MainDeck { get; set; }
//         public Deck ExtraDeck { get; set; }
//         public Deck SideDeck { get; set; }
//         public IList<Turn> Turns { get; set; }

//         public Player(User _user, Deck mainDeck,  Deck extraDeck, Deck sideDeck)
//         {
//             this.User = _user;

//             this.MainDeck = mainDeck;
//             this.ExtraDeck = extraDeck;
//             this.SideDeck = sideDeck;

//             this.Hand = new List<Card>();
//             this.RemovedFromPlay = new List<Card>();
//             this.Turns = new List<Turn>();
//         }
//     }
// }
