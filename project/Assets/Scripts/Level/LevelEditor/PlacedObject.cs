using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObject : MonoBehaviour, IInteractable, IPlacedObject
{
    private static int nextObjectId = 1;
    private static float maxInteractionDistance = 2f;

    public GameObject prefab;
    public PrefabType type;
    public string text;
    public int objectId;
    public string prefabId;
    public bool isProtectedObject;
    private IPlacedObject personWhichBelongsTo = null;

    /// <summary>
    /// The LevelTile the placedObject is associated to
    /// </summary>
    public IList<ILevelTile> levelTiles = null;


    public GameObject Prefab {
        get {
            return prefab;
        }
        set
        {
            prefab = value;
        }
    }
    public PrefabType Type {
        get {
            return type;
        }
    }

    public string Text {
        get {
            return text;
        }
    }
    public int ObjectId {
        get {
            return objectId;
        }
    }
    public string PrefabId {
        get {
            return prefabId;
        }
    }
    public bool IsProtectedObject {
        get {
            return isProtectedObject;
        }
        set
        {
            isProtectedObject = value;
        }
    }
    public IList<ILevelTile> LevelTiles {
        get {
            return levelTiles;
        }
        set {
            levelTiles = value;
        }
    }

    public void SetupPrefab(GameObject prefab, string prefabId, string name, PrefabType type, bool isProtectedObject)
    {
        this.prefab = prefab;
        this.type = type;
        this.text = name;
        this.prefabId = prefabId;
        this.isProtectedObject = isProtectedObject;
    }

    public void SetupInstantiatedObject(IList<ILevelTile> levelTiles, Transform parent)
    {
        this.levelTiles = levelTiles;
        transform.SetParent(parent);
        transform.position = (Vector2)prefab.transform.position;
        prefab.transform.SetParent(this.gameObject.transform);
        prefab.transform.localPosition = new Vector3(0, 0, 0);

        DisableComponents();
        AddCollidersOfChild();
        SetObjectId();
    }

    public void SetupObjectToPlace()
    {
        transform.position = new Vector2(-10, -10);
        prefab.transform.localPosition = new Vector3(0, 0, 0);

        if (type == PrefabType.LEVELELEMENT)
        {
            Wall wall = prefab.GetComponent<Wall>();
            if (wall != null)
            {
                prefab.transform.position = this.transform.position;
                //wall.RemoveFromTilemap();
            }
        }
        DisableComponents();
    }


    public void DisableComponents()
    {
        foreach (Behaviour childComponent in prefab.GetComponentsInChildren<Behaviour>())
        {
            if (childComponent is Animator)
                continue;
            childComponent.enabled = false;
        }
    }

    public void SetAllCollidersStatus(bool active)
    {
        foreach (Component c in GetComponentsInParent<Component>())
        {
            if (c is BoxCollider2D)
            {
                ((BoxCollider2D)c).enabled = active;
            }

            else if (c is CircleCollider2D)
            {
                ((CircleCollider2D)c).enabled = active;
            }
            else if (c is CapsuleCollider2D)
            {
                ((CapsuleCollider2D)c).enabled = active;
            }
        }
    }

    public void SetProtectedObject(bool isProtectedObject)
    {
        this.isProtectedObject = isProtectedObject;
    }

    private Vector2 RotateVector(Vector2 v, float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        float _x = v.x * Mathf.Cos(radian) - v.y * Mathf.Sin(radian);
        float _y = v.x * Mathf.Sin(radian) + v.y * Mathf.Cos(radian);
        return new Vector2(Mathf.Abs(_x), Mathf.Abs(_y));
    }

    public void AddCollidersOfChild()
    {
        foreach (Component childComponent in prefab.GetComponents<Component>())
        {
            if (childComponent is BoxCollider2D)
            {
                BoxCollider2D collider = this.gameObject.AddComponent<BoxCollider2D>();
                BoxCollider2D childCollider = (BoxCollider2D)childComponent;
                collider.size = RotateVector(childCollider.size, childCollider.gameObject.transform.eulerAngles.magnitude);
                collider.offset = RotateVector(childCollider.offset, childCollider.gameObject.transform.eulerAngles.magnitude);

                Door door = prefab.GetComponent<Door>();
                if (door != null)
                    return;
            }

            else if (childComponent is CircleCollider2D)
            {
                CircleCollider2D collider = this.gameObject.AddComponent<CircleCollider2D>();
                CircleCollider2D childCollider = (CircleCollider2D)childComponent;
                collider.radius = childCollider.radius;
                collider.offset = childCollider.offset;
            }
            else if (childComponent is CapsuleCollider2D)
            {
                CapsuleCollider2D collider = this.gameObject.AddComponent<CapsuleCollider2D>();
                CapsuleCollider2D childCollider = (CapsuleCollider2D)childComponent;
                collider.size = childCollider.size;
                collider.offset = childCollider.offset;
                collider.direction = childCollider.direction;
            }
        }
    }
    
    public bool BelongsToParentObject()
    {
        if (this.personWhichBelongsTo != null)
            return true;
        return false;
    }

    public void SetObjectId()
    {
        objectId = nextObjectId;
        nextObjectId = nextObjectId + 1;
    }
    
    public void SetHighlight(bool highlighted)
    {
        if (prefab == null)
            return;

        SpriteRenderer sprite = prefab.GetComponentInChildren<SpriteRenderer>();
        if (sprite == null)
        {
            return;
        }
        if (highlighted)
        {
            LevelCreator.GetInstance().stateInformation.hoveredObject = this;
            sprite.color = Color.cyan;
        }
        else
        {
            LevelCreator.GetInstance().stateInformation.hoveredObject = null;
            sprite.color = Color.white;
        }
    }

    public bool CanInteract(Vector3 position)
    {
        if (position == null)
        {
            return false;
        }
        float distance = (prefab.transform.position - position).magnitude;
        return (distance <= GetMaxInteractionDistance());
    }

    public void Interact(Person person)
    {
        PlacedObject parentObject = person.gameObject.transform.parent.GetComponent<PlacedObject>();
        SetWeaponToPerson(parentObject);
    }

    private void SetWeaponToPerson(PlacedObject personObject)
    {
        // Remove weapons of person
        IList<Weapon> prevWeapons = personObject.GetComponentsInChildren<Weapon>();
        foreach (Weapon prevWeapon in prevWeapons)
        {
            if (prevWeapon is Fist)
                continue;

            PlacedObject weaponObject = prevWeapon.gameObject.transform.parent.GetComponent<PlacedObject>();
            if (weaponObject != null)
                weaponObject.RemoveWeaponFromPerson();
        }

        IWeapon weapon = (IWeapon) this.prefab.GetComponent<Weapon>();
        Person person = personObject.prefab.GetComponent<Person>();
        transform.SetParent(personObject.transform);
        transform.position = person.GetWeaponAnchorPos();
        transform.rotation = person.transform.rotation;

        weapon.SetSortingLayerOfTextureOfWeapon(SortingLayerConstants.WEAPON_LAYER_NAME);
        person.Weapon = (Weapon)weapon;

        weapon.SetAnimatorOnFloorFlag(false);
        this.personWhichBelongsTo = personObject;
    }

    public void RemoveWeaponFromPerson()
    {
        if (personWhichBelongsTo == null)
            return;

        Weapon weapon = this.prefab.GetComponent<Weapon>();
        if (weapon == null)
            return;

        Person person = personWhichBelongsTo.Prefab.GetComponent<Person>();
        if (person == null)
            return;

        transform.SetParent(LevelCreator.GetInstance().transform);
        weapon.SetSortingLayerOfTextureOfWeapon(SortingLayerConstants.ITEM_LAYER_NAME);
        weapon.SetAnimatorOnFloorFlag(true);
        personWhichBelongsTo.SetAllCollidersStatus(true);
        this.personWhichBelongsTo = null;
        person.Weapon = null;   
    }

    public float GetMaxInteractionDistance()
    {
        return maxInteractionDistance;
    }

    public string GetID()
    {
        return null;
    }
}
