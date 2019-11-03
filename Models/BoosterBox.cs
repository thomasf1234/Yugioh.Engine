// TODO : Flesh out
using System;
using System.Collections.Generic;
using System.Linq;

using Yugioh.Engine.Entities;
using Yugioh.Engine.Exceptions;

namespace Yugioh.Engine.Models
{
  public class BoosterBox
  {
    public string PackName { get; }
    public int PackCost { get; }
    public int PackLimit { get; }
    public IList<BoosterPack> Packs { get; set; }

    public BoosterBox(string _packName)
    {
      this.PackName = _packName;
      this.PackCost = 300;
      this.PackLimit = 24;
      this.Packs = new List<BoosterPack>();
    }

    public BoosterBox(string _packName, int _packCost)
    {
      this.PackName = _packName;
      this.PackCost = _packCost;
      this.PackLimit = 24;
      this.Packs = new List<BoosterPack>();
    }

    public BoosterBox(string _packName, int _packCost, int _packLimit)
    {
      this.PackName = _packName;
      this.PackCost = _packCost;
      this.PackLimit = _packLimit;
      this.Packs = new List<BoosterPack>();
    }

    public bool HasPacks()
    {
      return this.Packs.Any();
    }

    public bool HasReachedLimit()
    {
      return this.Packs.Count == this.PackLimit;
    }

    public void AddPack(BoosterPack boosterPack)
    {
      if (HasReachedLimit())
      {
        throw new BoosterBoxFullException();
      }
      else
      {
        this.Packs.Add(boosterPack);
      }
    }

    public BoosterPack ShiftPack()
    {
      if (HasPacks())
      {
        BoosterPack boosterPack = this.Packs[0];
        this.Packs.Remove(boosterPack);
        return boosterPack;
      }
      else
      {
        throw new BoosterBoxEmptyException();
      }
    }
  }
}
