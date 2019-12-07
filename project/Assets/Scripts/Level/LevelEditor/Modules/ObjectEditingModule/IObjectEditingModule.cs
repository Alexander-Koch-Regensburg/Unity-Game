using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectEditingModule
{
    void Setup(IObjectInformationModule objectInformationModule);
    void ResetPatrolPoints(IPlacedObject placedObject);
    void AddPatrolPoint(IPlacedObject selectedObject, Vector2 position);
    void MoveObject(IPlacedObject selectedObject, Vector2 position);
    void GiveWeaponToPerson(IPlacedObject selectedObject, IPlacedObject hoveredObject);
    void ChangeAmmunitionAmount(IPlacedObject selectedObject, int amount);
}
