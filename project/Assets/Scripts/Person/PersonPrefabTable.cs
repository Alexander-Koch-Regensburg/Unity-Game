using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonPrefabTable : MonoBehaviour {
	public static PersonPrefabTable instance;
	public Player playerPrefab;
	public Enemy basicEnemyPrefab;
	public Enemy easyEnemyPrefab;
	public Enemy hardEnemyPrefab;

	void Awake() {
		instance = this;
    }
}
