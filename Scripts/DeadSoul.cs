using Godot;
using System;

public partial class DeadSoul : Node3D {
	private const double MinSecForSound = 1;
	private const double MaxSecForSound = 40;

	private AudioStreamPlayer3D _soundPlayer;
	private float _currentTimer = 0;

	public override void _Ready() {
		this._soundPlayer = this.GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3d");

		this._currentTimer = (float) GD.RandRange(MinSecForSound, MaxSecForSound);
	}

	public override void _Process(double delta) {
		if (this._currentTimer <= 0) {
			this._soundPlayer.Play();
			this._currentTimer = (float) GD.RandRange(MinSecForSound, MaxSecForSound);
		} else {
			this._currentTimer -= (float) delta;
		}
	}
}
