using Godot;
using System;
using Godot.Collections;

[GlobalClass][Tool]
public partial class ItemInstance : Resource
{

	[Export]
	public String DisplayName
    {
		get
		{
			if (displayName == null)
			{
				return this.ResourceName;
			}
			else
			{
				return displayName;
			}
		}
		set => displayName = value;
    }
	private String displayName;

	[Export]
	public Texture2D Texture;

	[Export(PropertyHint.MultilineText)]
	public String Description;


	[Export]
	public Array<ItemComponent> Components;

}