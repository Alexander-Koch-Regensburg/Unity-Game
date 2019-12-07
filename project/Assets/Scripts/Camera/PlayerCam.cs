using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour {

	public static PlayerCam instance;

	/// <summary>
	/// Determines how smooth the Camera follows the player
	/// </summary>
	[Range(0f, 1f)]
	public float smoothness = 0.1f;

	private Player player = null;

	private Vector3 currentVelocity = Vector3.zero;

	void Awake() {
		instance = this;
	}

	void LateUpdate() {
		UpdatePos();
	}

	private void UpdatePos() {
		if (player == null) {
			return;
		}
		Vector3 playerPos = player.transform.position;
		Vector3 targetPos = new Vector3(playerPos.x, playerPos.y, transform.position.z);

		transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref currentVelocity, smoothness);
	}

	public void InitPos(Player player) {
		if (player == null) {
			return;
		}
		this.player = player;

		Vector3 playerPos = player.transform.position;
		transform.position = new Vector3(playerPos.x, playerPos.y, transform.position.z);
	}
}
