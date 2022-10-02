using Godot;

public delegate void PlayerEntered();

public interface ILevel {
    event PlayerEntered PlayerEnteredEvent;
    Vector3 SpawnPoint { get; }
}
