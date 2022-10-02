using Godot;

public partial class Level6 : GenericLevel {
	private Door _door;

    public override void _Ready() {
        base._Ready();

        this._door = this.GetNode<Door>("%Door");

		var buttonsNode = this.GetNode<Node3D>("%Buttons");
        var randomActiveIndex = GD.RandRange(0, buttonsNode.GetChildCount() - 1);
		for (var i = 0; i < buttonsNode.GetChildCount(); i++) {
            var button = buttonsNode.GetChild<ButtonTrigger>(i);
			button.ButtonValidated += this.OnButtonValidated;

            if (i == randomActiveIndex) {
                button.IsValid = true;
            }
		}
    }

	private void OnButtonValidated() {
		this._door.Open();
        this.GetNode<Node3D>("%DoorCollision")?.QueueFree();
	}
}
