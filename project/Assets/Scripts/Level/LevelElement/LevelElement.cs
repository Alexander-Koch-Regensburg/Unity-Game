using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A set element of a level placed on specific tiles
/// </summary>
public abstract class LevelElement : MonoBehaviour, ILevelElement {
	private Vector2Int pos;

    /// <summary>
    /// A 2D-vector representing the lower left position of the <c>LevelElement</c>
    /// </summary>
    public Vector2Int Pos {
		get {
            return pos;
        }
		set {
            if (pos == null) {
                pos = value;
                return;
            }
            if (pos == value) {
                return;
            }
            int deltaX = value.x - pos.x;
            int deltaY = value.y - pos.y;
            pos = value;
            MoveElement(new Vector2Int(deltaX, deltaY));
		}
	}

    public Vector2Int getActualPosition() {
        return new Vector2Int((int)transform.position.x, (int)transform.position.y);
    }

	/// <summary>
	/// Moves the <c>LevelElement</c> by a delta and sets all <c>Tile</c>-references correspondingly
	/// </summary>
	/// <param name="delta"></param>
	private void MoveElement(Vector2Int delta) {
		ISet<ILevelTile> oldTiles = GetTiles();
		if (oldTiles == null) {
			return;
		}
		if (oldTiles.Count == 0) {
			return;
		}
		ISet<ILevelTile> newTiles = new HashSet<ILevelTile>();
		foreach (LevelTile tile in oldTiles) {
			tile.RemoveLevelElement(this);

			Vector2Int newTilePos = new Vector2Int(tile.Pos.x + delta.x, tile.Pos.y + delta.y);
            ILevelTile newTile = LevelController.instance.GetTileAtWorldPos(newTilePos);
			if (newTile == null) {
				continue;
			}
			newTiles.Add(newTile);
			newTile.AddLevelElement(this);
		}
		oldTiles.Clear();
		oldTiles.UnionWith(newTiles);
	}

	/// <summary>
	/// Inits this LevelElement with the given position and sets all corresponding tile-references
	/// </summary>
	/// <param name="pos"></param>
	public virtual void InitPos(Vector2Int pos, int sizeX = 1, int sizeY = 1) {
        if(pos.x < 0 || pos.y < 0) {
            return;
        }
        if(sizeX <= 0 || sizeY <= 0) {
            return;
        }
        this.pos = pos;
        float levelPosX = (pos.x) + (sizeX / 2f);
        float levelPosY = (pos.y) + (sizeY / 2f);
        transform.position = new Vector3(levelPosX, levelPosY, 0);

        ISet<ILevelTile> tiles = GetTiles();
		foreach (ILevelTile tile in tiles) {
			tile.AddLevelElement(this);
		}
	}

	/// <summary>
	/// The tiles this level element is placed on
	/// </summary>
	public abstract ISet<ILevelTile> GetTiles();

	/// <summary>
	/// Returns, whether this <c>LevelElement</c> is solid
	/// </summary>
	/// <returns></returns>
	public abstract bool IsSolid();
}
