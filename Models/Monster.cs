
using System;
using System.Collections.Generic;

using Yugioh.Engine.Constants;
using Yugioh.Engine.Entities;

namespace Yugioh.Engine.Models
{
  public class Monster
  {
    public Card Card { get; set; }
    public string Category { get; set; }
    public string Name { get; set; }
    public int? Level { get; set; }
    public int? Rank { get; set; }
    public int? PendulumScale { get; set; }
    public string Attribute { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public virtual ICollection<string> Types { get; set; }

    public Monster(Card _card)
    {
      this.Card = _card;

      BaseCard baseCard = _card.UserCard.BaseCard;

      this.Category = baseCard.Category;
      this.Name = baseCard.Name;
      this.Level = baseCard.Level;
      this.Rank = baseCard.Rank;
      this.PendulumScale = baseCard.PendulumScale;
      this.Attribute = baseCard.CardAttribute;
    
      int _attack = 0;
      int _defense = 0;
      Int32.TryParse(baseCard.Attack, out _attack);
      Int32.TryParse(baseCard.Defense, out _defense);

      this.Attack = _attack; 
      this.Defense = _defense;

      this.Types = new List<string>();

      foreach (MonsterType monsterType in baseCard.MonsterTypes)
      {
        this.Types.Add(monsterType.Name);
      }
    }

    public bool IsInAttackPosition()
    {
      return this.Card.IsVertical();
    }

    public bool IsInDefensePosition()
    {
      return this.Card.IsHorizontal();
    }

    public bool IsSpell()
    {
      return this.Category == Categories.Spell;
    }

    public bool IsTrap()
    {
      return this.Category == Categories.Trap;
    }

    public bool IsNormal()
    {
      return this.Category == Categories.Normal;
    }

    public bool IsAqua()
    {
      return this.Types.Contains(MonsterTypes.Aqua);
    }

    public bool IsBeast()
    {
      return this.Types.Contains(MonsterTypes.Beast);
    }

    public bool IsBeastWarrior()
    {
      return this.Types.Contains(MonsterTypes.BeastWarrior);
    }

    public bool IsCharisma()
    {
      return this.Types.Contains(MonsterTypes.Charisma);
    }

    public bool IsCreatorGod()
    {
      return this.Types.Contains(MonsterTypes.CreatorGod);
    }

    public bool IsCyberse()
    {
      return this.Types.Contains(MonsterTypes.Cyberse);
    }

    public bool IsDarkTuner()
    {
      return this.Types.Contains(MonsterTypes.DarkTuner);
    }

    public bool IsDinosaur()
    {
      return this.Types.Contains(MonsterTypes.Dinosaur);
    }

    public bool IsDivineBeast()
    {
      return this.Types.Contains(MonsterTypes.DivineBeast);
    }

    public bool IsDragon()
    {
      return this.Types.Contains(MonsterTypes.Dragon);
    }

    public bool IsEffect()
    {
      return this.Types.Contains(MonsterTypes.Effect);
    }

    public bool IsFairy()
    {
      return this.Types.Contains(MonsterTypes.Fairy);
    }

    public bool IsFiend()
    {
      return this.Types.Contains(MonsterTypes.Fiend);
    }

    public bool IsFish()
    {
      return this.Types.Contains(MonsterTypes.Fish);
    }

    public bool IsFlip()
    {
      return this.Types.Contains(MonsterTypes.Flip);
    }

    public bool IsFusion()
    {
      return this.Types.Contains(MonsterTypes.Fusion);
    }

    public bool IsGemini()
    {
      return this.Types.Contains(MonsterTypes.Gemini);
    }

    public bool IsInsect()
    {
      return this.Types.Contains(MonsterTypes.Insect);
    }

    public bool IsLink()
    {
      return this.Types.Contains(MonsterTypes.Link);
    }

    public bool IsMachine()
    {
      return this.Types.Contains(MonsterTypes.Machine);
    }

    public bool IsPendulum()
    {
      return this.Types.Contains(MonsterTypes.Pendulum);
    }

    public bool IsPlant()
    {
      return this.Types.Contains(MonsterTypes.Plant);
    }

    public bool IsPsychic()
    {
      return this.Types.Contains(MonsterTypes.Psychic);
    }

    public bool IsPyro()
    {
      return this.Types.Contains(MonsterTypes.Pyro);
    }

    public bool IsReptile()
    {
      return this.Types.Contains(MonsterTypes.Reptile);
    }

    public bool IsRitual()
    {
      return this.Types.Contains(MonsterTypes.Ritual);
    }

    public bool IsRock()
    {
      return this.Types.Contains(MonsterTypes.Rock);
    }

    public bool IsSeaSerpent()
    {
      return this.Types.Contains(MonsterTypes.SeaSerpent);
    }

    public bool IsSpellcaster()
    {
      return this.Types.Contains(MonsterTypes.Spellcaster);
    }

    public bool IsSpirit()
    {
      return this.Types.Contains(MonsterTypes.Spirit);
    }

    public bool IsSynchro()
    {
      return this.Types.Contains(MonsterTypes.Synchro);
    }

    public bool IsThunder()
    {
      return this.Types.Contains(MonsterTypes.Thunder);
    }

    public bool IsToken()
    {
      return this.Types.Contains(MonsterTypes.Token);
    }

    public bool IsToon()
    {
      return this.Types.Contains(MonsterTypes.Toon);
    }

    public bool IsTuner()
    {
      return this.Types.Contains(MonsterTypes.Tuner);
    }

    public bool IsUnion()
    {
      return this.Types.Contains(MonsterTypes.Union);
    }

    public bool IsWarrior()
    {
      return this.Types.Contains(MonsterTypes.Warrior);
    }

    public bool IsWingedBeast()
    {
      return this.Types.Contains(MonsterTypes.WingedBeast);
    }

    public bool IsWyrm()
    {
      return this.Types.Contains(MonsterTypes.Wyrm);
    }

    public bool IsXyz()
    {
      return this.Types.Contains(MonsterTypes.Xyz);
    }

    public bool IsZombie()
    {
      return this.Types.Contains(MonsterTypes.Zombie);
    }
  }
}
