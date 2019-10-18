using System.Collections.Generic;
using System.Linq;

using Stateless;

namespace Yugioh.Engine.Models
{
  public class Duel
  {
    public enum States : int { New, Preparing, Ready, Player1Turn, Player2Turn, Finished, Cancelled }
    public Player Winner { get; set; }
    public Player Player1 { get; set; }
    public Player Player2 { get; set; }
    public IList<Turn> Turns { get; set; }
    private enum Triggers { Prepare, Prepared, Start, EndTurn, Finish, Cancel }
    private readonly StateMachine<States, Triggers> stateMachine;
    private StateMachine<States, Triggers>.TriggerWithParameters<bool> prepareTrigger;
    private StateMachine<States, Triggers>.TriggerWithParameters<Player> finishTrigger;

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
        .Permit(Triggers.Prepare, States.Preparing)
        .Permit(Triggers.Cancel, States.Cancelled);

      // State - Preparing
      prepareTrigger = this.stateMachine.SetTriggerParameters<bool>(Triggers.Prepare);

      this.stateMachine.Configure(States.Preparing)
        .OnEntryFrom(prepareTrigger, (player1HasFirstMove) =>
        {
            OnPrepare(player1HasFirstMove);
        })
        .Permit(Triggers.Prepared, States.Ready)
        .Permit(Triggers.Finish, States.Finished)
        .Permit(Triggers.Cancel, States.Cancelled);

      // State - Player1Turn
      this.stateMachine.Configure(States.Player1Turn)
        .OnEntry(() =>
        {
            OnPlayerTurn(this.Player1);
        })
        .Permit(Triggers.EndTurn, States.Player2Turn)
        .Permit(Triggers.Finish, States.Finished)
        .Permit(Triggers.Cancel, States.Cancelled);

      // State - Player2Turn
      this.stateMachine.Configure(States.Player2Turn)
        .OnEntry(() =>
        {
            OnPlayerTurn(this.Player2);
        })
        .Permit(Triggers.EndTurn, States.Player1Turn)
        .Permit(Triggers.Finish, States.Finished)
        .Permit(Triggers.Cancel, States.Cancelled);

      // State - Finished
      this.stateMachine.Configure(States.Finished)
        .OnEntryFrom(finishTrigger, (wonPlayer) =>
        {
            OnFinish(wonPlayer);
        });
    }

    public States GetState()
    {
      return this.stateMachine.State;
    }

    public Turn GetCurrentTurn()
    {
      return this.Turns.Last();
    }

    public void Prepare(bool player1HasFirstMove)
    {
      this.stateMachine.Fire(this.prepareTrigger, player1HasFirstMove); 
    }

    public void Start()
    {
      this.stateMachine.Fire(Triggers.Start); 
    }

    public void EndTurn()
    {
      this.stateMachine.Fire(Triggers.EndTurn);   
    }

    private void OnPrepare(bool player1HasFirstMove)
    {
      // Set state transition to first player
      if (player1HasFirstMove)
      {
        this.stateMachine.Configure(States.Ready)
          .Permit(Triggers.Start, States.Player1Turn)
          .Permit(Triggers.Cancel, States.Cancelled);
      }
      else 
      {
        this.stateMachine.Configure(States.Ready)
          .Permit(Triggers.Start, States.Player2Turn)
          .Permit(Triggers.Cancel, States.Cancelled);    
      }

      // Set the player opponents
      this.Player1.Opponent = this.Player2;
      this.Player2.Opponent = this.Player1;

      // Shuffle both user decks
      this.Player1.Shuffle(this.Player2.MainDeck);
      this.Player2.Shuffle(this.Player1.MainDeck);

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
        this.Player1.Draw(this.Player1.FieldSide.MainDeck);
        this.Player2.Draw(this.Player2.FieldSide.MainDeck);
      }  

      // Set to Prepared state
      this.stateMachine.Fire(Triggers.Prepared); 
    }

    private void OnPlayerTurn(Player player)
    {
      // Build new turn
      int turnIndex = this.Turns.Count();
      Turn turn = new Turn(turnIndex, player);
      this.Turns.Add(turn);  
    }

    private void OnFinish(Player wonPlayer)
    {
      this.Winner = wonPlayer;
    }


    // public void AdvancePhase()
    // {
    //   try
    //   {
    //     Player turnPlayer = GetTurnPlayer();
    //     turnPlayer.Draw(turnPlayer.FieldSide.MainDeck);
    //   }
    //   catch (RunOutOfCardsException runOutOfCardsException)
    //   {
    //     Lose(runOutOfCardsException.Player);
    //   }
    //   finally
    //   {
    //     ++this.Turn;
    //   }
    // }

    // public Player GetTurnPlayer()
    // {
    //   return (this.Turn % 2) + this.PlayerSequenceShift == 0 ? this.Player1 : this.Player2;
    // }
  }
}
