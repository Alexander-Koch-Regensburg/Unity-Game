using UnityEngine;

public class JsonEnvironmentPlayerStructure {
    public JsonFloatPositionStructure Position { get; set; } 
    public float Rotation { get; set; }
    public string WeaponType { get; set; }
    public int Ammunition { get; set; }
    public int Health { get; set; }
}