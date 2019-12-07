public class JsonEnemyCommandStructure {
    public string EnemyID { get; set; }
    public string Method{ get; set; }
    public float? Rotation { get; set; }
    public JsonFloatPositionStructure Destination { get; set; }
    public float? Velocity { get; set; }
    public string InteractableID { get; set; }
}
