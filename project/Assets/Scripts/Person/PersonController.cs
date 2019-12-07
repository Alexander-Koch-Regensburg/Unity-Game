using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonController : MonoBehaviour {

	public static PersonController instance;

	public Player player;

	private bool showEnemyFieldOfView;

	private ISet<Enemy> enemies;
	/// <summary>
	/// All enemies in the level
	/// </summary>
	public ISet<Enemy> Enemies {
		get {
			return enemies;
		}
	}

	private IList<Enemy> hardEnemies;
	public IList<Enemy> HardEnemies {
		get {
			return hardEnemies;
		}
	}

    void Awake() {
		instance = this;
		enemies = new HashSet<Enemy>();
		hardEnemies = new List<Enemy>();
    }

	void Start() {
		showEnemyFieldOfView = MainMenuPlayerPreferences.EnableFOV;
	}

	public void SpawnPlayer(PlayerData data, Vector2 spawnPosition) {
		player = PersonFactory.InstantiatePlayer(data, spawnPosition, transform);
		player.OnPersonDied += EndGameScreen.instance.SetGameLost;
	}

	public IList<Enemy> SpawnEnemies(IList<EnemyData> data) {
        IList<Enemy> spawnedEnemies = new List<Enemy>();
		foreach (EnemyData element in data) {
			Enemy enemy = PersonFactory.InstantiateEnemy(element, transform, showEnemyFieldOfView);
            spawnedEnemies.Add(enemy);
			enemies.Add(enemy);
			enemy.OnPersonDied += () => enemies.Remove(enemy);
			if (element.type == EnemyType.HARD) {
				hardEnemies.Add(enemy);
			}
		}
        return spawnedEnemies;
	}

	public void KillEnemy(Enemy enemy) {
		StartCoroutine(FadeOut(enemy));
	}

	public Enemy GetEnemyById(string ID) {
		foreach (Enemy enemy in enemies) {
			if (enemy.ID == ID) {
				return enemy;
			}
		}
		return null;
	}

	IEnumerator FadeOut(Enemy enemy) {

		Material enemyMaterial = enemy.transform.Find("Texture").GetComponent<SpriteRenderer>().material;
		Color startColor = enemyMaterial.color;
        Color targetColor = startColor;
		targetColor.a = 0;
		float startTime = Time.time;
		float duration = 0f;

		while (duration < enemy.fadeout && enemy.gameObject)
		{
			yield return new WaitForEndOfFrame();
			duration = Time.time - startTime;
			enemyMaterial.color = Color.Lerp(startColor, targetColor, duration / enemy.fadeout);
		}
		Destroy(enemy.gameObject);
		yield break;
     }
}
