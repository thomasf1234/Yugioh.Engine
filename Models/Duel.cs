using System;
using System.Collections.Generic;
using System.Linq;

using Stateless;
using Yugioh.Engine.Entities;
using Yugioh.Engine.Exceptions;

namespace Yugioh.Engine.Models
{
  public class Duel
  {
    public enum States : int { New, Preparing, Ready, TurnStart, DrawPhase, StandbyPhase, MainPhase1, BattlePhaseStartStep, BattlePhaseBattleStep, BattlePhaseDamageStep, BattlePhaseEndStep, MainPhase2, EndPhase, Settled, Cancelled }
    public Player Winner { get; set; }
    public Player Player1 { get; set; }
    public Player Player2 { get; set; }
    public IList<Turn> Turns { get; set; }
    private enum Triggers { Confirmed, Prepared, TurnStarted, TurnInitialised, DrawPhaseEnded, StandbyPhasePhaseEnded, BattlePhaseEntered, BattleStepEntered, AttackDeclared, AttackResolved, BattlePhaseEnded, MainPhase2Entered, TurnEnded, WinnerDeclared, Cancel }
    private readonly StateMachine<States, Triggers> stateMachine;
    private StateMachine<States, Triggers>.TriggerWithParameters<Player> startTurnTrigger;
    private StateMachine<States, Triggers>.TriggerWithParameters<UserCard, UserCard> attackDeclaredTrigger;

    private StateMachine<States, Triggers>.TriggerWithParameters<Player> winnerDeclaredTrigger;

    public Duel(Player _player1, Player _player2)
    {
      this.Winner = null;
      this.Turns = new List<Turn>();

      this.Player1 = _player1;
      this.Player2 = _player2;

      this.Player1.FieldSide = new FieldSide();
      this.Player2.FieldSide = new FieldSide();

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
          .Permit(Triggers.TurnStarted, States.TurnStart)
          .Permit(Triggers.Cancel, States.Cancelled);

      // State - TurnStart
      this.startTurnTrigger = this.stateMachine.SetTriggerParameters<Player>(Triggers.TurnStarted);

      this.stateMachine.Configure(States.TurnStart)
        .OnEntryFrom(startTurnTrigger, (turnPlayer) =>
        {
          OnTurnStart(turnPlayer);
        })
        .Permit(Triggers.TurnInitialised, States.DrawPhase)
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
        .Permit(Triggers.TurnEnded, States.EndPhase)
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
      this.attackDeclaredTrigger = this.stateMachine.SetTriggerParameters<UserCard, UserCard>(Triggers.AttackDeclared);
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
        .OnEntryFrom(attackDeclaredTrigger, (attackingUserCard, targetUserCard) =>
        {
          OnBattlePhaseDamageStep(attackingUserCard, targetUserCard);
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
        .Permit(Triggers.TurnEnded, States.EndPhase)
        .Permit(Triggers.WinnerDeclared, States.Settled)
        .Permit(Triggers.Cancel, States.Cancelled);

      // State - EndPhase
      this.stateMachine.Configure(States.EndPhase)
        .OnEntry(() =>
        {
          OnEndPhase();
        })
        .PermitIf(Triggers.TurnStarted, States.TurnStart, () => GetCurrentTurn().Player.Hand.Count < 7)
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
      return this.Turns.Last();
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
      this.stateMachine.Fire(this.startTurnTrigger, firstPlayer);
    }

    public void EnterBattlePhase()
    {
      this.stateMachine.Fire(Triggers.BattlePhaseEntered);
    }

    public void EndTurn()
    {
      this.stateMachine.Fire(Triggers.TurnEnded);
    }

    public void EndDrawPhase()
    {
      this.stateMachine.Fire(Triggers.DrawPhaseEnded);
    }

    public void Shuffle(Deck deck)
    {
      Random _random = new Random();
      int n = deck.UserCards.Count;
      for (int i = 0; i < n; i++)
      {
        // Use Next on random instance with an argument.
        // ... The argument is an exclusive bound.
        //     So we will not go past the end of the array.
        int r = i + _random.Next(n - i);
        UserCard userCard = deck.UserCards[r];

        // Swap cards
        deck.UserCards[r] = deck.UserCards[i];
        deck.UserCards[i] = userCard;
      }
    }

    public void Draw(Player player)
    {
      IList<UserCard> playerDeckUserCards = player.FieldSide.MainDeck.UserCards;

      if (playerDeckUserCards.Count == 0)
      {
        this.stateMachine.Fire(this.winnerDeclaredTrigger, player.Opponent);
      }
      else
      {
        player.Hand.Add(playerDeckUserCards[0]);
        playerDeckUserCards.RemoveAt(0);
      }
    }

    public void NormalSummon(UserCard userCard, int zone)
    {
      Turn currentTurn = GetCurrentTurn();
      Player turnPlayer = currentTurn.Player;

      if (currentTurn.ReachedSummonLimit())
      {
        Console.WriteLine("Player has reached the summon limit this turn");
      }
      else if (turnPlayer.FieldSide.MonsterZones[zone] != null)
      {
        Console.WriteLine("Monster zone is currently occupied");
      }
      else if (turnPlayer.Hand.Contains(userCard))
      {
        if (userCard.Card.IsMonster())
        {
          if (userCard.Card.Level > 4)
          {
            Console.WriteLine("Monster requires tributes to normal summon");
          }
          else
          {
            turnPlayer.Hand.Remove(userCard);
            turnPlayer.FieldSide.MonsterZones[zone] = userCard;
            currentTurn.SummonCount += 1;
          }
        }
        else
        {
          Console.WriteLine("NonMonsters cannot be normal summoned");
        }
      }
      else
      {
        Console.WriteLine("Player does not contain the specified card in their hand");
      }
    }

    public void AttackDirectly(UserCard attackingMonster)
    {
      this.stateMachine.Fire(this.attackDeclaredTrigger, attackingMonster, null);
    }

    public void AttackMonster(UserCard attackingMonster, UserCard targetMonster)
    {
      this.stateMachine.Fire(this.attackDeclaredTrigger, attackingMonster, targetMonster);
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



    // Options
    // public bool CanNormalSummon(DuelCard duelCard)
    // {
    //     return false;
    // }

    // public bool CanNormalSet()
    // {
    //     return false;
    // }

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

    public int NormalSummonTributeCount(DuelCard duelCard)
    {
      if (duelCard.UserCard.Card.IsMonster())
      {
        if (duelCard.UserCard.Card.Level < 5)
        {
          return 0;
        }
        else if (duelCard.UserCard.Card.Level < 7)
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
      // Set the player opponents
      this.Player1.Opponent = this.Player2;
      this.Player2.Opponent = this.Player1;

      // Shuffle both user decks
      Shuffle(this.Player2.MainDeck);
      Shuffle(this.Player1.MainDeck);

      // Set Player LP
      this.Player1.Lp = 8000;
      this.Player2.Lp = 8000;

      // Populate field for Players
      this.Player1.FieldSide.MainDeck = this.Player1.MainDeck;
      this.Player1.FieldSide.ExtraDeck = this.Player1.ExtraDeck;
      this.Player2.FieldSide.MainDeck = this.Player2.MainDeck;
      this.Player2.FieldSide.ExtraDeck = this.Player2.ExtraDeck;

      // Draw Cards
      for (int i = 0; i < 5; ++i)
      {
        Draw(this.Player1);
        Draw(this.Player2);
      }

      // Set to Prepared state
      this.stateMachine.Fire(Triggers.Prepared);
    }

    private void OnTurnStart(Player turnPlayer)
    {
      // Build new turn
      int turnIndex = this.Turns.Count();
      int summonLimit = 1;
      Turn turn = new Turn(turnIndex, turnPlayer, summonLimit);
      this.Turns.Add(turn);

      // Set to TurnInitialised state
      this.stateMachine.Fire(Triggers.TurnInitialised);
    }

    private void OnDrawPhase()
    {
      Player turnPlayer = GetTurnPlayer();

      if (GetCurrentTurn().IsFirstTurn())
      {
        Console.WriteLine("Cannot draw on first turn");
      }
      else
      {
        Draw(turnPlayer);
      }

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

    private void OnBattlePhaseDamageStep(UserCard attackingUserCard, UserCard targetUserCard)
    {
      Turn currentTurn = GetCurrentTurn();
      Player turnPlayer = currentTurn.Player;

      if (targetUserCard == null)
      {
        Console.WriteLine("Attacking directly");
        turnPlayer.Opponent.Lp -= Convert.ToInt32(attackingUserCard.Card.Attack);

        if (turnPlayer.Opponent.Lp <= 0)
        {
          this.stateMachine.Fire(this.winnerDeclaredTrigger, turnPlayer);
        }
      }
      else 
      {
        Console.WriteLine("Attacking monster");
      }

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
      Player turnPlayer = GetTurnPlayer();
      // Trigger TurnStart for the turnPlayer opponent
      this.stateMachine.Fire(this.startTurnTrigger, turnPlayer.Opponent);
    }

    private void OnSettled(Player wonPlayer)
    {
      this.Winner = wonPlayer;
    }
  }
}

// http://www.webgraphviz.com/
// http://i.imgur.com/6w2lc.jpg
// https://img.yugioh-card.com/en/rulebook/SD_RuleBook_EN_10.pdf
// digraph {
//  New ->  Preparing [label="Confirmed"];
//  Preparing ->  Ready [label="Prepared"];
//  Ready -> TurnStart [label="TurnStarted"];
//  TurnStart -> DrawPhase [label="TurnInitialised"];
//  DrawPhase -> StandbyPhase [label="DrawPhaseEnded (if card drawn)"];
//  StandbyPhase -> MainPhase1 [label="StandbyPhaseEnded"];
//  MainPhase1 -> BattlePhaseStartStep [label="BattlePhaseEntered (unless first turn)"];
//  MainPhase1 -> EndPhase [label="TurnEnded"];
//  BattlePhaseStartStep -> BattlePhaseBattleStep [label="BattleStepEntered"];
//  BattlePhaseBattleStep -> BattlePhaseDamageStep [label="AttackDeclared"];
//  BattlePhaseDamageStep -> BattlePhaseBattleStep [label="AttackResolved"];
//  BattlePhaseBattleStep -> BattlePhaseEndStep [label="BattlePhaseEnded"];
//  BattlePhaseEndStep -> MainPhase2 [label="MainPhase2Entered"];
//  MainPhase2 -> EndPhase [label="TurnEnded"];
//  EndPhase -> TurnStart [label="TurnStarted"];
// node [shape=box];
//  Preparing -> "Prepare()" [label="On Entry" style=dotted];
// } 