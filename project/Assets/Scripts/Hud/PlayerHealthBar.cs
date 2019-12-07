using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour {
	public PersonController personController;
	public Image healthBar;
	public int segmentSize = 32;

	private Color healthHighColor = Color.white;
	private Color healthMediumColor = new Color(1f, .5f, .5f);
	private Color healthLowColor = new Color(1f, 0f, 0f);

	private RectTransform rectTransform;

	private void Start() {
		rectTransform = GetComponent<RectTransform>();
	}

	private void Update() {
		UpdateBarSize();
		UpdateBarColor();
	}

	/// <summary>
	/// Updates the healthbar size, so it displays a health-segment for 1 player health
	/// </summary>
	private void UpdateBarSize() {
		Player player = personController.player;
		if (player == null) {
			rectTransform.sizeDelta = new Vector2(0f, rectTransform.sizeDelta.y);
		} else {
			int width = Mathf.Max(0, player.health) * segmentSize;
			rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);
		}
	}

	private void UpdateBarColor() {
		Player player = personController.player;
		if (player == null || player.health > 3) {
			healthBar.color = healthHighColor;
		} else if (player.health > 1) {
			healthBar.color = healthMediumColor;
		} else {
			healthBar.color = healthLowColor;
		}
	}
}
