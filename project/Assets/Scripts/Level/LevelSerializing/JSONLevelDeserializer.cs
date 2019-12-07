using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class JsonLevelDeserializer : ILevelDeserializer {

    private string path;

    public JsonLevelDeserializer(string path) {
        this.path = path; 
    }

    /// <summary>
    /// Reads the json content from the file at the given path and returns a new JsonDataProvider that contains all information about the level structure.
    /// </summary>
    /// <returns>DataProvider that contains all information about the level structure.</returns>
    public ILevelDataProvider DeserializeLevel() {
        string jsonString = File.ReadAllText(path, Encoding.UTF8);
        JsonLevelStructure levelStructure = JsonConvert.DeserializeObject<JsonLevelStructure>(jsonString);

        JsonIntPositionStructure levelSize = levelStructure.Size;

        JsonPlayerStructure playerStructure = levelStructure.Player;

        IList<JsonWallStructure> wallElements = levelStructure.LevelElements.Walls;
        IList<JsonDoorStructure> doorElements = levelStructure.LevelElements.Doors;
		IList<JsonEndZoneStructure> endZones = levelStructure.LevelElements.EndZones;

        IList<JsonWeaponStructure> weaponElements = levelStructure.LevelItems.Weapons;

        IList<JsonEnemyStructure> enemyElements = levelStructure.Enemies;

        return new JsonDataProvider(levelStructure, playerStructure, wallElements, doorElements, endZones, weaponElements, enemyElements);
    }
}