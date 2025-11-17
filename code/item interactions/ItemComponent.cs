using Godot;
using System;

public abstract partial class ItemComponent : Node
{
    public abstract void React(Interactable item);
}