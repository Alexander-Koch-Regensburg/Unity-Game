using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData {
    public EnemyType type;
    public IList<Vector2> patrolPoints;
	public WeaponType weapon;
	public int ammo;
    public Vector2 position;

	public EnemyData(EnemyType type, IList<Vector2> patrolPoints, WeaponType weapon, int ammo, Vector2 position) {
		this.type = type;
        this.patrolPoints = patrolPoints;
        this.weapon = weapon;
		this.ammo = ammo;
        this.position = position;
	}
}
