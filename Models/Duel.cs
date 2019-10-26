using System;
using System.Collections.Generic;
using System.Linq;

using Stateless;
using Yugioh.Engine.Entities;
using Yugioh.Engine.Exceptions;
using Yugioh.Engine.Models.Events;
using Yugioh.Engine.Models.Zones;

namespace Yugioh.Engine.Models
{
  public class Duel
  {
    public enum States : int { New, Preparing, Ready, TurnStart, DrawPhase, StandbyPhase, MainPhase1, BattlePhaseStartStep, BattlePhaseBattleStep, BattlePhaseDamageStep, BattlePhaseEndStep, MainPhase2, EndPhase, TurnEnd, Settled, Cancelled }
    public Player Winner { get; set; }
    public History History { get; }
    public IList<Player> Players { get; }
    public IList<Turn> Turns { get; set; }
    private enum Triggers { Confirmed, Prepared, NewTurn, TurnStarted, DrawPhaseEnded, StandbyPhasePhaseEnded, BattlePhaseEntered, BattleStepEntered, AttackDeclared, AttackResolved, BattlePhaseEnded, MainPhase2Entered, EndPhaseEntered, TurnEnded, WinnerDeclared, Cancel }
    private readonly StateMachine<States, Triggers> stateMachine;
    private StateMachine<States, Triggers>.TriggerWithParameters<Player> newTurnTrigger;
    private StateMachine<States, Triggers>.TriggerWithParameters<Monster, Monster> attackDeclaredTrigger;

    private StateMachine<States, Triggers>.TriggerWithParameters<Player> winnerDeclaredTrigger;

    public Duel(Player _player1, Player _player2)
    {
      this.History = new History();

      this.Winner = null;
      this.Turns = new List<Turn>();

      // Set the player opponents
      _player1.Opponent = _player2;
      _player2.Opponent = _player1;

      this.Players = new List<Player>() { _player1, _player2 };

      this.stateMachine = new StateMachine<States, Triggers>(States.New);

      // State - New
      this.stateMachine.Configure(States.New)
        .Permit(Triggers.Confirmed, States.Preparing)
        .Permit(Triggers.Cancel, States.Cancelled);

      // State - Preparing
      this.stateMachine.Configure(States.Preparing)
        .OnEntry(() =>
        {
          OnPreparing();
        })
        .Permit(Triggers.Prepared, States.Ready)
        .Permit(Triggers.WinnerDeclared, States.Settled)
        .Permit(Triggers.Cancel, States.Cancelled);

      // State - Ready
      this.stateMachine.Configure(States.Ready)
          .Permit(Triggers.NewTurn, States.TurnStart)
          .Permit(Triggers.Cancel, States.Cancelled);

      // State - TurnStart
      this.newTurnTrigger = this.stateMachine.SetTriggerParameters<Player>(Triggers.NewTurn);

      this.stateMachine.Configure(States.TurnStart)
        .OnEntryFrom(newTurnTrigger, (turnPlayer) =>
        {
          OnTurnStart(turnPlayer);
        })
        .Permit(Triggers.TurnStarted, States.DrawPhase)
        .Permit(Triggers.WinnerDeclared, States.Settled)
        .Permit(Triggers.Cancel, States.Cancelled);

      // State - DrawPhase
      this.stateMachine.Configure(States.DrawPhase)
        .OnEntry(() =>
        {
          OnDrawPhase();
        })
        .Permit(Triggers.DrawPhaseEnded, States.StandbyPhase)
        .Permit(Triggers.WinnerDeclared, States.Settled)
        .Permit(Triggers.Cancel, States.Cancelled);

      // State - StandbyPhase
      this.stateMachine.Configure(States.StandbyPhase)
        .OnEntry(() =>
        {
          OnStandbyPhase();
        })
        .Permit(Triggers.StandbyPhasePhaseEnded, States.MainPhase1)
        .Permit(Triggers.WinnerDeclared, States.Settled)
        .Permit(Triggers.Cancel, States.Cancelled);

      // State - MainPhase1
      this.stateMachine.Configure(States.MainPhase1)
        .OnEntry(() =>
        {
          OnMainPhase1();
        })
        // Cannot enter the BattlePhase on the first turn of the duel
        .PermitIf(Triggers.BattlePhaseEntered, States.BattlePhaseStartStep, () => !GetCurrentTurn().IsFirstTurn())
        .Permit(Triggers.EndPhaseEntered, States.EndPhase)
        .Permit(Triggers.WinnerDeclared, States.Settled)
        .Permit(Triggers.Cancel, States.Cancelled);

      // State - BattlePhase
      // State - BattlePhaseStartStep
      this.stateMachine.Configure(States.BattlePhaseStartStep)
        .OnEntry(() =>
        {
          OnBattlePhaseStartStep();
        })
        .Permit(Triggers.BattleStepEntered, States.BattlePhaseBattleStep)
        .Permit(Triggers.WinnerDeclared, States.Settled)
        .Permit(Triggers.Cancel, States.Cancelled);

      // State - BattlePhaseBattleStep
      this.attackDeclaredTrigger = this.stateMachine.SetTriggerParameters<Monster, Monster>(Triggers.AttackDeclared);
      this.stateMachine.Configure(States.BattlePhaseBattleStep)
        .OnEntry(() =>
        {
          OnBattlePhaseBattleStep();
        })
        .Permit(Triggers.AttackDeclared, States.BattlePhaseDamageStep)
        .Permit(Triggers.BattlePhaseEnded, States.BattlePhaseEndStep)
        .Permit(Triggers.WinnerDeclared, States.Settled)
        .Permit(Triggers.Cancel, States.Cancelled);

      // State - BattlePhaseDamageStep
      this.stateMachine.Configure(States.BattlePhaseDamageStep)
        .OnEntryFrom(attackDeclaredTrigger, (attackingMonster, targetMonster) =>
        {
          OnBattlePhaseDamageStep(attackingMonster, targetMonster);
        })
        .Permit(Triggers.AttackResolved, States.BattlePhaseBattleStep)
        .Permit(Triggers.WinnerDeclared, States.Settled)
        .Permit(Triggers.Cancel, States.Cancelled);

      // State - BattlePhaseEndStep
      this.stateMachine.Configure(States.BattlePhaseEndStep)
        .OnEntry(() =>
        {
          OnBattlePhaseEndStep();
        })
        .Permit(Triggers.MainPhase2Entered, States.MainPhase2)
        .Permit(Triggers.WinnerDeclared, States.Settled)
        .Permit(Triggers.Cancel, States.Cancelled);

      // State - MainPhase2
      this.stateMachine.Configure(States.MainPhase2)
        .OnEntry(() =>
        {
          OnMainPhase2();
        })
        .Permit(Triggers.EndPhaseEntered, States.EndPhase)
        .Permit(Triggers.WinnerDeclared, States.Settled)
        .Permit(Triggers.Cancel, States.Cancelled);

      // State - EndPhase
      this.stateMachine.Configure(States.EndPhase)
        .OnEntry(() =>
        {
          OnEndPhase();
        })
        .Permit(Triggers.TurnEnded, States.TurnEnd)
        .Permit(Triggers.WinnerDeclared, States.Settled)
        .Permit(Triggers.Cancel, States.Cancelled);

      // State - TurnEnd
      this.stateMachine.Configure(States.TurnEnd)
        .OnEntry(() =>
        {
          OnTurnEnd();
        })
        .PermitIf(Triggers.NewTurn, States.TurnStart, () => GetCurrentTurn().Player.Hand.Count < 7)
        .Permit(Triggers.WinnerDeclared, States.Settled)
        .Permit(Triggers.Cancel, States.Cancelled);

      // State - Settled
      this.winnerDeclaredTrigger = this.stateMachine.SetTriggerParameters<Player>(Triggers.WinnerDeclared);
      this.stateMachine.Configure(States.Settled)
        .OnEntryFrom(winnerDeclaredTrigger, (wonPlayer) =>
        {
          OnSettled(wonPlayer);
        })
        .Ignore(Triggers.DrawPhaseEnded);
    }

    public States GetState()
    {
      return this.stateMachine.State;
    }

    public Turn GetCurrentTurn()
    {
      if (this.Turns.Any())
      {
        return this.Turns.Last();
      }
      else
      {
        return null;
      }
    }

    public Player GetTurnPlayer()
    {
      return GetCurrentTurn().Player;
    }

    // Actions
    public void Prepare()
    {
      this.stateMachine.Fire(Triggers.Confirmed);
    }

    public void Start(Player firstPlayer)
    {
      this.stateMachine.Fire(this.newTurnTrigger, firstPlayer);
    }

    public void EnterBattlePhase()
    {
      this.stateMachine.Fire(Triggers.BattlePhaseEntered);
    }

    public void EnterEndPhase()
    {
      this.stateMachine.Fire(Triggers.EndPhaseEntered);
    }

    public void EndTurn()
    {
      this.stateMachine.Fire(Triggers.TurnEnded);
    }

    public void EndDrawPhase()
    {
      this.stateMachine.Fire(Triggers.DrawPhaseEnded);
    }

    public void ShuffleDeck(Player player)
    {
      IList<Card> cards = player.FieldSide.DeckZone.Cards;
      Shuffle(cards);

      // Record ShuffleDeckEvent
      ShuffleDeckEvent shuffleDeckEvent = new ShuffleDeckEvent(DateTime.Now, GetCurrentTurn(), player);
      this.History.Events.Add(shuffleDeckEvent);
    }

    public void Shuffle(IList<Card> cards)
    {
      Random _random = new Random();
      int n = cards.Count;
      for (int i = 0; i < n; i++)
      {
        // Use Next on random instance with an argument.
        // ... The argument is an exclusive bound.
        //     So we will not go past the end of the array.
        int r = i + _random.Next(n - i);
        Card card = cards[r];

        // Swap cards
        cards[r] = cards[i];
        cards[i] = card;
      }
    }



    public void Draw(Player player)
    {
      Turn currentTurn = GetCurrentTurn();
      IList<Card> playerDeckZoneCards = player.FieldSide.DeckZone.Cards;

      if (playerDeckZoneCards.Any())
      {
        int topCardIndex = playerDeckZoneCards.Count - 1;
        Card topCard = playerDeckZoneCards[topCardIndex];
        playerDeckZoneCards.RemoveAt(topCardIndex);
        player.Hand.Add(topCard);
        topCard.SetFaceUp();
      }
      else
      {
        this.stateMachine.Fire(this.winnerDeclaredTrigger, player.Opponent);
      }

      // Record DrawCardEvent
      DrawCardEvent drawCardEvent = new DrawCardEvent(DateTime.Now, currentTurn, player);
      this.History.Events.Add(drawCardEvent);
    }

    // public void NormalSummonOrSet(Card card, MonsterZone monsterZone, bool set, Card tribute1)
    // {

    // }

    // public void NormalSummonOrSet(Card card, MonsterZone monsterZone, bool set, Card tribute1, Card tribute2)
    // {

    // }

    // public void NormalSummonOrSet(Card card, MonsterZone monsterZone, bool set, Card tribute1, Card tribute2, Card tribute3)
    // {

    // }

    public void NormalSummonOrSet(Card card, MonsterZone monsterZone, bool set)
    {
      Turn currentTurn = GetCurrentTurn();
      Player turnPlayer = currentTurn.Player;

      if (IsInMainPhase1() || IsInMainPhase2())
      {
        if (CanNormalSummonOrSet())
        {
          if (monsterZone.IsOccupied())
          {
            throw new IllegalMoveException($"{turnPlayer.User.Username} attempted to normal summon or set into an already occupied zone");
          }
          else
          {
            if (turnPlayer.Hand.Contains(card))
            {
              if (card.UserCard.BaseCard.IsMonster())
              {
                if (card.UserCard.BaseCard.Level > 4)
                {
                  throw new IllegalMoveException($"{turnPlayer.User.Username} attempted to normal summon or set a monster that requires tributes");
                }
                else
                {
                  turnPlayer.Hand.Remove(card);

                  if (set == true)
                  {
                    card.SetFaceDown();
                    card.SetHorizontal();
                    monsterZone.Set(card);
                  }
                  else
                  {
                    card.SetFaceUp();
                    card.SetVertical();
                    monsterZone.Set(card);
                    Monster summonedMonster = new Monster(card);
                    monsterZone.Monster = summonedMonster;
                  }

                  // Record NormalSummonOrSetEvent
                  NormalSummonOrSetEvent normalSummonOrSetEvent = new NormalSummonOrSetEvent(DateTime.Now, currentTurn, turnPlayer, card, monsterZone, set);
                  this.History.Events.Add(normalSummonOrSetEvent);
                }
              }
              else
              {
                throw new IllegalMoveException($"{turnPlayer.User.Username} attempted to normal summon or set a NonMonster");
              }
            }
            else
            {
              throw new IllegalMoveException($"{turnPlayer.User.Username} does not contain the specified card in their hand");
            }
          }
        }
        else
        {
          throw new IllegalMoveException($"{turnPlayer.User.Username} can no longer normal summon or set");
        }
      }
      else
      {
        throw new IllegalMoveException($"{turnPlayer.User.Username} attempted to normal summon or set outside of MainPhase1 or MainPhase2");
      }
    }

    public void AttackDirectly(Monster attackingMonster)
    {
      Turn currentTurn = GetCurrentTurn();
      Player turnPlayer = currentTurn.Player;

      if (this.History.HasMonsterAttacked(currentTurn, attackingMonster))
      {
        throw new IllegalMoveException($"{turnPlayer.User.Username} attempt to attack with monster '{attackingMonster.Name}' a second time");
      }
      else
      {
        this.stateMachine.Fire(this.attackDeclaredTrigger, attackingMonster, null);
      }
    }

    public void AttackMonster(Monster attackingMonster, Monster attackedMonster)
    {
      Turn currentTurn = GetCurrentTurn();
      Player turnPlayer = currentTurn.Player;

      if (this.History.HasMonsterAttacked(currentTurn, attackingMonster))
      {
        throw new IllegalMoveException($"{turnPlayer.User.Username} attempt to attack with monster '{attackingMonster.Name}' a second time");
      }
      else
      {
        this.stateMachine.Fire(this.attackDeclaredTrigger, attackingMonster, attackedMonster);
      }
    }

    public bool IsInDrawPhase()
    {
      return this.stateMachine.State == States.DrawPhase;
    }

    public bool IsInStandbyPhase()
    {
      return this.stateMachine.State == States.StandbyPhase;
    }

    public bool IsInMainPhase1()
    {
      return this.stateMachine.State == States.MainPhase1;
    }

    public bool IsInBattlePhase()
    {
      IList<States> battlePhaseStates = new List<States>() {
        States.BattlePhaseStartStep,
        States.BattlePhaseBattleStep,
        States.BattlePhaseDamageStep,
        States.BattlePhaseEndStep
      };
      return battlePhaseStates.Contains(this.stateMachine.State);
    }

    public bool IsInMainPhase2()
    {
      return this.stateMachine.State == States.MainPhase2;
    }

    public bool IsInEndPhase()
    {
      return this.stateMachine.State == States.EndPhase;
    }

    public bool HasTurnEnded()
    {
      return this.stateMachine.State == States.TurnEnd;
    }

    // Options
    public bool MustDiscardCardAtTurnEnd()
    {
      return GetTurnPlayer().Hand.Count > 6;
    }

    public void DiscardCard(Player player, Card card)
    {
      Turn currentTurn = GetCurrentTurn();
      
      if (player.Hand.Contains(card))
      {
        player.Hand.Remove(card);
        card.SetFaceUp();
        player.FieldSide.Graveyard.Cards.Add(card);

        // Record cardDiscardedEvent
        CardDiscardedEvent cardDiscardedEvent = new CardDiscardedEvent(DateTime.Now, currentTurn, player, card);
        this.History.Events.Add(cardDiscardedEvent);
      }
      else 
      {
        throw new IllegalMoveException($"{player.User.Username} attempted to discard '{card.UserCard.BaseCard.Name}' from hand but wasn't found");
      }
    }

    public bool CanNormalSummonOrSet()
    {
      return !this.History.HasNormalSummonedOrSet(GetCurrentTurn());
    }

    public bool CanAttack(Monster monster)
    {
      return !this.History.HasMonsterAttacked(GetCurrentTurn(), monster);
    }

    // public bool CanSpecialSummon()
    // {
    //     return false;
    // }

    // public bool CanSwitchToAttack()
    // {
    //     return false;
    // }

    // public bool CanSwitchToDefense()
    // {
    //     return false;
    // }

    // public bool CanActivate()
    // {
    //     return false;
    // }

    // public bool CanAttack()
    // {
    //     return false;
    // }



    public int NormalSummonTributeCount(Card card)
    {
      if (card.UserCard.BaseCard.IsMonster())
      {
        if (card.UserCard.BaseCard.Level < 5)
        {
          return 0;
        }
        else if (card.UserCard.BaseCard.Level < 7)
        {
          return 1;
        }
        else
        {
          return 2;
        }
      }
      else
      {
        return 0;
      }
    }

    private void OnPreparing()
    {
      foreach (Player player in this.Players)
      {
        // Set Player LP
        player.LifePoints = 8000;

        // Set the FieldSide
        player.FieldSide = new FieldSide();

        // Populate DeckZone
        foreach (UserCard userCard in player.MainDeck.UserCards)
        {
          Card card = new Card(userCard, player);
          card.Orientation = Card.Orientations.FaceDown;
          player.FieldSide.DeckZone.Cards.Add(card);
        }

        // Populate ExtraDeckZone
        foreach (UserCard userCard in player.ExtraDeck.UserCards)
        {
          Card card = new Card(userCard, player);
          card.Orientation = Card.Orientations.FaceDown;
          player.FieldSide.ExtraDeckZone.Cards.Add(card);
        }

        // Shuffle deck
        ShuffleDeck(player);

        // Draw Cards
        for (int i = 0; i < 5; ++i)
        {
          Draw(player);
        }
      }

      // Set to Prepared state
      this.stateMachine.Fire(Triggers.Prepared);
    }

    private void OnTurnStart(Player turnPlayer)
    {
      // Build new turn
      int turnIndex = this.Turns.Count();
      Turn turn = new Turn(turnIndex, turnPlayer);
      this.Turns.Add(turn);

      // Set to TurnStarted state
      this.stateMachine.Fire(Triggers.TurnStarted);
    }

    private void OnDrawPhase()
    {
      Player turnPlayer = GetTurnPlayer();
      Draw(turnPlayer);

      // Set to StandbyPhase state
      this.stateMachine.Fire(Triggers.DrawPhaseEnded);
    }

    private void OnStandbyPhase()
    {
      this.stateMachine.Fire(Triggers.StandbyPhasePhaseEnded);
    }

    private void OnMainPhase1()
    {

    }

    private void OnBattlePhaseStartStep()
    {
      this.stateMachine.Fire(Triggers.BattleStepEntered);
    }

    private void OnBattlePhaseBattleStep()
    {

    }

    private void OnBattlePhaseDamageStep(Monster attackingMonster, Monster targetMonster)
    {
      Turn currentTurn = GetCurrentTurn();
      Player turnPlayer = currentTurn.Player;

      if (targetMonster == null)
      {
        Console.WriteLine("Attacking directly");
        InflictDamage(turnPlayer.Opponent, attackingMonster.Attack);
      }
      else if (targetMonster.IsInAttackPosition())
      {
        Console.WriteLine("Attacking monster in attack position");

        if (attackingMonster.Attack > targetMonster.Attack)
        {
          int damageAmount = attackingMonster.Attack - targetMonster.Attack;
          InflictDamage(turnPlayer.Opponent, damageAmount);
          Destroy(targetMonster.Card);
        }
        else if (attackingMonster.Attack < targetMonster.Attack)
        {
          int damageAmount = targetMonster.Attack - attackingMonster.Attack;
          InflictDamage(turnPlayer, damageAmount);
          Destroy(attackingMonster.Card);
        }
        else if (attackingMonster.Attack == targetMonster.Attack && attackingMonster.Attack > 0)
        {
          Destroy(attackingMonster.Card);
          Destroy(targetMonster.Card);
        }
      }
      else if (targetMonster.IsInDefensePosition())
      {
        Console.WriteLine("Attacking monster in defense position");

        if (attackingMonster.Attack > targetMonster.Defense)
        {
          Destroy(targetMonster.Card);
        }
        else if (attackingMonster.Attack < targetMonster.Defense)
        {
          int damageAmount = targetMonster.Defense - attackingMonster.Attack;
          InflictDamage(turnPlayer, damageAmount);
        }
      }

      // Record AttackEvent
      AttackEvent attackEvent = new AttackEvent(DateTime.Now, currentTurn, attackingMonster, targetMonster, turnPlayer, turnPlayer.Opponent);
      this.History.Events.Add(attackEvent);

      this.stateMachine.Fire(Triggers.AttackResolved);
    }

    private void OnBattlePhaseEndStep()
    {

    }

    private void OnMainPhase2()
    {

    }

    private void OnEndPhase()
    {
      
    }

    private void OnTurnEnd()
    {
      Player turnPlayer = GetTurnPlayer();
      // Trigger TurnStart for the turnPlayer opponent
      this.stateMachine.Fire(this.newTurnTrigger, turnPlayer.Opponent);
    }

    private void OnSettled(Player wonPlayer)
    {
      this.Winner = wonPlayer;
    }

    private void Destroy(Card card)
    {
      Player owner = card.Owner;

      foreach (Zone zone in owner.FieldSide.MonsterZones)
      {
        if (zone.IsOccupied() && zone.Cards.Contains(card))
        {
          zone.Cards.Remove(card);

          Type zoneType = zone.GetType();
          if (zoneType == typeof(MonsterZone))
          {
            MonsterZone monsterZone = (MonsterZone)zone;
            monsterZone.Monster = null;
          }

          break;
        }
      }

      owner.FieldSide.Graveyard.Cards.Add(card);
    }

    private void InflictDamage(Player player, int amount, int opponentAmount)
    {
      Turn currentTurn = GetCurrentTurn();

      if (amount > 0)
      {
        player.LifePoints -= amount;
        // Record DamageTakenEvent
        DamageTakenEvent damageTakenEvent = new DamageTakenEvent(DateTime.Now, currentTurn, player, amount);
        this.History.Events.Add(damageTakenEvent);

        player.Opponent.LifePoints -= opponentAmount;

        // Check if any player has won
        EvaluatePlayerWonState();
      }
      else
      {
        throw new IllegalMoveException($"Attempted to inflict non-positive damage");
      }
    }

    private void InflictDamage(Player player, int amount)
    {
      Turn currentTurn = GetCurrentTurn();

      if (amount > 0)
      {
        player.LifePoints -= amount;
        // Record DamageTakenEvent
        DamageTakenEvent damageTakenEvent = new DamageTakenEvent(DateTime.Now, currentTurn, player, amount);
        this.History.Events.Add(damageTakenEvent);

        // Check if any player has won
        EvaluatePlayerWonState();
      }
      else
      {
        throw new IllegalMoveException($"Attempted to inflict non-positive damage to {player.User.Username}");
      }
    }

    private void GainLifePoints(Player player, int amount)
    {
      Turn currentTurn = GetCurrentTurn();

      if (amount > 0)
      {
        player.LifePoints += amount;
        // Record DamageTakenEvent
        LifePointsGainEvent lifePointsGainEvent = new LifePointsGainEvent(DateTime.Now, currentTurn, player, amount);
        this.History.Events.Add(lifePointsGainEvent);
      }
      else
      {
        throw new IllegalMoveException($"Attempted to gain non-positive life points for {player.User.Username}");
      }
    }

    private void EvaluatePlayerWonState()
    {
      Player turnPlayer = GetTurnPlayer();

      if (turnPlayer.Opponent.LifePoints <= 0 && turnPlayer.LifePoints > 0)
      {
        this.stateMachine.Fire(this.winnerDeclaredTrigger, turnPlayer);
      }
      else if (turnPlayer.Opponent.LifePoints > 0 && turnPlayer.LifePoints <= 0)
      {
        this.stateMachine.Fire(this.winnerDeclaredTrigger, turnPlayer.Opponent);
      }
      else if (turnPlayer.Opponent.LifePoints <= 0 && turnPlayer.LifePoints <= 0)
      {
        this.stateMachine.Fire(this.winnerDeclaredTrigger, null);
      }
    }
  }
}

// http://www.webgraphviz.com/
// http://i.imgur.com/6w2lc.jpg
// https://img.yugioh-card.com/en/rulebook/SD_RuleBook_EN_10.pdf
// digraph {
//  New ->  Preparing [label="Confirmed"];
//  Preparing ->  Ready [label="Prepared"];
//  Ready -> TurnStart [label="NewTurn"];
//  TurnStart -> DrawPhase [label="TurnStarted"];
//  DrawPhase -> StandbyPhase [label="DrawPhaseEnded (if card drawn)"];
//  StandbyPhase -> MainPhase1 [label="StandbyPhaseEnded"];
//  MainPhase1 -> BattlePhaseStartStep [label="BattlePhaseEntered (unless first turn)"];
//  MainPhase1 -> EndPhase [label="EndPhaseEntered"];
//  BattlePhaseStartStep -> BattlePhaseBattleStep [label="BattleStepEntered"];
//  BattlePhaseBattleStep -> BattlePhaseDamageStep [label="AttackDeclared"];
//  BattlePhaseDamageStep -> BattlePhaseBattleStep [label="AttackResolved"];
//  BattlePhaseBattleStep -> BattlePhaseEndStep [label="BattlePhaseEnded"];
//  BattlePhaseEndStep -> MainPhase2 [label="MainPhase2Entered"];
//  MainPhase2 -> EndPhase [label="EndPhaseEntered"];
//  EndPhase -> TurnEnd [label="TurnEnded"];
//  TurnEnd -> TurnStart [label="NewTurn (unless > 6 cards in hand)"]
// node [shape=box];
//  Preparing -> "Prepare()" [label="On Entry" style=dotted];
// } 