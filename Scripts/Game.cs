using Godot;
using System;
using System.Collections.Generic;

public partial class Game : Node {
	private const float FogMoveSpeed = 40f;
	private const int NumberOfDeadSouls = 1000;

	private double _currentTime;
	private Player _player;
	private FogWall _fog;
	private Area3D _deadZone;
	private List<ILevel> _levels = new List<ILevel>();
	private ILevel _currentLevel;
	private Node3D _groundDeads;
	private PackedScene _deadSoulPrefab;
	private AudioStreamPlayer3D _soundPlayer;
	private AudioStreamPlayer _10SecondPlayer;

    public override void _Ready() {
        base._Ready();

		this._deadSoulPrefab = ResourceLoader.Load<PackedScene>("res://Prefabs/DeadSoul.tscn");

		this._player = this.GetNode<Player>("%Player");
		this._fog = this.GetNode<FogWall>("%Fog");
		this._deadZone = this.GetNode<Area3D>("%DeadZone");
		this._groundDeads = this.GetNode<Node3D>("%GroundDeads");
		this._10SecondPlayer = this.GetNode<AudioStreamPlayer>("%10SecondPlayer");

		var levelsNode = this.GetNode<Node3D>("%Levels");
		var numberOfLevels = levelsNode.GetChildCount();

		for (var i = 0; i < numberOfLevels; i++) {
			var level = levelsNode.GetChild<ILevel>(i);
			level.PlayerEnteredEvent += () => {
				this.OnPlayerEnterLevel(level);
			};
		}

		this._deadZone.BodyEntered += OnPlayerEnterDeadZone;
		this._fog.PlayerHitByTheFogEvent += this.OnPlayerDead;
		this.GetNode<Area3D>("%WinArea3D").BodyEntered += (Node3D body) => {
			if (body is Player) {
				this.OnPlayerWin();
			}
		};

		this.InitDeadSouls();

		Input.MouseMode = Input.MouseModeEnum.Captured;
    }

	public override void _Process(double delta) {
		this._currentTime += delta;

		if (this._currentTime >= 10) {
			this._currentTime = 0;
			this.Every10Seconds();
		}
	}

	public override void _Input(InputEvent input) {
        base._Input(input);

		if (Input.IsActionJustPressed("Menu")) {
			Input.MouseMode = Input.MouseMode == Input.MouseModeEnum.Captured ? Input.MouseModeEnum.Visible : Input.MouseModeEnum.Captured;
		}
	}


	private void InitDeadSouls() {
		for (var i = 0; i < NumberOfDeadSouls; i++) {
			var randomX = GD.RandRange(-100, 100);
			var randomZ = GD.RandRange(10, -500);
			var deadSoul = this._deadSoulPrefab.Instantiate<DeadSoul>();
			deadSoul.Position = new Vector3(randomX, 0, randomZ);

			this._groundDeads.AddChild(deadSoul);
		}
	}

	private void Every10Seconds() {
		this._fog.Translate(new Vector3(0, 0, -FogMoveSpeed));
		this._10SecondPlayer.Play();
	}

	private void OnPlayerEnterDeadZone(Node3D body) {
		if (this._currentLevel != null) {
			var tween = this.GetTree().CreateTween();
			tween.TweenProperty(this._player, "position", this._currentLevel.SpawnPoint, 0.2f);
		}
	}

	private void OnPlayerEnterLevel(ILevel level) {
		this._currentLevel = level;
	}

	private void OnPlayerWin() {
		Input.MouseMode = Input.MouseModeEnum.Visible;
		this.GetTree().ChangeSceneToFile("res://Scenes/Win.tscn");
	}

	private void OnPlayerDead() {
		Input.MouseMode = Input.MouseModeEnum.Visible;
		this.GetTree().ChangeSceneToFile("res://Scenes/Dead.tscn");
	}
}
