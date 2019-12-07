public class JsonEnvironmentProjectileStructure {
    public JsonFloatPositionStructure Position { get; set; }
    public JsonFloatPositionStructure Direction { get; set; }
    public float Velocity { get; set; }
    public int Damage { get; set; }
    public string Origin { get; set; } // either the ID of an NPC or "Player"
}