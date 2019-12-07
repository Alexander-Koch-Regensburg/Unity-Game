using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : FireArm {
	private const float projectileOffset = 0.5f;

	protected override void CreateProjectile() {
		GameObject projectileObject = Instantiate(projectilePrefab);
		BasicProjectile projectile = projectileObject.GetComponent<BasicProjectile>();
		if (this.person != null) {
			projectile.origin = this.person.gameObject;
		}

		float rotation = transform.eulerAngles.z;
		Vector2 direction = VectorUtil.Deg2Vector2(rotation);
		projectile.Direction = direction;

		float spawnX = transform.position.x + direction.x * projectileOffset;
		float spawnY = transform.position.y + direction.y * projectileOffset;
		Vector3 spawnPos = new Vector3(spawnX, spawnY, transform.position.z);
		projectile.transform.position = spawnPos;
		projectile.transform.rotation = transform.rotation;

		projectile.projectileRigidbody.velocity = direction * projectile.velocity;
	}

}
