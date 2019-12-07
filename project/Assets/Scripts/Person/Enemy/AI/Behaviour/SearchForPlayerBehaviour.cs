using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchForPlayerBehaviour : IPlayerFollowBehaviour {

	private IPlayerFollowBehaviour decoratedBahaviour;
	private int searchAreaSize;

	private Vector2? nextSearchPoint;

	/// <summary>
	/// A Enemey with this behaviour will search the player after he lost track of him
	/// </summary>
	/// <param name="decoratedBahaviour"></param>
	/// <param name="searchAreaSize">How far away from the player the enemy will search at max</param>
	public SearchForPlayerBehaviour(IPlayerFollowBehaviour decoratedBahaviour, int searchAreaSize) {
		this.decoratedBahaviour = decoratedBahaviour;
		this.searchAreaSize = searchAreaSize;
	}

	public void FollowPlayer(Enemy enemy, Player player) {
		decoratedBahaviour.FollowPlayer(enemy, player);

		if (LostTrackOfPlayer() == false) {
			return;
		}
		if (nextSearchPoint == null || enemy.navAgent.Destination == null) {
			CalculateNextSearchPoint(player);
		}
		if (nextSearchPoint != null) {
			if (Vector2.Distance(enemy.transform.position, nextSearchPoint.Value) < 0.5f) {
				nextSearchPoint = null;
			}
		}
		enemy.navAgent.Destination = nextSearchPoint;
	}

	public bool LostTrackOfPlayer() {
		return decoratedBahaviour.LostTrackOfPlayer();
	}

	public void Reset() {
		decoratedBahaviour.Reset();
		nextSearchPoint = null;
	}

	private void CalculateNextSearchPoint(Player player) {
		IList<Vector2> tilesAroundPlayer = GetPositionsAroundPlayer(player);

		int chosenPoint = Random.Range(0, tilesAroundPlayer.Count);
		nextSearchPoint = tilesAroundPlayer[chosenPoint];
	}

	private IList<Vector2> GetPositionsAroundPlayer(Player player) {
		IList<Vector2> ret = new List<Vector2>();
		LevelController levelController = LevelController.instance;
		if (levelController == null) {
			return ret;
		}
		Vector2 playerPos = player.transform.position;
		Vector2Int squarePos = new Vector2Int(Mathf.Max((int) (playerPos.x - searchAreaSize), 0), Mathf.Max((int) (playerPos.y - searchAreaSize), 0));
		Vector2Int levelSize = levelController.GetLevelSize();
		Vector2Int squareSize = new Vector2Int(Mathf.Min(searchAreaSize * 2 + 1, levelSize.x), Mathf.Min(searchAreaSize * 2 + 1, levelSize.y));
		ISet<ILevelTile> tiles = levelController.GetTilesInRectangle(squarePos, squareSize);
		if (tiles == null) {
			return ret;
		}
		foreach (ILevelTile tile in tiles) {
			if (tile.IsTraversable()) {
				ret.Add(tile.CenterPos);
			}
		}

		return ret;
	}
}
