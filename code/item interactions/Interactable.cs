using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

[Tool]
/// <summary>
/// An item in the game the Player can interact with in different ways.
/// </summary>
public partial class Interactable : StaticBody2D
{
    //[Export] public Texture2D Text;

    [Export]
	public Texture2D ItemTexture 
	{
		get
		{
			return _itemTexture;
		}
		set
		{
			_itemTexture = value;
			if( TextureContainer != null)
				TextureContainer.Texture = value;
			//TextureContainer?.Set("texture", value);
		}
	}
	[Export(PropertyHint.MultilineText)]
	public String ItemDescription;
	[Export]
	public String HighlightAnimation = "interactable_highlight"; //todo: Global variable

	public string TexturePath 
	{
		get 
		{
			return ItemTexture?.ResourcePath;
		}
	}
	private Texture2D _itemTexture;

	private Sprite2D TextureContainer;
	private AnimationPlayer AnimationPlayer;
	public List<ItemComponent> Components {
        get; private set;
    }

	public override void _Ready()
	{
		this.ProcessMode = ProcessModeEnum.Pausable;
		GetComponentsInChildren();

		TextureContainer = (Sprite2D)GetNode<Sprite2D>("Texture");
		TextureContainer?.Set("texture", ItemTexture);

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
        Components.ForEach(c =>
        {
            if (c.CanInteract(interactionSource)) 
				c.React(interactionSource, this);
        });
    }


// todo: put this in the interaction Manager
	public void Highlight(bool on) 
	{
		if(on)		
			AnimationPlayer.Play(HighlightAnimation, -1, 2f);
		else
			AnimationPlayer.Play(AnimationPlayer.AssignedAnimation, -1, -2f, true);	
	}
	
    private void GetComponentsInChildren()
    {
        Components = new();
        foreach (var child in this.GetChildren())
        {
            var type = child.GetType();
            if (child.GetType().IsSubclassOf(typeof(ItemComponent)))
            {
                Components.Add(child as ItemComponent);
            }
        }
    }
}
