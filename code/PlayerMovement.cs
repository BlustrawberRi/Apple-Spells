using Godot;
using System;

public partial class PlayerMovement : CharacterBody2D
{
	[Export(PropertyHint.Range, "10,250")] 
	public float walkingSpeed = 70.0f;
	[Export(PropertyHint.Range, "10,250")]
    public float runningSpeed = 120.0f;
	[Export(PropertyHint.Range, "0,10")]
	public float slideValue = 0f;
	
	public bool IsIdle {
		private set
		{
			_isIdle = value;
			AnimTree.Set("parameters/conditions/is_moving", !_isIdle);
			AnimTree.Set("parameters/conditions/idle", _isIdle);
		}
		get
		{
			return _isIdle;
		}	
	}
    private bool _isIdle;

    [Export] public AnimationTree AnimTree {get;set;}

	public override void _Ready()
	{
	}
    public override void _PhysicsProcess(double delta)
	{
		
		// Get the input direction and handle the movement/deceleration.
		// todo As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		bool moved = direction != Vector2.Zero;

		if (moved)
        {
            Move(direction);
            // GD.Print(direction);
            SetSpeed();
        }
        else if (Velocity != Vector2.Zero)
		{
			Slide();
			IsIdle = true;
		} else
		{
			IsIdle = true;
		}
		
		
		MoveAndSlide();
	}

    private void SetSpeed()
    {
        if (Input.IsActionPressed("Run"))
		{
            Velocity = Velocity * runningSpeed;
			AnimTree.Set("parameters/Walk2/WalkSpeed/scale", 1.0f);
		}
        else
        {    
			Velocity = Velocity * walkingSpeed;
			AnimTree.Set("parameters/Walk2/WalkSpeed/scale", 0.5f);
		}
    }

    public void Move(Vector2 direction)
	{
		Vector2 velocity;
		velocity.X = direction.X ;
		velocity.Y = direction.Y ;

		//against key speed glitch
		if (velocity.Length() > 1.0f)
			velocity.Normalized();

		Velocity = velocity;

		velocity.Y = velocity.Y*-1; //todo animation is flipped
		
		AnimTree.Set("parameters/Idle/blend_position", velocity);
		AnimTree.Set("parameters/Walk2/WalkAnimation/blend_position", velocity);
		IsIdle = false;
		//animTree.
	}

	/// <summary>
	/// Slide after Movement. 0 is no slide and the higher the slideValue the farther the slide.
	/// </summary>
	private void Slide ()
	{
		Vector2 velocity;
		velocity.X = (float)Mathf.MoveToward(Velocity.X, 0, walkingSpeed/(slideValue*slideValue+1));
		velocity.Y = (float)Mathf.MoveToward(Velocity.Y, 0, walkingSpeed/(slideValue*slideValue+1));
		Velocity = velocity;
	}

}
