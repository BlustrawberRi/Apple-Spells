using Godot;
using System;

public partial class HoldableComponent : Node2D
{
    [Export]
    public bool IsStackable;

    [Export(hint: PropertyHint.Range, hintString: "1,999")]

    public int MaxStackCount = 1;
}
