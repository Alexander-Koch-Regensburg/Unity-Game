using UnityEngine;

public class JsonEnvironmentEnemyStructure {
    public string ID { get; set; }
    public JsonFloatPositionStructure Position { get; set; }
    public string Type { get; set; }
    public string WeaponType { get; set; }
    public int Ammunition { get; set; }
    public int Health { get; set; }
    public float Awareness { get; set; }
}