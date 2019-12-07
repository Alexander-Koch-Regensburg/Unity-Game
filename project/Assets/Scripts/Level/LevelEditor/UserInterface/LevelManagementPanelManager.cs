using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManagementPanelManager : MonoBehaviour, ILevelManagementPanelManager
{

    public GameObject levelManagementButtonPrefab;
    public Transform newLevelPanel;
    public Transform newLevelButtonPanel;
    public Transform loadLevelPanel;
    public Transform saveLevelPanel;


    private GameObject buttonPrefab;

    public void SetupPanel(GameObject buttonPrefab)
    {
        this.buttonPrefab = buttonPrefab;

        AddInterfaceButtons();
    }

    private void AddInterfaceButtons()
    {
        AddNewLevelButton();
        AddLoadLevelButton();
        AddSaveLevelButton();
    }

    private void AddNewLevelButton()
    {
        GameObject newButton = Instantiate(levelManagementButtonPrefab);
        newButton.transform.SetParent(newLevelButtonPanel);

        LevelEditorButton levelEditorButton = newButton.GetComponent<LevelEditorButton>();
        levelEditorButton.SetupConfigLevelButton("New Level", newLevelButtonPanel);

        levelEditorButton.gameObject.GetComponent<Button>().onClick.AddListener(delegate { InterfaceButtonHandler.GetInstance().onAddNewLevel(newLevelPanel); InterfaceManager.GetInstance().OnButtonPressed(levelEditorButton); });
    }

    private void AddLoadLevelButton()
    {
        GameObject newButton = Instantiate(levelManagementButtonPrefab);
        newButton.transform.SetParent(loadLevelPanel);

        LevelEditorButton levelEditorButton = newButton.GetComponent<LevelEditorButton>();
        levelEditorButton.SetupConfigLevelButton("Load Level", loadLevelPanel);

        levelEditorButton.gameObject.GetComponent<Button>().onClick.AddListener(delegate { InterfaceButtonHandler.GetInstance().OnLoadLevel(); InterfaceManager.GetInstance().OnButtonPressed(levelEditorButton); });
    }

    private void AddSaveLevelButton()
    {
        GameObject newButton = Instantiate(levelManagementButtonPrefab);
        newButton.transform.SetParent(saveLevelPanel);

        LevelEditorButton levelEditorButton = newButton.GetComponent<LevelEditorButton>();
        levelEditorButton.SetupConfigLevelButton("Save Level", saveLevelPanel);

        levelEditorButton.gameObject.GetComponent<Button>().onClick.AddListener(delegate { InterfaceButtonHandler.GetInstance().OnSaveLevel(); InterfaceManager.GetInstance().OnButtonPressed(levelEditorButton); });
    }

}
