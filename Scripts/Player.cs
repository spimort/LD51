using Godot;
using System;

public partial class Player : CharacterBody3D {
	[Export]
	public float Gravity { get;set; } = 1f;
	[Export]
	public float MaxGravity { get;set; } = -30f;
	[Export]
	public float PlayerSpeed { get;set; } = 6f;
	[Export]
	public float PlayerRunningSpeed { get;set; } = 12f;
	[Export]
	public float MouseSensitivity { get;set; } = 0.2f;
	[Export]
	public float JumpForce { get;set; } = 30f;
	[Export]
	public int MaxCoyoteJumpFrames { get;set; } = 10;
	[Export]
	public float WalkEffectSpeed { get;set; } = 0.35f;
	[Export]
	public float RunEffectSpeed { get;set; } = 0.25f;

	private Node3D _head;
	private float _currentGravity;
    private int _coyoteJumpFramesLeft = 0;
	private AudioStreamPlayer _walkAudioStream;
	private float _lastWalkEffect;
	private Camera3D _camera;
	private bool _oddMovement = false;

	public override void _Ready() {
		this._head = this.GetNode<Node3D>("%Head");
		this._walkAudioStream = this.GetNode<AudioStreamPlayer>("%WalkAudioStream");
		this._camera = this.GetNode<Camera3D>("%Camera3d");
	}

    public override void _Input(InputEvent input) {
        base._Input(input);

		if (input is InputEventMouseMotion) {
			var mouseMotion = (InputEventMouseMotion) input;
			this.RotateY(Mathf.DegToRad(-mouseMotion.Relative.x * this.MouseSensitivity));

			this._head.RotateX(Mathf.DegToRad(-mouseMotion.Relative.y * this.MouseSensitivity));
			var resultRotation = this._head.Rotation;
			resultRotation.x = Mathf.Clamp(resultRotation.x, Mathf.DegToRad(-85), Mathf.DegToRad(85));
			this._head.Rotation = resultRotation;
		}
    }

    public override void _PhysicsProcess(double delta) {
		var moveX = Input.GetActionStrength("MoveRight") - Input.GetActionStrength("MoveLeft");
		var moveY = Input.GetActionStrength("MoveBackward") - Input.GetActionStrength("MoveForward");
		var isJumping = Input.IsActionJustPressed("Jump") && (this.IsOnFloor() || this._coyoteJumpFramesLeft > 0);
		var isRunning = Input.IsActionPressed("Run");

		if (this.IsOnFloor()) {
			this._coyoteJumpFramesLeft = MaxCoyoteJumpFrames;
		} else if (this._coyoteJumpFramesLeft > 0) {
            this._coyoteJumpFramesLeft--;
		}

		if (isJumping) {
			this._currentGravity = JumpForce;
		} else {
			this._currentGravity -= this.Gravity;
			if (this._currentGravity < this.MaxGravity) {
				this._currentGravity = this.MaxGravity;
			}
		}

		var playerSpeed = isRunning ? PlayerRunningSpeed : PlayerSpeed;

		this.Velocity = new Vector3(moveX * playerSpeed, this._currentGravity, moveY * playerSpeed);
		this.Velocity = this.Velocity.Rotated(Vector3.Up, this.Rotation.y);
		this.MoveAndSlide();

		if ((moveX != 0 || moveY != 0) && this.IsOnFloor()) {
			if (this._lastWalkEffect <= 0) {
				var effectSpeed = isRunning ? RunEffectSpeed : WalkEffectSpeed;

				this._walkAudioStream.Play();
				this._lastWalkEffect = effectSpeed;

				var tween = this.GetTree().CreateTween();
				tween.TweenProperty(this._camera, "rotation", new Vector3(0, 0, this._oddMovement ? 0.01f : -0.01f), (effectSpeed) / 2);
				tween.TweenProperty(this._camera, "rotation", Vector3.Zero, (effectSpeed) / 2);

				this._oddMovement = !this._oddMovement;
			} else {
				this._lastWalkEffect -= (float) delta;
			}
		}
    }
}
