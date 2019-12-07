using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookForPlayerBehaviour : IPlayerFollowBehaviour {
	private IPlayerFollowBehaviour decoratedBehaviour;

	private bool isRotating = false;

	public LookForPlayerBehaviour(IPlayerFollowBehaviour decoratedBehaviour) {
		this.decoratedBehaviour = decoratedBehaviour;
	}

	public void FollowPlayer(Enemy enemy, Player player) {
		decoratedBehaviour.FollowPlayer(enemy, player);
		if (LostTrackOfPlayer() && isRotating == false) {
			isRotating = true;
			enemy.StartCoroutine(RotateToRandomDirection(enemy));
		}
	}

	public bool LostTrackOfPlayer() {
		return decoratedBehaviour.LostTrackOfPlayer();
	}

	public void Reset() {
		decoratedBehaviour.Reset();
		isRotating = false;
	}

	private IEnumerator RotateToRandomDirection(Enemy enemy) {
		float targetRotation = Random.value * 360;
		while (Mathf.Abs(enemy.transform.eulerAngles.z % 360 - targetRotation) > 1 && isRotating) {
			yield return new WaitForEndOfFrame();
			enemy.RotateEnemyToAngle(targetRotation);
		}
		isRotating = false;
	}
}
