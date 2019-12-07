using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPlacementModule : MonoBehaviour, IObjectPlacementModule
{
    private static ObjectPlacementModule instance;

    private Sprite wallSprite;
    private Level level;
    private ILevelDataManager levelDataManager;
    private Dictionary<int, IPlacedObject> inScenePlacedObjects;
    private IObjectInformationModule objectInformationModule;
    private IObjectDeletionModule objectDeletionModule;

    public static ObjectPlacementModule GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
    }

    public void Setup(Dictionary<int, IPlacedObject> inScenePlacedObjects, IObjectDeletionModule objectDeletionModule, IObjectInformationModule objectInformationModule, Sprite wallSprite, ILevelDataManager levelDataManager)
    {
        this.inScenePlacedObjects = inScenePlacedObjects;
        this.wallSprite = wallSprite;
        this.objectInformationModule = objectInformationModule;
        this.objectDeletionModule = objectDeletionModule;
        this.levelDataManager = levelDataManager;
    }

    public void SetLevel(Level level)
    {
        this.level = level;
    }

    private IPlacedObject CloneObjectToPlace(IPlacedObject plObject)
    {
        IPlacedObject objectToPlace = (IPlacedObject)Instantiate((PlacedObject)plObject, ((PlacedObject)plObject).transform.parent);
        return objectToPlace;
    }

    public void PlaceObject(IPlacedObject objectToPlace, Vector3 mousePosition, bool checksEnabled = true, ILevelElement levelElement = null, bool cloneObject = true)
    {
        if (objectToPlace == null)
            return;

        if (checksEnabled)
        {
            bool containsLevelElement = objectInformationModule.CheckIfLevelTilesContainsLevelElements(objectToPlace, mousePosition);
            if (containsLevelElement)
                return;

            if (objectToPlace.Type == PrefabType.PLAYER)
            {
                bool playerExists = objectInformationModule.CheckIfPlayerExists();
                if (playerExists)
                    return;
            }
        }

        if (objectToPlace.Type == PrefabType.LEVELELEMENT)
            PlaceLevelElement(mousePosition, objectToPlace, levelElement, cloneObject);
        else
            PlaceNonLevelElement(mousePosition, objectToPlace, cloneObject);
    }

    private void PlaceNonLevelElement(Vector3 mousePosition, IPlacedObject objectToPlace, bool cloneObject)
    {
       InstantiatePlacedObject(mousePosition, objectToPlace, cloneObject);
    }

    private void PlaceLevelElement(Vector3 position, IPlacedObject objectToPlace, ILevelElement levelElement, bool cloneObject)
    {
        ISet<ILevelTile> levelTiles = objectInformationModule.GetLevelTilesOfLevelElement(objectToPlace, position);
        IList<IPlacedObject> placedObjects = objectInformationModule.GetInScenePlacedObjectsForLevelTiles(levelTiles);
        if (placedObjects.Count > 0)
            objectDeletionModule.DeletePlacedObjects(placedObjects, false, true);
        Vector2 worldPosition = objectInformationModule.GetPositionOfLevelTile(position);
        // The initialized placedObjects will be deleted automatically by garbage collector due to no reference at the end of this method
        // It is only important that the gameObjects are not visible anymore at this moment and will be deleted in the next time

        InstantiatePlacedObject(worldPosition, objectToPlace, cloneObject, levelElement);
    }

    private ILevelElement CreateLevelElement(IPlacedObject actualObjectPlaced)
    {
        LevelElementData data = ObjectToDataConverter.ObjectToLevelElement((PlacedObject)actualObjectPlaced);
        ILevelElement levelElement = LevelElementFactory.InstantiateLevelElement(data, ((PlacedObject)actualObjectPlaced).transform);
        return levelElement;
    }

    private void InstantiatePlacedObject(Vector2 worldPosition, IPlacedObject objectToPlace, bool cloneObject, ILevelElement levelElement = null)
    {
        IPlacedObject placedObject;
        if (cloneObject)
        {
            placedObject = CloneObjectToPlace(objectToPlace);
            ((PlacedObject)placedObject).transform.position = worldPosition;
        }
        else
        {
            placedObject = objectToPlace;
        }

        if (placedObject.Type == PrefabType.LEVELELEMENT)
        {
            //objectDeletionModule.RemoveWallFromTileMap(placedObject);
            Destroy(placedObject.Prefab);
            if (levelElement == null)
                levelElement = CreateLevelElement(placedObject);

            placedObject.Prefab = AssignLevelElementToPlacedObject(levelElement);
            AddWallToTileMap(placedObject.Prefab);

            DestroyeWallSpriteComponent(placedObject);
            AddSpriteRendererForWall(placedObject);

            levelDataManager.AddLevelElement(levelElement);
            bool outerBoundWall = objectInformationModule.CheckForOuterBoundWall(placedObject);
            if (outerBoundWall)
                placedObject.SetProtectedObject(true);
        }

        if (objectToPlace.Type is PrefabType.PLAYER)
            objectInformationModule.SetPlayerExists(true); 

        IList<ILevelTile> levelTiles = GetLevelTiles(placedObject, worldPosition, levelElement);
        placedObject.SetupInstantiatedObject(levelTiles, transform);
        LevelCreator.GetInstance().GetInScenePlacedObjects()[placedObject.ObjectId] = placedObject;
    }

    public IList<ILevelTile> GetLevelTiles(IPlacedObject placedObject, Vector2 position, ILevelElement levelElement)
    {
        IList<ILevelTile> levelTiles;

        if (placedObject.Type == PrefabType.LEVELELEMENT)
        {
            levelTiles = ((HashSet<ILevelTile>)levelElement.GetTiles()).ToList();
        }
        else
        {
            levelTiles = new List<ILevelTile>();
            ILevelTile levelTile = level.GetTileAtPos(position);
            levelTiles.Add(levelTile);
        }

        return levelTiles;
    }

    public GameObject AssignLevelElementToPlacedObject(ILevelElement levelElement)
    {
        if (levelElement is Door)
        {
            return ((Door)levelElement).gameObject;
        }
        else if (levelElement is Wall)
        {
            return ((Wall)levelElement).gameObject;
        }
        else if (levelElement is EndZone)
        {
            return ((EndZone)levelElement).gameObject;
        }
        return null;
    }

    public void SetWallSprite(IPlacedObject placedObject)
    {
        if (placedObject.Type == PrefabType.LEVELELEMENT)
        {
            Wall wall = placedObject.Prefab.GetComponent<Wall>();
            if (wall != null)
            {
                SpriteRenderer renderer = placedObject.Prefab.gameObject.AddComponent<SpriteRenderer>();
                renderer.sprite = wallSprite;
                renderer.sortingOrder = 1;
                renderer.sortingLayerName = "Wall";
            }
        }
    }

    private void DestroyeWallSpriteComponent(IPlacedObject placedObject)
    {
        if (placedObject.Type == PrefabType.LEVELELEMENT)
        {
            Wall wall = placedObject.Prefab.GetComponent<Wall>();
            if (wall != null)
            {
                SpriteRenderer renderer = placedObject.Prefab.gameObject.GetComponent<SpriteRenderer>();
                if (renderer == null)
                    return;
                Destroy(renderer);
            }
        }
    }

    private void AddSpriteRendererForWall(IPlacedObject placedObject)
    {
        if (placedObject.Type == PrefabType.LEVELELEMENT)
        {
            Wall wall = placedObject.Prefab.GetComponent<Wall>();
            if (wall != null)
            {
                SpriteRenderer renderer = placedObject.Prefab.gameObject.AddComponent<SpriteRenderer>();
                if (renderer == null)
                    return;
                renderer.sprite = null;
            }
        }
    }

    private void AddWallToTileMap(GameObject gObject)
    {
        Wall wall = gObject.GetComponent<Wall>();
        if (wall != null)
        {
            wall.AddToTilemap();
        }
    }
}
