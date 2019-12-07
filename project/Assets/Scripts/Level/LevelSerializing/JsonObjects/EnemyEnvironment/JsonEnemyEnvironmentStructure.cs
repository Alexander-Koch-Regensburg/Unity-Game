using UnityEngine;

public class JsonEnemyEnvironmentStructure {
    public string ID { get; set; }
    public string Type { get; set; }
    public int Health { get; set; }
    public float MaxVelocity { get; set; }
    public float TurnVelocity { get; set; }
    public float PatrolVelocity { get; set; }
    public float CurrentVelocity { get; set; }
    public float Acceleration { get; set; }
    public float VisionAngle { get; set; }
    public float VisionRange { get; set; }
    public JsonFloatPositionStructure Position { get; set; }
    public float Rotation { get; set; }
    public string WeaponType { get; set; }
    public int Ammunition { get; set; }
    public int ProjectileDamage { get; set; }
    public float ProjectileSpeed { get; set; }
    public float Awareness { get; set; }
    public float InciteEnemiesRange { get; set; }
    public JsonFloatPositionStructure LastKnownPlayerPosition { get; set; }
    public JsonFloatPositionStructure CurrentDestination { get; set; }
    public JsonEnvironmentVisibleElementsStructure VisibleElements { get; set; }
}