using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using SFB;
using System.Collections;

public class LevelController : MonoBehaviour, ILevelController {

	public static LevelController instance;

    private ILevelDataProvider dataProvider;

    private ILevelLoader levelLoader;

	/// <summary>
	/// The <c>Level</c> on which all elements will be placed on
	/// </summary>
	private Level level;

    /// <summary>
    /// Determines if a level is currently played or created
    /// </summary>
    public bool levelCreationMode = true;

    private LevelDataCollection levelDataCollection;

    /// <summary>
    /// The serializer of the level. 
    /// </summary>
    private ILevelSerializer levelSerializer;

    /// <summary>
    /// The deserializer of the level. 
    /// </summary>
    private ILevelDeserializer levelDeserializer;

    /// <summary>
    /// Boolean variable that indicates whether an existing level should be serialized or not. 
    /// </summary>
    private bool serializeLevel;

    /// <summary>
    /// String variable that holds the path where the actual level should be saved at.  
    /// </summary>
    private string pathToSave;

    private bool showSuccessfulSavedMessage;


    private void Update() {
        if (!MainMenuPlayerPreferences.InLevelCreationMode)
        {
            // In case the F2 key is pressed, run the following code to save the game. 
            if (Input.GetKeyDown(KeyCode.F2))
            {
                UpdateLevelDataCollection();
                StartCoroutine(SaveLevelIntern(true));
            }
            if (Input.GetKeyDown(KeyCode.F3))
            {
                UpdateLevelDataCollection();
                StartCoroutine(SaveLevelIntern(false));
            }
        }
    }

    private void UpdateLevelDataCollection() {
        // Get all active game objects and sort them into the right place in the updatedDataCollection.
        LevelDataCollection updatedDataCollection = new LevelDataCollection();

        Item[] items = GetComponentsInChildren<Item>();
        for (int i = 0; i < items.Length; i++) {
            updatedDataCollection.levelItems.Add(items[i]);
        }

        LevelElement[] levelElements = GetComponentsInChildren<LevelElement>();
        for (int i = 0; i < levelElements.Length; i++) {
            updatedDataCollection.levelElements.Add(levelElements[i]);
        }

        Enemy[] enemies = PersonController.instance.GetComponentsInChildren<Enemy>();
        for (int i = 0; i < enemies.Length; i++) {
            updatedDataCollection.enemies.Add(enemies[i]);
        }

        levelDataCollection = updatedDataCollection;
    }

    public void SaveLevel(bool selectPath)
    {
        StartCoroutine(SaveLevelIntern(selectPath));
    }

    private IEnumerator SaveLevelIntern(bool selectPath)
    {
        string actualDateTime = DateTime.Now.ToString();
        actualDateTime = actualDateTime.Replace(':', '.');
        actualDateTime = actualDateTime.Replace(' ', '_');

        if(selectPath) { 
            // Open a new windows dialog and let the user choose a directory where the game will be stored. 
            pathToSave = StandaloneFileBrowser.SaveFilePanel("Select json file to save.", "", string.Concat("SaveGame_", actualDateTime), "json");

            try {
                SerializeLevel();
            }
            catch (Exception) {
                Debug.LogError("You have to choose a valid directory and file name. Could not save current game.");
            }

        } else {
            pathToSave = string.Concat("QuickSaveGame_", actualDateTime, ".json");
            try {
                SerializeLevel();
            }
            catch (Exception ex) {
                Debug.LogError(ex.StackTrace);
            }
            showSuccessfulSavedMessage = true;
            yield return new WaitForSeconds(3);
            showSuccessfulSavedMessage = false;
        }
    }

    public void OnGUI() {
        if(showSuccessfulSavedMessage) {
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 200f, 200f), "Game successful saved!");
        }
    }

    void Awake() {
		instance = this;
        serializeLevel = false;
    }

	private void Start() {
        if (MainMenuPlayerPreferences.InLevelCreationMode)
        {
            return;
        }
        
        if(MainMenuPlayerPreferences.LoadFromJson) {
            dataProvider = DeserializeLevel();
        }
        else {
            dataProvider = new TestLevelDataProvider();
        }

        levelLoader = new BasicLevelLoader(dataProvider);

        LoadLevel();

        if (MainMenuPlayerPreferences.LoadFromJson) {
            JsonDataProvider jsonDataProvider = (JsonDataProvider)dataProvider;
            levelDataCollection.playerData = jsonDataProvider.GetPlayerData();
        }

        levelDataCollection.enemyData = dataProvider.GetEnemyData();
        levelDataCollection.enemies = LoadEnemies(levelDataCollection.enemyData);

        if (serializeLevel) {
            SerializeLevel();
        }

        StartLevel();
    }

	/// <summary>
	/// Loads the level and all related GameObjects like <c>LevelElement</c>s using the <c>ILevelLoader</c>
	/// </summary>
	public void LoadLevel() {
        if (MainMenuPlayerPreferences.InLevelCreationMode)
        {
            dataProvider = DeserializeLevel();
            levelLoader = new BasicLevelLoader(dataProvider);
        }
        // Execute the following in try catch blocks, because levelLoader could be null.
        try {
            levelDataCollection = new LevelDataCollection();
		    level = levelLoader.LoadLevel();
            levelDataCollection.levelElements = levelLoader.LoadLevelElements(transform);
            levelDataCollection.levelItemsData = levelLoader.LoadItemsData();
            levelDataCollection.levelItems = levelLoader.LoadItems(transform, levelDataCollection.levelItemsData);

            if (MainMenuPlayerPreferences.InLevelCreationMode)
            {
                levelDataCollection.enemyData = dataProvider.GetEnemyData();
                levelDataCollection.enemies = LoadEnemies(levelDataCollection.enemyData);

                JsonDataProvider jsonDataProvider = (JsonDataProvider)dataProvider;
                levelDataCollection.playerData = jsonDataProvider.GetPlayerData();
            }

        } catch(NullReferenceException ex) {
            Debug.LogError("Cannot load level. No ILevelLoader assigned!");
            throw new NullReferenceException("Cannot load level. No ILevelLoader assigned!", ex);
        }
    }

    public IList<Enemy> LoadEnemies(IList<EnemyData> data) {
        return PersonController.instance.SpawnEnemies(data);
    }

	public void StartLevel() {
        PersonController.instance.SpawnPlayer(levelDataCollection.playerData, level.getSpawnPosition());
    }

    /// <summary>
    /// Creates a new json level serializer that creates a json file from the given level structure and level elements.  
    /// </summary>
    public void SerializeLevel() {
        if(levelSerializer == null) {
            levelSerializer = new JsonLevelSerializer();
        }
        
        levelSerializer.SerializeLevel(pathToSave, level, levelDataCollection.levelElements, levelDataCollection.levelItems, levelDataCollection.enemies);
    }

    /// <summary>
    /// Creates a new json level deserializer that returns an ILevelDataProvider.
    /// </summary>
    /// <returns>ILevelDataProvider that contains the structure of the level.</returns>
    public ILevelDataProvider DeserializeLevel() {
        levelDeserializer = new JsonLevelDeserializer(MainMenuPlayerPreferences.PathToLoad);

        return levelDeserializer.DeserializeLevel();
    }

    public void RemoveLevelElement(ILevelElement element) {
		if (levelDataCollection.levelElements.Remove(element) == false) {
			return;
		}
		ISet<ILevelTile> tiles = element.GetTiles();
		foreach(ILevelTile tile in tiles) {
			tile.RemoveLevelElement(element);
		}
		if (element is LevelElement) {
			Destroy((LevelElement)element);
		}
	}

	public Vector2Int GetLevelSize() {
		return level.GetLevelSize();
	}

	public ILevelTile GetTileAtWorldPos(Vector2 pos) {
        if (pos.x < 0 || pos.y < 0) {
            return null;
        }
		return level.GetTileAtPos(pos);
	}

	public ILevelTile GetTileAtWorldPos(Vector2Int pos) {
        if (pos.x < 0 || pos.y < 0) {
            return null;
        }
        return level.GetTileAtPos(pos);
	}

	public ISet<ILevelTile> GetTilesInRectangle(Vector2Int pos, Vector2Int size) {
        if (pos.x < 0 || pos.y < 0) {
            return null;
        }
        if (size.x <= 0 || size.y <= 0) {
            return null;
        }
        return level.GetTilesInRectangle(pos, size);
	}

    public void SetLevel(Level lvl)
    {
        level = lvl;
    }

    public void SetLevelDataCollection(LevelDataCollection levelDataCollection)
    {
        this.levelDataCollection = levelDataCollection;
    }

    public LevelDataCollection GetLevelDataCollection()
    {
        return levelDataCollection;
    }

    public Level GetLevel()
    {
        return level;
    }
}
