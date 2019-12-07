using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeapon : Weapon
{
    protected override void CreateProjectile() {
		GameObject projectileObject = Instantiate(projectilePrefab);
		BasicProjectile projectile = projectileObject.GetComponent<BasicProjectile>();
		if (this.person != null) {
			projectile.origin = this.person.gameObject;
		}
		float rotation = transform.eulerAngles.z;
		Vector2 direction = VectorUtil.Deg2Vector2(rotation);
		projectile.transform.position = transform.position;
		projectile.transform.rotation = transform.rotation;
		projectile.projectileRigidbody.velocity = direction * projectile.velocity;
	}
}
