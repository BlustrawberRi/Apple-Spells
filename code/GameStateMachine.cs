using Godot;
using System;

public partial class GameStateMachine : Node
{
    public enum State
    {
        MAIN_MENU = 0,
        GAME_PLAY = 1,
		DIALOGUE = 2
    }

    [Signal]
    public delegate void DialogueOpenEventHandler(Vector2 velocity);

    [Export]
    public static State CurrentState { get; private set; }
    
	public void Pause() 
	{
        this.GetTree().Paused = true;
    }
	public void Resume ()
	{
        this.GetTree().Paused = false;
    }

	public void OnDialogueOpen()
	{
        CurrentState = State.DIALOGUE;
        EmitSignal(SignalName.DialogueOpen);
    }

}
