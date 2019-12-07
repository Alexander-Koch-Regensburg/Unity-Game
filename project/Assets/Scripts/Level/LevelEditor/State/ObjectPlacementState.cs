using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacementState : State
{
    private IObjectPlacementModule objectPlacementModule;
    private MouseButton leftMouseButton;

    public ObjectPlacementState(IObjectPlacementModule objectPlacementModule, MouseButton leftMouseButton)
    {
        this.objectPlacementModule = objectPlacementModule;
        this.leftMouseButton = leftMouseButton;
    }

    public void OnMouseButton(Vector3 mousePosition, StateInformation stateInformation)
    {
        if (leftMouseButton.GetState() == MouseButtonState.PRESSED && stateInformation.objectToPlace.Type == PrefabType.LEVELELEMENT)
        {
            objectPlacementModule.PlaceObject(stateInformation.objectToPlace, mousePosition);
        }
        else if (leftMouseButton.GetState() == MouseButtonState.DOWN)
        {
            objectPlacementModule.PlaceObject(stateInformation.objectToPlace, mousePosition);
        }

    }
}
