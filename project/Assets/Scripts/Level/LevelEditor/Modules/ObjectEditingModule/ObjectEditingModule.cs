using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class ObjectEditingModule : MonoBehaviour, IObjectEditingModule
{
    private static ObjectEditingModule instance;

    private IObjectInformationModule objectInformationModule;

    public static ObjectEditingModule GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
    }

    public void Setup(IObjectInformationModule objectInformationModule)
    {
        this.objectInformationModule = objectInformationModule;
    }

    public void ResetPatrolPoints(IPlacedObject placedObject)
    {
        Enemy enemy = placedObject.Prefab.GetComponent<Enemy>();
        if (enemy == null)
            return;

        enemy.PatrolPoints = new List<Vector2>();
        //enemy.PatrolPoints.Add(((PlacedObject)placedObject).transform.position);
    }

    public void ChangeAmmunitionAmount(IPlacedObject selectedObject, int amount)
    {
        Weapon weapon = selectedObject.Prefab.GetComponent<Weapon>();
        weapon.SetAmmo(amount);
    }

    public void AddPatrolPoint(IPlacedObject selectedObject, Vector2 position)
    {
        Enemy enemy = selectedObject.Prefab.GetComponent<Enemy>();
        if (enemy == null)
            return;

        IList<Vector2> patrolPoints = enemy.PatrolPoints;
        if (patrolPoints == null)
        {
            patrolPoints = new List<Vector2>();
            //patrolPoints.Add(((PlacedObject)selectedObject).transform.position);
        }

		// Convert position to be tile-based
        patrolPoints.Add(new Vector2Int((int) position.x, (int) position.y));
    }

    public void MoveObject(IPlacedObject selectedObject, Vector2 position)
    {
        if (selectedObject == null)
            return;

        if (selectedObject.Type != PrefabType.LEVELELEMENT && !selectedObject.IsProtectedObject)
        {
            bool canBeMoved = objectInformationModule.CheckIfObjectCanBeMoved(selectedObject, position);
            if (canBeMoved)
                MoveObjectIntern(selectedObject, position);
        }
    }

    private void MoveObjectIntern(IPlacedObject selectedObject, Vector2 position)
    {
        ((PlacedObject)selectedObject).gameObject.transform.position = position;

        IList<PlacedObject> movedObjects = ((PlacedObject)selectedObject).gameObject.GetComponentsInChildren<PlacedObject>();
        foreach(PlacedObject movedObject in movedObjects)
        {
            IPlacedObject iMovedObject = (IPlacedObject)movedObject;
            UpdateLevelTiles(iMovedObject);
        }
    }

    public void UpdateLevelTiles(IPlacedObject selectedObject)
    {
        Vector2 position = ((PlacedObject)selectedObject).gameObject.transform.position;
        if (selectedObject.Type == PrefabType.LEVELELEMENT)
        {
            ISet<ILevelTile> levelTiles = objectInformationModule.GetLevelTilesOfLevelElement(selectedObject, position);
            selectedObject.LevelTiles = levelTiles.ToList();
        }
        else
        {
            ILevelTile levelTile = objectInformationModule.GetLevelTile(position);
            IList <ILevelTile> levelTiles = new List<ILevelTile>();
            levelTiles.Add(levelTile);
            selectedObject.LevelTiles = levelTiles;
        }
    }

    public void GiveWeaponToPerson(IPlacedObject selectedObject, IPlacedObject hoveredObject)
    {
        Weapon weapon = selectedObject.Prefab.GetComponent<Weapon>();
        if (weapon == null)
            return;

        Person person = hoveredObject.Prefab.GetComponent<Person>();
        if (person == null)
            return;

        ((PlacedObject)selectedObject).Interact(person);
    }
}
