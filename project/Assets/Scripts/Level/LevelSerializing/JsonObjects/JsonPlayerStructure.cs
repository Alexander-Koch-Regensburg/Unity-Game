using UnityEngine;
using UnityEditor;

public class JsonPlayerStructure {
    public string WeaponType { get; set; }
    public int Amount { get; set; }
    public JsonFloatPositionStructure SpawnPosition { get; set; }
}