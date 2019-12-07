using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ObjectPlacementPanelManager : MonoBehaviour, IObjectPlacementPanelManager
{
    public Transform deletionModePanel;

    public Transform weaponPanel;
    public List<IPlacedObject> weaponButtons = new List<IPlacedObject>();

    public Transform elementPanel;
    public List<IPlacedObject> levelElementButtons = new List<IPlacedObject>();

    public Transform enemyPanel;
    public List<IPlacedObject> enemyButtons = new List<IPlacedObject>();

    public Transform playerPanel;
    public List<IPlacedObject> playerButtons = new List<IPlacedObject>();

    private GameObject buttonPrefab;
    private float panelPadding;


    public void SetupPanel(Dictionary<string, IPlacedObject> prefabTable, GameObject buttonPrefab, float panelPadding)
    {
        this.buttonPrefab = buttonPrefab;
        this.panelPadding = panelPadding;

        LoadData(prefabTable);
        AddInterfaceButtons();
    }

    private void LoadData(Dictionary<string, IPlacedObject> prefabTable)
    {
        foreach (IPlacedObject plObject in prefabTable.Values)
        {
            switch (plObject.Type)
            {
                case PrefabType.LEVELELEMENT:
                    levelElementButtons.Add(plObject);
                    break;
                case PrefabType.PLAYER:
                    playerButtons.Add(plObject);
                    break;
                case PrefabType.ENEMY:
                    enemyButtons.Add(plObject);
                    break;
                case PrefabType.WEAPON:
                    weaponButtons.Add(plObject);
                    break;
                default:
                    Debug.Log("Warning(LevelEditor-InterfaceManager): Prefab of type " + plObject.Type + " unknown");
                    break;
            }
        }
    }

    private void AddInterfaceButtons()
    {
        AddPrefabButtons(enemyButtons, enemyPanel);
        AddPrefabButtons(weaponButtons, weaponPanel);
        AddPrefabButtons(levelElementButtons, elementPanel);
        AddPrefabButtons(playerButtons, playerPanel);

        AddDeletionModeButton();
    }

    private void AddPrefabButtons(List<IPlacedObject> placedObjects, Transform panel)
    {
        foreach (IPlacedObject plObject in placedObjects)
        {
            GameObject newButton = Instantiate(buttonPrefab);
            newButton.transform.SetParent(panel);

            LevelEditorButton levelEditorButton = newButton.GetComponent<LevelEditorButton>();
            levelEditorButton.SetupPrefab(plObject.Text, panel);

            levelEditorButton.gameObject.GetComponent<Button>().onClick.AddListener(delegate { InterfaceButtonHandler.GetInstance().PassObjectToPlace(plObject.PrefabId); InterfaceManager.GetInstance().OnButtonPressed(levelEditorButton); });
        }
    }

    private void AddDeletionModeButton()
    {
        GameObject newButton = Instantiate(buttonPrefab);
        newButton.transform.SetParent(deletionModePanel);

        LevelEditorButton levelEditorButton = newButton.GetComponent<LevelEditorButton>();
        levelEditorButton.SetupPrefab("Deletion Mode", deletionModePanel);

        levelEditorButton.gameObject.GetComponent<Button>().onClick.AddListener(delegate { InterfaceButtonHandler.GetInstance().onDeleteSelection(); InterfaceManager.GetInstance().OnButtonPressed(levelEditorButton); });
    }

}
