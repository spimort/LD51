using Godot;
using System;

public delegate void ButtonValidated();

public partial class ButtonTrigger : Node {
	private const float ValidateButtonTime = 2;

	public event ButtonValidated ButtonValidated;

	private Area3D _area;
	private AnimationPlayer _animationPlayer;
	private Timer _timer;
	private AudioStreamPlayer _validatingSound;
	private AudioStreamPlayer _validSound;
	private AudioStreamPlayer _wrongSound;

	[Export]
	public bool IsValid { get;set; }
	[Export]
	public bool UseDelay { get;set; } = true;

	public override void _Ready() {
		this._area = this.GetNode<Area3D>("%Area3d");
		this._animationPlayer = this.GetNode<AnimationPlayer>("%AnimationPlayer");
		this._timer = this.GetNode<Timer>("%Timer");
		this._validatingSound = this.GetNode<AudioStreamPlayer>("%ValidatingSound");
		this._validSound = this.GetNode<AudioStreamPlayer>("%ValidSound");
		this._wrongSound = this.GetNode<AudioStreamPlayer>("%WrongSound");

		this._area.BodyEntered += this.OnBodyEntered;
		this._area.BodyExited += this.OnBodyExited;
		this._timer.Timeout += this.OnTimout;
	}

	private void OnBodyEntered(Node3D body) {
		if (this.UseDelay) {
			this._timer.Stop();
			this._timer.Start(ValidateButtonTime);

			this._animationPlayer.Stop();
			this._animationPlayer.Play("Validating");
			this._validatingSound.Play();
		} else {
			this.OnTimout();
		}
	}

	private void OnBodyExited(Node3D body) {
		this._animationPlayer.Stop();
		this._animationPlayer.Play("RESET");
		this._timer.Stop();
	}

	private void OnTimout() {
		if (this.IsValid) {
			this.ButtonValidated?.Invoke();

			this._animationPlayer.Stop();
			this._animationPlayer.Play("Valid");
			this._validSound.Play();
		} else {
			this._animationPlayer.Stop();
			this._animationPlayer.Play("RESET");
			this._wrongSound.Play();
		}
	}
}
