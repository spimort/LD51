using Godot;
using System;

public partial class Door : Node {
	private AnimationPlayer _animationPlayer;
	private bool _isOpen;

	public override void _Ready() {
		this._animationPlayer = this.GetNode<AnimationPlayer>("%AnimationPlayer");
	}

	public void Open() {
		if (!this._isOpen) {
			this._animationPlayer.Play("Open");
			this._isOpen = true;
		}
	}
}
