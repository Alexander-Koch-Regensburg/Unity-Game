using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZone : RectLevelElement {

    public EndZone ()
    {
        this.size = new Vector2Int(2, 2);
    }

	public override bool IsSolid() {
		return false;
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.GetComponent<Player>() != null) {
			EndGameScreen.instance.SetGameWon();
		}
	}
}
