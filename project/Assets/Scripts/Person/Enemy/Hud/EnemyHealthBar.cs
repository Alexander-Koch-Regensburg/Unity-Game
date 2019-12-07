using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour {

	public int segmentSize;
	public Enemy enemy;

	private RectTransform rectTransform;

	private void Awake() {
		rectTransform = GetComponent<RectTransform>();
	}

    private void Update() {
		UpdateBarSize();
    }

	private void UpdateBarSize() {
		if (enemy.health <= 0) {
			rectTransform.sizeDelta = new Vector2(0, rectTransform.sizeDelta.y);
		} else {
			int newWidth = enemy.health * segmentSize;
			rectTransform.sizeDelta = new Vector2(newWidth, rectTransform.sizeDelta.y);
		}
	}
}
