using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemiesLeftDisplay : MonoBehaviour {

	private const string TEXT_START = "NPCs: ";

	public TMP_Text enemiesLeftText;

	private PersonController personController;

	private void Start()	{
		personController = PersonController.instance;
		enemiesLeftText.text = TEXT_START + "0";
    }

	private void Update() {
		UpdateText();
    }

	private void UpdateText() {
		if (personController == null) {
			return;
		}

		int enemyCount = personController.Enemies.Count;
		enemiesLeftText.text = TEXT_START + enemyCount;
	}
}
