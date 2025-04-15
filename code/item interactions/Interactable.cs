using Godot;
using System;
using System.Reflection.Metadata.Ecma335;

[Tool]
/// <summary>
/// An item in the game the Player can interact with in different ways.
/// </summary>
public partial class Interactable : StaticBody2D
{

	[Export]
	public Texture ItemTexture 
	{
		get
		{
			return _itemTexture;
		}
		set
		{
			_itemTexture = value;
			TextureContainer?.Set("texture", value);
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
	private Texture _itemTexture;

	private Sprite2D TextureContainer;
	private AnimationPlayer AnimationPlayer;
	
    public override void _Ready()
    {
		TextureContainer = (Sprite2D)GetNode<Sprite2D>("Texture");
		TextureContainer?.Set("texture", ItemTexture);

		AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		//ItemTexture = ItemTexture; //initialize the set Texture
    }
    public override void _Process(double delta)
    {
        
    }

// todo: put this in the interaction Manager
	public void Highlight(bool on) 
	{
		if(on)		
			AnimationPlayer.Play(HighlightAnimation, -1, 2f);
		else
			AnimationPlayer.Play(AnimationPlayer.AssignedAnimation, -1, -2f, true);	
	}
	
}
