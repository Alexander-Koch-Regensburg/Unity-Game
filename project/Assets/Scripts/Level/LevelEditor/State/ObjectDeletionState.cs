using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDeletionState : State
{
    private IObjectDeletionModule objectDeletionModule;
    private MouseButton leftMouseButton;


    public ObjectDeletionState(IObjectDeletionModule objectDeletionModule, MouseButton leftMouseButton)
    {
        this.objectDeletionModule = objectDeletionModule;
        this.leftMouseButton = leftMouseButton;
    }

    public void OnMouseButton(Vector3 mousePosition, StateInformation stateInformation)
    {
        if (leftMouseButton.GetState() == MouseButtonState.PRESSED || leftMouseButton.GetState() == MouseButtonState.DOWN)
        {
            objectDeletionModule.DeleteObject(stateInformation.hoveredObject);
            stateInformation.hoveredObject = null;
        }
    }

}
