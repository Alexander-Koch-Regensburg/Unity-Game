using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInterfaceManager {

    bool MouseOverUIElement { get; set; }
    bool EditorPanelVisible { get; set; }

    void SetupUI(Dictionary<string, IPlacedObject> prefabs);
    void SetupObjectEditorPanel(IPlacedObject placedObject);
    void ResetButtonPressedColor();
}
