using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : FireArm
{
    public int spreadAngle = 13;
    public int projectilePelletCount = 5;
    private const float projectileOffset = 1.2f;
    private float livetime = 1;

    protected override void CreateProjectile()
    {   
        for (int i = 0; i <= projectilePelletCount; i++)
        {
            GameObject projectileObject = Instantiate(projectilePrefab);
            BasicProjectile projectile = projectileObject.GetComponent<BasicProjectile>();
            if (this.person != null)
            {
                projectile.origin = this.person.gameObject;
            }

            //float rotation = transform.eulerAngles.z;
            float rotation = -spreadAngle / 2 + (i / (float)projectilePelletCount) * spreadAngle + transform.eulerAngles.z;

            Vector2 direction = VectorUtil.Deg2Vector2(rotation);

            float spawnX = transform.position.x + direction.x * projectileOffset;
            float spawnY = transform.position.y + direction.y * projectileOffset;
            Vector2 spawnPos = new Vector2(spawnX, spawnY);
            projectile.transform.position = spawnPos;
            projectile.transform.rotation = transform.rotation;
            projectile.transform.Rotate(0, 0, rotation);
            projectile.projectileRigidbody.velocity = direction * projectile.velocity;

            Destroy(projectileObject, livetime);
        }

    }

}
