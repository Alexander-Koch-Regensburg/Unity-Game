using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AwarenessBar : MonoBehaviour {
	public float maxWidth;
	public Image awarenessBar;
	public EnemyAwarenessSystem awarenessSystem;

	private RectTransform rectTransform;

	private void Awake() {
		rectTransform = GetComponent<RectTransform>();
	}

	private void Update() {
		UpdateAwarenessBar();
		UpdateBarColor();
    }

	private void UpdateAwarenessBar() {
		if (awarenessSystem == null) {
			rectTransform.sizeDelta = new Vector2(0f, rectTransform.sizeDelta.y);
			return;
		}
		float awarenessPercent = awarenessSystem.Awareness / awarenessSystem.MaxAwareness;
		rectTransform.sizeDelta = new Vector2(maxWidth * awarenessPercent, rectTransform.sizeDelta.y);
	}

	private void UpdateBarColor() {
		if (awarenessSystem == null) {
			return;
		}
		if (awarenessSystem.Awareness >= awarenessSystem.attackThreshold) {
			awarenessBar.color = new Color(1f, 0f, 0f);
		} else {
			float ratioAttackThreshold = awarenessSystem.Awareness / awarenessSystem.attackThreshold;
			float red = 0.5f * ratioAttackThreshold;
			float blue = 1 / ratioAttackThreshold;
			awarenessBar.color = new Color(red, 0f, blue);
		}
	}
}
