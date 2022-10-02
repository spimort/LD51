using Godot;
using System;

public partial class Win : Node {
	public override void _Ready() {
		this.GetNode<Button>("%StartButton").Pressed += async () => {
			this.GetNode<AnimationPlayer>("%AnimationPlayer").Play("StartGame");
			await ToSignal(this.GetTree().CreateTimer(1), "timeout");
			this.GetTree().ChangeSceneToFile("res://Scenes/Game.tscn");
		};

		this.GetNode<Button>("%QuitButton").Pressed += () => {
			this.GetTree().Quit();
		};
	}
}
