using Godot;
using System;

public abstract partial class ItemComponent : Node
{
    /// <summary>
    /// The components reaction to an interaction.
    /// </summary>
    /// <param name="interactionSource">The item that was used to cause the interaction.</param>
    /// <param name="item">The item that holds this component.</param>
    public abstract void React(Interactable interactionSource, Interactable item);

    /// <summary>
    /// Checks if the component should react with React().
    /// </summary>
    /// <param name="interactionSource">The item that was used to cause the interaction.</param>
    /// <returns>If this component can manage this interaction with the given source.</returns>
    public abstract bool CanInteract(Interactable interactionSource);
}