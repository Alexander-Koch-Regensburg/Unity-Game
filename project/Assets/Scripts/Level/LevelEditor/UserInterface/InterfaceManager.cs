using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InterfaceManager : MonoBehaviour, IInterfaceManager
{
    private static InterfaceManager instance = null;
    private IObjectEditorPanelManager objectEditorPanelManager;
    private IObjectPlacementPanelManager objectPlacementPanelManager;
    private ILevelManagementPanelManager levelManagementPanelManager;

    public GameObject levelManagementPanelObject;
    public GameObject objectEditorPanelObject;
    public GameObject objectPlacementPanelObject;

    private static float panelPadding = 1f;
    private LevelEditorButton lastButtonPressed;

    private bool mouseOverUIElement;
    public bool MouseOverUIElement {
        get {
            return mouseOverUIElement;
        }
        set {
            mouseOverUIElement = value;
        }
    }

    public GameObject buttonPrefab;

    public Transform objectEditorPanel;
    private bool editorPanelVisible = false;
    public bool EditorPanelVisible {
        get {
            return editorPanelVisible;
        }
        set {
            editorPanelVisible = value;
        }
    }

    public void Awake()
    {
        instance = this;
    }

    void Start()
    {
        levelManagementPanelManager = (ILevelManagementPanelManager)levelManagementPanelObject.GetComponent<LevelManagementPanelManager>();
        objectEditorPanelManager = (IObjectEditorPanelManager)objectEditorPanelObject.GetComponent<ObjectEditorPanelManager>();
        objectPlacementPanelManager = (IObjectPlacementPanelManager)objectPlacementPanelObject.GetComponent<ObjectPlacementPanelManager>();
}

    void Update()
    {
        ShowObjectEditorPanel(editorPanelVisible);
    }

    public void SetupUI(Dictionary<string, IPlacedObject> prefabTable)
    {
        objectPlacementPanelManager.SetupPanel(prefabTable, buttonPrefab, panelPadding);
        levelManagementPanelManager.SetupPanel(buttonPrefab);
    }

    public void SetupObjectEditorPanel(IPlacedObject placedObject)
    {
        objectEditorPanelManager.SetupPanel(placedObject);
    }

    public void MouseEnter()
    {
        mouseOverUIElement = true;
    }

    public void MouseExit()
    {
        mouseOverUIElement = false;
    }

    public static InterfaceManager GetInstance()
    {
        return instance;
    }

    private void ShowObjectEditorPanel(bool setActive)
    {
        objectEditorPanel.gameObject.SetActive(setActive);
    }

    public void ResetButtonPressedColor()
    {
        if (lastButtonPressed == null)
            return;
        lastButtonPressed.RemovePressedColor();
        lastButtonPressed = null;
    }

    public void OnButtonPressed(LevelEditorButton levelEditorButton)
    {
        if (lastButtonPressed != null)
            lastButtonPressed.RemovePressedColor();
        if (lastButtonPressed != levelEditorButton)
        {
            levelEditorButton.SetPressedColor();
            lastButtonPressed = levelEditorButton;
        }
        else
            lastButtonPressed = null;
    }

}
