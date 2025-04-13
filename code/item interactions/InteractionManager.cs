using Godot;
using System;

public partial class InteractionManager : Area2D
{

    [Export]
    private Node2D ActiveItem;
    //[Signal]
    //public delegate void ItemCollisionEventHandler(StaticBody2D item);
    [Signal]
    public delegate void ItemInteractionEventHandler(Interactable item);

    private String ItemText;
    private Texture ItemTexture = null;
    private bool listenToInput = false;

    public override void _Ready() 
    {

        //BodyEntered += OnBodyEntered; // I dont need this if I connected it in editor
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!listenToInput) return;
        
        if (@event.IsActionReleased("Interact")){
            EmitSignal(SignalName.ItemInteraction, ActiveItem);
            this.GetViewport().SetInputAsHandled();
        }
    }

    private void OnBodyEntered(Node2D body)
    {
        ActiveItem = body;

        if (body is Plant) 
        {
            GD.PrintRich("Collided with: [img]"+ (ActiveItem as Plant).PlantSprite?.Texture?.ResourcePath +"[/img] "+ (body as Plant).PlantType + " | Level " + (body as Plant).CurrentGrowthPhase); 
        }

        if (body is Interactable)
        {
            //EmitSignal(SignalName.ItemCollision, body as StaticBody2D);
            //if (body is not Interactable) return;

            //GetInteractableFeatures((StaticBody2D)body);
            listenToInput = true;
            GD.PrintRich("Collided with: [img]"+ (ActiveItem as Interactable).ItemTexture?.ResourcePath+"[/img] "+ body.Name); 
        }
    }

    private void OnBodyExit(Node2D body)
    {
        listenToInput = false;
    }


}
