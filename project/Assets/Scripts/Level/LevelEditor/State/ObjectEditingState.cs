using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEditingState : State
{
    private MouseButton leftMouseButton;
    private IObjectEditingModule objectEditingModule;
    private IObjectInformationModule objectInformationModule;
    private MouseButtonState lastLeftMouseButtonState;

    public ObjectEditingState(MouseButton leftMouseButton, IObjectEditingModule objectEditingModule, IObjectInformationModule objectInformationModule)
    {
        this.leftMouseButton = leftMouseButton;
        this.objectEditingModule = objectEditingModule;
        this.objectInformationModule = objectInformationModule;

        this.lastLeftMouseButtonState = MouseButtonState.NONE;
    }

    public void OnMouseButton(Vector3 mousePosition, StateInformation stateInformation)
    {
        IPlacedObject selectedObject = stateInformation.selectedObject;
        MouseButtonState leftMouseButtonState = leftMouseButton.GetState();

        // moving the Object
        if (leftMouseButtonState == MouseButtonState.DOWN)
        {
            selectedObject.SetAllCollidersStatus(false);
        }
        else if (leftMouseButtonState == MouseButtonState.PRESSED)
        {
            selectedObject.RemoveWeaponFromPerson();
            //selectedObject.SetAllCollidersStatus(false);
            objectEditingModule.MoveObject(selectedObject, mousePosition);
        }
        else if (leftMouseButtonState == MouseButtonState.UP)
        {
            selectedObject.SetAllCollidersStatus(true);
        }

        // Giving an Enemy a Weapon
        if (leftMouseButtonState == MouseButtonState.UP && lastLeftMouseButtonState == MouseButtonState.PRESSED)
        {
            IPlacedObject hoveredObject = stateInformation.hoveredObject;
            bool objectCanBeGiven = objectInformationModule.CheckIfWeaponCanBeGivenToPerson(selectedObject, hoveredObject);
            if (objectCanBeGiven)
            {
                // give selectedObject to hoveredObject
                objectEditingModule.GiveWeaponToPerson(selectedObject, hoveredObject);
            }
        }
        lastLeftMouseButtonState = leftMouseButtonState;
    }
}
