using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : FireArm {

    private const float projectileOffset = 1.0f;
    private int projectileShootNumber = 1;
    private const float projectileDisplacementFactor = 0.15f;

    protected override void CreateProjectile()
    {
        GameObject projectileObject = Instantiate(projectilePrefab);
        BasicProjectile projectile = projectileObject.GetComponent<BasicProjectile>();
        if (this.person != null)
        {
            projectile.origin = this.person.gameObject;
        }

        float rotation = transform.eulerAngles.z;
        Vector2 direction = VectorUtil.Deg2Vector2(rotation);

        float spawnX = transform.position.x + direction.x * projectileOffset + GetProjectileShootDisplacementValue();
        float spawnY = transform.position.y + direction.y * projectileOffset + GetProjectileShootDisplacementValue();
        
        Vector3 spawnPos = new Vector3(spawnX, spawnY, transform.position.z);
        projectile.transform.position = spawnPos;
        projectile.transform.rotation = transform.rotation;

        projectile.projectileRigidbody.velocity = direction * projectile.velocity;
    }

    protected float GetProjectileShootDisplacementValue()
    {
        float displacementFactor;

        if (projectileShootNumber == 1)
            displacementFactor = projectileDisplacementFactor;
        else if (projectileShootNumber == 2)
            displacementFactor = 0;
        else if (projectileShootNumber == 3)
            displacementFactor = -1.0f * projectileDisplacementFactor;
        else
            displacementFactor = 0;

        projectileShootNumber += 1;
        if (projectileShootNumber % 4 == 0)
            projectileShootNumber = 1;

        return displacementFactor;

    }

}
