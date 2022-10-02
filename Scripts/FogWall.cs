using Godot;
using System;

public delegate void PlayerHitByTheFog();

public partial class FogWall : Node3D {
    public event PlayerHitByTheFog PlayerHitByTheFogEvent;

	public override void _Ready() {
		this.GetNode<Area3D>("%Area3d").BodyEntered += (Node3D body) => {
			if (body is Player) {
				this.PlayerHitByTheFogEvent?.Invoke();
			}
		};
	}
}
