using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public abstract class ProjectileBase : MonoBehaviour {
	public GameObject origin;
	public Rigidbody2D projectileRigidbody;
	public int damage;
	public float velocity;
	public float maxLiveTime = 3f;
    public BulletAudioComponent audioComponent;
	private Vector2 direction;
	public Vector2 Direction {
		get {
			return direction;
		}
		set {
			this.direction = value;
		}
	}

    private void Start() {
        StartCoroutine(WaitForSelfDestruct());
	}

	private void OnTriggerEnter2D(Collider2D collider) {
		HandleCollision(collider);
    }

	protected abstract void HandleCollision(Collider2D collider);

	protected IEnumerator WaitForSelfDestruct() {
		yield return new WaitForSeconds(maxLiveTime);
        Destroy(gameObject);
	}

    
}
