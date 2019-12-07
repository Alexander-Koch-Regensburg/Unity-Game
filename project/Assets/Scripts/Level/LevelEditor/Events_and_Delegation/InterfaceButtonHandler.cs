using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class InterfaceButtonHandler : MonoBehaviour
{
    private static InterfaceButtonHandler instance = null;
    private static LevelCreator levelCreator;
    private PrefabsManager prefabsManager;
    public bool mouseOverUIElement;

    
    public static InterfaceButtonHandler GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        prefabsManager = PrefabsManager.GetInstance();
        levelCreator = LevelCreator.GetInstance();
    }

    public void PassObjectToPlace(string prefabId)
    {
        levelCreator.PassObjectToPlace(prefabId);
    }

    public void onDeleteSelection()
    {
        levelCreator.OnDeleteSelection();
    }

    public void onAddNewLevel(Transform newLevelPanel)
    {
        Transform widthPanel = newLevelPanel.Find("WidthField");
        Transform heightPanel = newLevelPanel.Find("HeightField");

        if (heightPanel == null) {
            Debug.Log("HeightField of NewLevelButton could not be found");
            return;
        }

        if (widthPanel == null)
        {
            Debug.Log("WidthField of NewLevelButton could not be found");
            return;
        }

        TMP_InputField widthInputField = widthPanel.GetComponentInChildren<TMP_InputField>();
        TMP_InputField heightInputField = heightPanel.GetComponentInChildren<TMP_InputField>();

        if (heightInputField == null)
        {
            Debug.Log("InputField component of heightField of NewLevelButton could not be found");
            return;
        }

        if (widthInputField == null)
        {
            Debug.Log("InputField component of widthField of NewLevelButton could not be found");
            return;
        }

        int height = int.Parse(heightInputField.text);
        int width = int.Parse(widthInputField.text);
        levelCreator.OnNewLevelSelection(width, height);
    }

    public void OnSaveLevel()
    {
        levelCreator.OnSaveLevelSelection();
    }

    public void OnLoadLevel()
    {
        levelCreator.OnLoadLevelSelection();
    }

    public void OnNewPatrolPointsSelection()
    {
        levelCreator.OnNewPatrolPointsSelection();
    }
}
