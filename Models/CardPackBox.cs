// TODO : Flesh out
using System;
using System.Collections.Generic;
using System.Linq;

using Yugioh.Engine.Entities;
using Yugioh.Engine.Exceptions;

namespace Yugioh.Engine.Models
{
  public class CardPackBox
  {
    public string PackName { get; }
    public int PackCost { get; }
    public int PackLimit { get; }
    public IList<CardPack> Packs { get; set; }

    public CardPackBox(string _packName)
    {
      this.PackName = _packName;
      this.PackCost = 300;
      this.PackLimit = 24;
      this.Packs = new List<CardPack>();
    }

    public CardPackBox(string _packName, int _packCost)
    {
      this.PackName = _packName;
      this.PackCost = _packCost;
      this.PackLimit = 24;
      this.Packs = new List<CardPack>();
    }

    public CardPackBox(string _packName, int _packCost, int _packLimit)
    {
      this.PackName = _packName;
      this.PackCost = _packCost;
      this.PackLimit = _packLimit;
      this.Packs = new List<CardPack>();
    }

    public bool HasPacks()
    {
      return this.Packs.Any();
    }

    public bool HasReachedLimit()
    {
      return this.Packs.Count == this.PackLimit;
    }

    public void AddPack(CardPack cardPack)
    {
      if (HasReachedLimit())
      {
        throw new CardPackBoxFullException();
      }
      else
      {
        this.Packs.Add(cardPack);
      }
    }

    public CardPack ShiftPack()
    {
      if (HasPacks())
      {
        CardPack cardPack = this.Packs[0];
        this.Packs.Remove(cardPack);
        return cardPack;
      }
      else
      {
        throw new CardPackBoxEmptyException();
      }
    }
  }
}
