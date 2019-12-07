using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ObjectEditorPanelManager : MonoBehaviour, IObjectEditorPanelManager
{
    public Transform objectTypeTextPanel;
    public Transform ammunitionPanel;
    public Transform patrolPointsPanel;
    public Transform patrolPointsContentPanel;

    private IList<GameObject> patrolPointsElements;
    public GameObject patrolPointPrefab;
    public Transform objectEditorPanel;

    private bool showAmmunitionPanel = false;
    public bool showObjectEditorPanel = false;
    private bool showPatrolPointEditorPanel = false;

    void Awake()
    {
        patrolPointsElements = new List<GameObject>();
    }

    public void SetupPanel(IPlacedObject placedObject)
    {
        DisablePanelElements();
        SetObjectEditorVariablesToFalse();
        SetObjectTypeText(placedObject);
        //DisableObjectEditorPanelElements();
        switch (placedObject.Type)
        {
            case PrefabType.LEVELELEMENT:
                break;
            case PrefabType.WEAPON:
                if (placedObject.PrefabId != "batPrefab")
                    showAmmunitionPanel = true;
                break;
            case PrefabType.PLAYER:
                break;
            case PrefabType.ENEMY:
                showPatrolPointEditorPanel = true;
                break;
            default:
                break;
        }
        SetupObjectEditorPanelElements(placedObject);
    }

    public void DisablePanelElements()
    {
        foreach (Transform child in objectEditorPanel)
        {
            if (child.gameObject.name != "ObjectText" && child.gameObject.name != "PanelText")
                child.gameObject.SetActive(false);
        }
    }

    private void SetObjectTypeText(IPlacedObject placedObject)
    {
        if (placedObject == null)
            return;
        TextMeshProUGUI textMeshPro = objectTypeTextPanel.gameObject.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = placedObject.Text;
    }

    private void SetObjectEditorVariablesToFalse()
    {
        showAmmunitionPanel = false;
        showPatrolPointEditorPanel = false;
    }

    private void SetupObjectEditorPanelElements(IPlacedObject placedObject)
    {
        if (showAmmunitionPanel)
            SetupAmmunitionPanelElement(placedObject);
        else
            ammunitionPanel.gameObject.SetActive(false);

        if (showPatrolPointEditorPanel)
            SetupEnemyPatrolPointsPanel(placedObject);
        else
            patrolPointsPanel.gameObject.SetActive(false);

    }

    private void SetupAmmunitionPanelElement(IPlacedObject placedObject)
    {
        Weapon weapon = placedObject.Prefab.GetComponent<Weapon>();
        ammunitionPanel.gameObject.SetActive(true);
        
        Transform inputFieldTansform = ammunitionPanel.Find("InputField");
        TMP_InputField textMeshPro = inputFieldTansform.gameObject.GetComponent<TMP_InputField>();

        string ammunitionText = weapon.GetAmmo().ToString();
        textMeshPro.text = ammunitionText;
    }

    private void SetupEnemyPatrolPointsPanel(IPlacedObject placedObject)
    {
        Enemy enemy = placedObject.Prefab.GetComponent<Enemy>();
        if (enemy == null)
            return;

        patrolPointsPanel.gameObject.SetActive(true);
        DestroyPatrolPointElements();

        IList<Vector2> patrolPoints = enemy.PatrolPoints;
        if (patrolPoints == null)
            return;

        SetupPatrolPointsPanel(patrolPoints);
    }

    private void SetupPatrolPointsPanel(IList<Vector2> patrolPoints)
    {
        int count = 1;
        patrolPointsElements = new List<GameObject>();
        foreach (Vector2 patrolPoint in patrolPoints)
        {
            GameObject patrolPointsElement = Instantiate(patrolPointPrefab, patrolPointsContentPanel);
            PatrolPointsPanelElement element = patrolPointsElement.AddComponent<PatrolPointsPanelElement>();

            element.Setup(count, patrolPoint);
            patrolPointsElements.Add(patrolPointsElement);
            count += 1;
        }
    }

    private void DestroyPatrolPointElements()
    {
        for (int i = 0; i < patrolPointsElements.Count; i++)
        {
            GameObject gObject = patrolPointsElements[i];
            Destroy(gObject);
        }
    }
}
