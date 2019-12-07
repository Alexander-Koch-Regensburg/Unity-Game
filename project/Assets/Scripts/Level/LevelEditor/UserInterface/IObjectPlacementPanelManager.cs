using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectPlacementPanelManager
{
    void SetupPanel(Dictionary<string, IPlacedObject> prefabTable, GameObject buttonPrefab, float panelPadding);
}
