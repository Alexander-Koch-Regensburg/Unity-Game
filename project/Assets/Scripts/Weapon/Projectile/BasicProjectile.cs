using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : ProjectileBase {

    protected override void HandleCollision(Collider2D collider) {
		if (collider.isTrigger) {
			//Ignore if other collider is also a trigger
			return;
		}

		if (origin == null || collider.gameObject == origin) {
			return;
		}
        
        IDamageable damageable = collider.gameObject.GetComponent<IDamageable>();
        Vector2 position = collider.gameObject.transform.position;
        
        if (damageable != null)
        {
            damageable.Damage(damage, origin);
            audioComponent.Play2DClipAtPoint("onPersonCollision", position);
        }
        else
        {
            audioComponent.Play2DClipAtPoint("onEnvironmentCollision", position);
        }
        Destroy(gameObject);
    }
}
