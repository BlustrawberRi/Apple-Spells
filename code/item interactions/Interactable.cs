using Godot;
using System;
using System.ComponentModel;

[Tool]
/// <summary>
/// An item in the game the Player can interact with in different ways.
/// </summary>
public partial class Interactable : StaticBody2D
{
	[Export]
	public ItemInstance ItemInstance;

	[Export]
	public String HighlightAnimation = "interactable_highlight"; //todo: Global variable

	public string TexturePath 
	{
		get 
		{
			return ItemInstance?.Texture?.ResourcePath;
		}
	}

	private Sprite2D TextureContainer;
	private AnimationPlayer AnimationPlayer;
	
	public override void _Ready()
	{
		this.ProcessMode = ProcessModeEnum.Pausable;
		// GetComponentsInChildren();
		if (ItemInstance == null)
        {
			Disable();
        }

		TextureContainer = (Sprite2D)GetNode<Sprite2D>("Texture");
		TextureContainer?.Set("texture", ItemInstance?.Texture);

		AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		//ItemTexture = ItemTexture; //initialize the set Texture
	}

	public void Disable()
	{
		this.Visible = false;
		this.ProcessMode = ProcessModeEnum.Disabled;
	}
	
	public void Enable()
    {
		this.Visible = true;
		this.ProcessMode = ProcessModeEnum.Pausable;
    }

    // todo: make Interactable an Area2D
    //public override onBodyEntered
    public void React(Interactable interactionSource)
	{
		if (ItemInstance == null || ItemInstance.Components.Count == 0) return;

		foreach(var c in ItemInstance.Components)
        {
            if (c.CanInteract(interactionSource)) 
				c.React(interactionSource, this);
        }
    }

	// todo: put this in the interaction Manager
	public void Highlight(bool on)
	{
		if (on)
			AnimationPlayer.Play(HighlightAnimation, -1, 2f);
		else
			AnimationPlayer.Play(AnimationPlayer.AssignedAnimation, -1, -2f, true);
	}
}
