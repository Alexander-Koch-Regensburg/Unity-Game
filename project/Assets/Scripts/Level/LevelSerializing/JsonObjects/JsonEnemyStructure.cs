using UnityEngine;

public class JsonEnemyStructure {
    public JsonFloatPositionStructure Position { get; set; } 
    public string Type { get; set; }
    public JsonFloatPositionStructure[] PatrolPoints { get; set; }
    public string WeaponType { get; set; }
    public int Amount { get; set; }
}