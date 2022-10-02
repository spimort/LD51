using Godot;

public partial class GenericLevel : Node3D, ILevel {
    public event PlayerEntered PlayerEnteredEvent;

    private Area3D _levelArea;
    private Marker3D _spawnPointMarker;

    public Vector3 SpawnPoint => this._spawnPointMarker.GlobalPosition;

    public override void _Ready() {
        base._Ready();

        this._spawnPointMarker = this.GetNode<Marker3D>("%SpawnPoint");
        this._levelArea = this.GetNode<Area3D>("%LevelArea");
        this._levelArea.BodyEntered += OnPlayerEntered;
    }

    private void OnPlayerEntered(Node3D node) {
        this.PlayerEnteredEvent?.Invoke();
    }
}
