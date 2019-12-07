using UnityEngine;

public class JsonEnvironmentVisibleElementsStructure {
    public JsonLevelElementStructure LevelElements { get; set; }
    public JsonEnvironmentLevelItemsStructure LevelItems { get; set; }
    public JsonEnvironmentEnemyStructure[] Enemies { get; set; }
    public JsonEnvironmentPlayerStructure Player { get; set; }
    public JsonEnvironmentProjectileStructure[] Projectiles { get; set; }
}