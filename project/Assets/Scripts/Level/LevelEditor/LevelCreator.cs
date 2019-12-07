using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SFB;

public class LevelCreator : MonoBehaviour
{
    private static LevelCreator instance;
    private Dictionary<int, IPlacedObject> inScenePlacedObjects = new Dictionary<int, IPlacedObject>();
    public InteractableProximityChecker proximityChecker;

    private Vector2 mousePosition;

    public GameObject mouseButtonPrefab;
    public int initialLevelWidth = 30;
    public int initialLevelHeight = 30;
    public Sprite wallSprite;

    public StateInformation stateInformation = null;
    private State state;

    private Level level;
    private static ILevelController levelController;
    public static IInterfaceManager interfaceManager;
    public static ILevelDataManager levelDataManager;
    public static IPrefabsManager prefabsManager;
    private IStartupModule startupModule;
    private IObjectDeletionModule objectDeletionModule;
    private IObjectPlacementModule objectPlacementModule;
    private IObjectEditingModule objectEditingModule;
    private IObjectInformationModule objectInformationModule;

    private float cameraDragTime = 0.2f;
    private float mouseButtonLeftPressedTime = 0.12f;

    private MouseButton leftMouseButton;
    private MouseButton rightMouseButton;

    public static LevelCreator GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        MainMenuPlayerPreferences.LoadFromJson = false;
        MainMenuPlayerPreferences.InLevelCreationMode = true;
        instance = this;
    }

    void Start()
    {
        stateInformation = new StateInformation();
        interfaceManager = InterfaceManager.GetInstance();
        prefabsManager = PrefabsManager.GetInstance();
        levelController = LevelController.instance;
        levelDataManager = LevelDataManager.GetInstance();

        startupModule = (IStartupModule) StartupModule.GetInstance();
        objectDeletionModule = (IObjectDeletionModule) ObjectDeletionModule.GetInstance();
        objectPlacementModule = (IObjectPlacementModule) ObjectPlacementModule.GetInstance();
        objectEditingModule = (IObjectEditingModule) ObjectEditingModule.GetInstance();
        objectInformationModule = (IObjectInformationModule) ObjectInformationModule.GetInstance();


        GameObject leftMouseButtonObject = Instantiate(mouseButtonPrefab, transform);
        leftMouseButton = leftMouseButtonObject.GetComponent<MouseButton>(); 
        GameObject rightMouseButtonObject = Instantiate(mouseButtonPrefab, transform);
        rightMouseButton = rightMouseButtonObject.GetComponent<MouseButton>();

        leftMouseButton.Setup(0, mouseButtonLeftPressedTime);
        rightMouseButton.Setup(1, cameraDragTime);

        prefabsManager.AddPrefabs();
        interfaceManager.SetupUI(prefabsManager.GetPrefabTable());

        InitializeLevel(initialLevelWidth, initialLevelHeight);
        SetupLevel();
    }

    private void InitializeLevel(int width, int height)
    {
        level = startupModule.InitializeLevel(width, height);
    }

    private void SetupLevel()
    {
        SetupModules();
        levelDataManager.InitializeLevelData();
        levelController.SetLevel(level);
        objectDeletionModule.SetLevel(level);
        objectPlacementModule.SetLevel(level);
        objectInformationModule.SetLevel(level);
        startupModule.SetLevel(level);

        IPlacedObject wallPrefabObject = prefabsManager.GetInstantiatedPrefab("wallPrefab", transform);
        startupModule.SetupLevel(wallPrefabObject);
        objectDeletionModule.DestroyPlacedObject(wallPrefabObject);
    }

    private void SetupModules()
    {
        startupModule.Setup(objectDeletionModule, objectPlacementModule, objectInformationModule, objectEditingModule, levelDataManager, levelController, prefabsManager);
        objectDeletionModule.Setup(objectInformationModule, inScenePlacedObjects, levelDataManager);
        objectPlacementModule.Setup(inScenePlacedObjects, objectDeletionModule, objectInformationModule, wallSprite, levelDataManager);
        objectEditingModule.Setup(objectInformationModule);
        objectInformationModule.Setup(inScenePlacedObjects);
    }

    private void SwitchState(State state)
    {
        State newState = state;

        if (this.state != null && state != null)
        {
            Type currentType = this.state.GetType();
            Type newType = state.GetType();

            if (currentType == newType)
            {
                if (state is ObjectDeletionState)
                {
                    newState = null;
                }
            }
        }

        if (newState is ObjectEditingState)
            SetUpObjectEditingMode();
        else if (newState is PatrolPointState)
        {

        }
        else if (this.state is ObjectEditingState || this.state is PatrolPointState)
            DisableObjectEditorPanel();

        if (newState is ObjectDeletionState)
            ResetStateInformation();

        else if (newState == null)
        {
            ResetStateInformation();
            interfaceManager.ResetButtonPressedColor();
        }

        this.state = newState;
    }

    private void ResetStateInformation()
    {
        objectDeletionModule.DestroyPlacedObject(stateInformation.objectToPlace);
        stateInformation.objectToPlace = null;
        stateInformation.hoveredObject = null;
        stateInformation.selectedObject = null;
        stateInformation.objectPlacingId = "";
    }

    void Update()
    {
        if (InGameMenu.gameIsPaused)
            return;

        rightMouseButton.DetermineMouseButtonState();
        leftMouseButton.DetermineMouseButtonState();

        UpdateMousePosition();
        if (!interfaceManager.MouseOverUIElement)
            DetermineCurrentState();
        
        if (stateInformation.objectToPlace != null)
            MoveObjectToPlaceToMousePosition();

        HandleLeftMouseButton();
    }

    private void HandleLeftMouseButton()
    {
        if (leftMouseButton.GetState() != MouseButtonState.NONE && !interfaceManager.MouseOverUIElement)
        {
            if (state == null)
                return;
            state.OnMouseButton(mousePosition, stateInformation);
        }
    }

    private void SetUpObjectEditingMode()
    {
        interfaceManager.EditorPanelVisible = true;
        interfaceManager.SetupObjectEditorPanel(stateInformation.hoveredObject);
        if (stateInformation.selectedObject != null)
            stateInformation.selectedObject.SetAllCollidersStatus(true);
        stateInformation.selectedObject = stateInformation.hoveredObject;
    }

    private void DisableObjectEditorPanel()
    {
        interfaceManager.EditorPanelVisible = false;
        if (stateInformation.selectedObject != null)
            stateInformation.selectedObject.SetAllCollidersStatus(true);
        else
            // If this case is true, it might be a bug, so log this case for developer
            Debug.Log("Warning: selectedObject == null");
        stateInformation.selectedObject = null;
    }

    private void DetermineCurrentState()
    {
        bool rightMouseClick = CheckForRightMouseClick();
        if (rightMouseClick)
        {
            SwitchState(null);
            return;
        }

        bool objectEditingStateSwitch = CheckObjectEditingStateSwitch();
        if (objectEditingStateSwitch)
        {
            SwitchState(new ObjectEditingState(leftMouseButton, objectEditingModule, objectInformationModule));
            return;
        }

        bool nullStateSwitch = CheckForNullStateSwitch();
        if (nullStateSwitch)
        {
            SwitchState(null);
            return;
        }
    }

    private bool CheckForRightMouseClick()
    {
        if (rightMouseButton.GetState() == MouseButtonState.CLICKED)
            return true;
        return false;
    }

    private bool CheckObjectEditingStateSwitch()
    {
        if (state is ObjectPlacementState || state is ObjectDeletionState || state is PatrolPointState)
            return false;

        if (leftMouseButton.GetState() != MouseButtonState.DOWN)
            return false;

        if (state != null && !(state is ObjectEditingState))
            return false;

        if (stateInformation.hoveredObject == null || stateInformation.hoveredObject.IsProtectedObject)
            return false;

        return true;
    }

    private bool CheckForNullStateSwitch()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (state is ObjectPlacementState || state is ObjectDeletionState || state is PatrolPointState || stateInformation.hoveredObject != null)
            {
                return false;
            }
            else
                return true;
        }
        return false;
    }

    public void OnAmmunitionChanged(int ammunition)
    {
        objectEditingModule.ChangeAmmunitionAmount(stateInformation.selectedObject, ammunition);
    }

    public void OnDeleteSelection()
    {
        SwitchState(new ObjectDeletionState(objectDeletionModule, leftMouseButton));
    }

    public void OnNewLevelSelection(int width, int height)
    {
        ResetLevel();
        InitializeLevel(width, height);
        SetupLevel();
    }

    public void OnNewPatrolPointsSelection()
    {
        objectEditingModule.ResetPatrolPoints(stateInformation.selectedObject);
        interfaceManager.SetupObjectEditorPanel(stateInformation.selectedObject);
        SwitchState(new PatrolPointState(objectEditingModule, leftMouseButton, interfaceManager));
    }

    private void ResetLevel()
    {
        SwitchState(null);
        if (level != null)
        {
            objectDeletionModule.DeleteInScenePlacedObjects();
            ResetStateInformation();
        }
    }

    public void OnSaveLevelSelection()
    {
        SwitchState(null);

        bool playerExists = objectInformationModule.CheckIfPlayerExists();
        bool winConditionCanBeSatisfied = objectInformationModule.CheckIfEndZoneOrEnemyExists(inScenePlacedObjects);

        if (!playerExists || !winConditionCanBeSatisfied)
            return;

        levelDataManager.SavePlacedObjects(inScenePlacedObjects, level);
        levelController.SetLevelDataCollection(levelDataManager.GetLevelDataCollection());
        levelController.SaveLevel(true);
    }

    public void OnLoadLevelSelection()
    {
        SwitchState(null);
        bool levelSelected = ShowLoadGameWindow();
        if (!levelSelected)
            return;
        ResetLevel();
        levelController.LoadLevel();
        level = levelController.GetLevel();
        LevelDataCollection levelData = levelController.GetLevelDataCollection();
        levelController.SetLevelDataCollection(null);

        SetupLevel();
        startupModule.ConvertGameObjects(levelData);
    }

    private bool ShowLoadGameWindow()
    {
        string path = StandaloneFileBrowser.OpenFilePanel("Select json file to load.", "", "json", false)[0];

        if (path.Length != 0)
        {
            MainMenuPlayerPreferences.LoadFromJson = true;
            MainMenuPlayerPreferences.PathToLoad = path;
            return true;
        }
        else
        {
            Debug.LogError("You have to choose a valid directory. Could not load current game.");
            return false;
        }
    }

    private void UpdateMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            mousePosition = hit.point;
        }
    }

    private void MoveObjectToPlaceToMousePosition()
    {
        if (stateInformation.objectToPlace.Type == PrefabType.LEVELELEMENT)
        {
            Vector2 offset = objectInformationModule.GetOffset(stateInformation.objectToPlace);
            ILevelTile levelTile = level.GetTileAtPos(mousePosition);
            Vector2 position = (Vector2)levelTile.Pos;
            ((PlacedObject)stateInformation.objectToPlace).transform.position = position + offset;
        }
        else
            ((PlacedObject)stateInformation.objectToPlace).transform.position = mousePosition;
    }

    public void PassObjectToPlace(string prefabId)
    {
        objectDeletionModule.DestroyPlacedObject(stateInformation.objectToPlace);
        // Deselection of active button
        if (state is ObjectPlacementState && stateInformation.objectPlacingId == prefabId)
        {
            stateInformation.objectPlacingId = "";
            stateInformation.objectToPlace = null;
            SwitchState(null);
        }
        else
        {
            stateInformation.objectToPlace = prefabsManager.GetInstantiatedPrefab(prefabId, transform);
            stateInformation.objectPlacingId = prefabId;
            objectPlacementModule.SetWallSprite(stateInformation.objectToPlace);
            SwitchState(new ObjectPlacementState(objectPlacementModule, leftMouseButton));
        }
    } 

    public Dictionary<int, IPlacedObject> GetInScenePlacedObjects()
    {
        return inScenePlacedObjects;
    }
}
