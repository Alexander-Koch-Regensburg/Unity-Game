using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWithRange : BasicProjectile {

	public float range = 5f;

    protected Vector2 startPosition;

    private void Start() {
        startPosition = transform.position;
		StartCoroutine(CheckDistanceTravelled());
	}

    protected IEnumerator CheckDistanceTravelled() {
        float distanceTravelled = Vector2.Distance(transform.position, startPosition);
        while (distanceTravelled < range) {
            distanceTravelled = Vector2.Distance(transform.position, startPosition);
			yield return new WaitForEndOfFrame();
		}
        Destroy(gameObject);
	}
}
