public class JsonLevelStructure {
    public JsonIntPositionStructure Size { get; set; }
    public JsonPlayerStructure Player { get; set; }
    public JsonLevelElementStructure LevelElements { get; set; }
    public JsonLevelItemStructure LevelItems { get; set; }
    public JsonEnemyStructure[] Enemies { get; set; }
}