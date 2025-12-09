using Godot;
using System;
using Godot.Collections;

[GlobalClass][Tool]
public partial class ItemInstance : Resource
{
	[Export]
	public Texture2D ItemTexture;

	[Export(PropertyHint.MultilineText)]
	public String ItemDescription;

	[Export]
	public Array<ItemComponent> Components;

}