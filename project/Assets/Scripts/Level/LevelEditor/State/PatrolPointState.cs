using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPointState : State
{
    private IObjectEditingModule objectEditingModule;
    private MouseButton leftMouseButton;
    private IInterfaceManager interfaceManager;

    public PatrolPointState(IObjectEditingModule objectEditingModule, MouseButton leftMouseButton, IInterfaceManager interfaceManager)
    {
        this.leftMouseButton = leftMouseButton;
        this.objectEditingModule = objectEditingModule;
        this.interfaceManager = interfaceManager;
    }

    public void OnMouseButton(Vector3 mousePosition, StateInformation stateInformation)
    {
        if (leftMouseButton.GetState() == MouseButtonState.DOWN)
        {
            objectEditingModule.AddPatrolPoint(stateInformation.selectedObject, mousePosition);
            interfaceManager.SetupObjectEditorPanel(stateInformation.selectedObject);
        }
    }
}
