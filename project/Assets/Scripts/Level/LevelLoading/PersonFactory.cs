using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonFactory {

	public static Player InstantiatePlayer(PlayerData data, Vector2 spawnPosition, Transform parent) {
		PersonPrefabTable personPrefabTable = PersonPrefabTable.instance;
		if (personPrefabTable == null) {
			return null;
		}
		Player player = Object.Instantiate(personPrefabTable.playerPrefab, parent);
		SetPersonPosition(player, spawnPosition);
        if(MainMenuPlayerPreferences.LoadFromJson && data.weapon != WeaponType.INVALID) {
            SetPersonWeapon(player, data.weapon, data.ammo);
        }
		return player;
	}

	public static Enemy InstantiateEnemy(EnemyData data, Transform parent, bool showFieldOfView) {
		PersonPrefabTable personPrefabTable = PersonPrefabTable.instance;
		if (personPrefabTable == null) {
			return null;
		}
		Enemy enemy;
		IPlayerFollowBehaviour playerFollowBehaviour = new BasicPlayerFollowBehaviour();
		playerFollowBehaviour = new LookForPlayerBehaviour(playerFollowBehaviour);
		switch (data.type) {
			case EnemyType.BASIC:
				enemy = Object.Instantiate(personPrefabTable.basicEnemyPrefab, parent);
				playerFollowBehaviour = new SearchForPlayerBehaviour(playerFollowBehaviour, 4);
				break;
			case EnemyType.EASY:
				enemy = Object.Instantiate(personPrefabTable.easyEnemyPrefab, parent);
				break;
			case EnemyType.HARD:
				enemy = Object.Instantiate(personPrefabTable.hardEnemyPrefab, parent);
				playerFollowBehaviour = new SearchForPlayerBehaviour(playerFollowBehaviour, 2);
				break;
			default:
				return null;
		}
		enemy.Type = data.type;
		enemy.SetPlayerFollowBehaviour(playerFollowBehaviour);
		SetPersonPosition(enemy, data.position);
		SetPersonWeapon(enemy, data.weapon, data.ammo);
		enemy.PatrolPoints = data.patrolPoints;
		if (showFieldOfView) {
			enemy.VisualizeFieldOfView();
		}
		return enemy;
	}


	private static void SetPersonPosition(Person person, Vector2 position) {
		float x = position.x + 0.5f;
		float y = position.y + 0.5f;
		person.transform.position = new Vector3(x, y, 0);
	}

	private static void SetPersonWeapon(Person person, WeaponType weaponType, int ammo) {
		WeaponData weaponData = new WeaponData(new Vector2(0, 0), 0f, weaponType, ammo);
		var weapon = ItemFactory.InstantiateWeapon(weaponData, person.transform);
		if (weapon != null) {
			weapon.SetPerson(person);
		}
	}
}
