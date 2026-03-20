using Godot;
using System;

[GlobalClass]
public abstract partial class ItemComponent : Resource
{
    /// <summary>
    /// The components reaction to an interaction.
    /// </summary>
    /// <param name="interactionSource">The item that was used to cause the interaction.</param>
    /// <param name="item">The item that holds this component.</param>
    public abstract void React(Interactable interactionSource, Interactable item);

    /// <summary>
    /// Checks if all the conditions to React are met.
    /// </summary>
    /// <param name="interactionSource">The item that was used to cause the interaction.</param>
    /// <returns>Wether this component can manage this interaction with the given source.</returns>
    public abstract bool CanInteract(Interactable interactionSource);
}