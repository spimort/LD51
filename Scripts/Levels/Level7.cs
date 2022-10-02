using Godot;

public partial class Level7 : GenericLevel {
    private Marker3D _finalPositionMarker;

    public override void _Ready() {
        base._Ready();

        this._finalPositionMarker = this.GetNode<Marker3D>("%FinalPosition");

        var doorsNode = this.GetNode<Node3D>("%Doors");
        var randomValidDoorIndex = GD.RandRange(0, doorsNode.GetChildCount() - 1);
        for (var i = 0; i < doorsNode.GetChildCount(); i++) {
            var door = doorsNode.GetChild<Door>(i);
            var button = door.GetNode<ButtonTrigger>("Button");
			button.ButtonValidated += () => this.OnButtonValidated(door);

            if (randomValidDoorIndex == i) {
                var movingPlatformCollision = door.GetNode<CollisionShape3D>("MovingPlatformPosition/AnimatableBody3d/CollisionShape3d");
                var movingPlatformArea = door.GetNode<Area3D>("MovingPlatformPosition/AnimatableBody3d/Area3d");
                var movingPlatformAreaCollision = movingPlatformArea.GetNode<CollisionShape3D>("CollisionShape3d");

                movingPlatformCollision.Disabled = false;
                movingPlatformAreaCollision.Disabled = false;
                movingPlatformArea.BodyEntered += (Node3D body) => this.MovePlatformToTarget(door);
                movingPlatformArea.BodyExited += (Node3D body) => this.MovePlatformToOrigin(door);
            }
        }
    }

	private void OnButtonValidated(Door door) {
        door.GetNodeOrNull<Node3D>("StaticBody3d")?.QueueFree();
        door.Open();
	}

    private void MovePlatformToTarget(Door door) {
        var movingPlatform = door.GetNode<Node3D>("MovingPlatformPosition/AnimatableBody3d");

        var tween = this.GetTree().CreateTween();
        tween.TweenProperty(movingPlatform, "global_position", this._finalPositionMarker.GlobalPosition, 5);
    }

    private void MovePlatformToOrigin(Door door) {
        var originPositionMarker = door.GetNode<Node3D>("MovingPlatformPosition/AnimatableBody3d");
        var movingPlatform = door.GetNode<Node3D>("MovingPlatformPosition/AnimatableBody3d");

        var tween = this.GetTree().CreateTween();
        tween.TweenProperty(movingPlatform, "position", Vector3.Zero, 5);
    }
}
