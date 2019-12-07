using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public abstract class Weapon : Item, IWeapon, IInteractable {

	/// <summary>
	/// The weapons firerate in shots/attacks per second
	/// </summary>
	public float fireRate;
	public GameObject projectilePrefab;
	protected bool onCooldown = false;

	/// <summary>
	/// The animator for this weapon's texture
	/// </summary>
	public Animator animator;

	public SpriteRenderer sprite;
	private const string animatorOnFloorFlag = "On_Floor";

    protected int ammo = 10;

    public WeaponAudioComponent audioComponent;

    protected Person person;

	private SpriteRenderer texture;

	private string id = Guid.NewGuid().ToString();

	private void Awake() {
		texture = GetComponentInChildren<SpriteRenderer>();
	}

	public int GetAmmo() {
		return ammo;
	}

	public Person GetPerson() {
		return person;
	}

	public void SetAmmo(int ammo) {
		this.ammo = ammo;
	}

    public Vector2 getActualPosition() {
        return transform.position;
    }

	public virtual void SetPerson(Person person) {
		if (person == null) {
			transform.SetParent(LevelController.instance.transform);

			if (texture != null) {
				texture.sortingLayerName = SortingLayerConstants.ITEM_LAYER_NAME;
			}

			if (animator != null) {
				animator.SetBool(animatorOnFloorFlag, true);
			}
			this.person = null;
		} else {
			transform.SetParent(person.transform);
			transform.position = person.GetWeaponAnchorPos();
			transform.rotation = person.transform.rotation;

			if (texture != null) {
				texture.sortingLayerName = SortingLayerConstants.WEAPON_LAYER_NAME;
			}

			IWeapon personCurrentWeapon = person.Weapon;
			if (personCurrentWeapon != null) {
				personCurrentWeapon.SetPerson(null);
			}
			person.Weapon = this;

			if (animator != null) {
				animator.SetBool(animatorOnFloorFlag, false);
			}
			this.person = person;
		}
	}

	public virtual void Fire() {
		if (onCooldown || InGameMenu.gameIsPaused) {
			return;
		}

		if (ammo > 0) {
			CreateProjectile();
			ammo--;
		}
        
		PlayAudio();        
		StartCoroutine(WaitForCooldown());
	}

	public float GetProjectileVelocity() {
		return projectilePrefab.GetComponentInChildren<ProjectileBase>(true).velocity;
	}

	public int GetProjectileDamage() {
		return projectilePrefab.GetComponentInChildren<ProjectileBase>(true).damage;
	}

	protected IEnumerator WaitForCooldown() {
		onCooldown = true;
		float waitTime = 1 / fireRate;
		yield return new WaitForSeconds(waitTime);
		onCooldown = false;
	}

	protected abstract void CreateProjectile();

	protected void PlayAudio() {
		if (audioComponent == null) {
			return;
		}
		if (ammo == 0) {
			audioComponent.Play("onNoAmmoFiring");
		} else {
			audioComponent.Play("onFiring");
		}
	}

    public void SetAnimatorOnFloorFlag(bool active)
    {
        if (animator != null)
            animator.SetBool(animatorOnFloorFlag, active);
    }

    public void SetSortingLayerOfTextureOfWeapon(string sortinglayerConstant)
    {
        if (texture != null)
            texture.sortingLayerName = sortinglayerConstant;
    }

    public WeaponAudioComponent GetAudioComponent ()
    {
        return audioComponent;
    }

    public void SetHighlight(bool highlighted) {
		if (sprite == null) {
			return;
		}
		if (highlighted) {
			if (person == null) {
				sprite.color = Color.cyan;
			}
		} else {
			sprite.color = Color.white;
		}
	}

	public bool CanInteract(Vector3 position) {
		if (this.person != null) {
			return false;
		}
		if (position == null) {
			return false;
		}
		float distance = (transform.position - position).magnitude;
		return (distance <= GetMaxInteractionDistance());
	}

	public void Interact(Person person) {
		if (CanInteract(person.transform.position)) {
			SetPerson(person);
		}
	}

	public float GetMaxInteractionDistance() {
		return 2f;
	}

	public string GetID() {
		return id;
	}
}
