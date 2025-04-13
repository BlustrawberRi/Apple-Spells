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
	public string TexturePath 
	{
		get 
		{
			return ItemTexture?.ResourcePath;
		}
	}
	private Texture _itemTexture;
	[Export(PropertyHint.MultilineText)]
	public String ItemDescription;

	private Sprite2D TextureContainer;
	
    public override void _Ready()
    {
		TextureContainer = (Sprite2D)GetNode<Sprite2D>("Texture");
		TextureContainer?.Set("texture", ItemTexture);
		//ItemTexture = ItemTexture; //initialize the set Texture
    }
    public override void _Process(double delta)
    {
        
    }
	
}
