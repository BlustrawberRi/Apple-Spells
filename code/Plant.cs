using Godot;
using System;
using System.Runtime.CompilerServices;

[Tool]

[Icon("res://editor/icons/Plant.svg")]
public partial class Plant : StaticBody2D
{
	public int MaxGrowthPhase
	{
        get => _maxGrowthPhase; 
        private set => _maxGrowthPhase = value;
    }
    [Export]
	public int CurrentGrowthPhase
    {
        get => _currentGrowthPhase; 
        private set 
		{
			if (value > MaxGrowthPhase || 
			value == _currentGrowthPhase ||
			value < 0) return;

			_currentGrowthPhase = value;
			PlantSprite.Frame = _currentGrowthPhase;
		}
    }

	[Export]
    public string PlantType { get => _plantType; private set => _plantType = value; }
	
	public Sprite2D PlantSprite
	{
		get => _plantSprite;
		private set => _plantSprite = value;
	}



    private int _maxGrowthPhase;
    private int _currentGrowthPhase;
    private string _plantType;
    private Sprite2D _plantSprite;

    // Called when the node enters the scene tree for the first time.
    public override async void _Ready()
	{
		PlantSprite = GetNode<Sprite2D>("PlantSprite");
		MaxGrowthPhase = PlantSprite.Hframes-1;
	}

	public void OnTimerTimeout()
	{
		CurrentGrowthPhase++;
	}
}
