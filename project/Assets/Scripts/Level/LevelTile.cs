using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A single tile of a level
/// </summary>
public class LevelTile : ILevelTile {
	private Vector2Int pos;

	/// <summary>
	/// The position of the <c>Tile</c> within the <c>Level</c>
	/// </summary>
	public Vector2Int Pos {
		get {
			return pos;
		}
	}

	public Vector2 CenterPos {
		get {
			return new Vector2(pos.x + 0.5f, pos.y + 0.5f);
		}
	}

	private ISet<ILevelElement> levelElements;
	public ISet<ILevelElement> LevelElements {
		get {
			return levelElements;
		}
	}

    /// <summary>
    /// Creates a new <c>Tile</c> at the given pos
    /// </summary>
    /// <param name="pos"></param>
    public LevelTile(Vector2Int pos) {
		this.pos = pos;
	}

	/// <summary>
	/// Registers a <c>LevelElement</c> on this <c>Tile</c>
	/// </summary>
	/// <param name="element"></param>
	public void AddLevelElement(ILevelElement element) {
		if (levelElements == null) {
			levelElements = new HashSet<ILevelElement>();
		}
		levelElements.Add(element);
	}

	/// <summary>
	/// Unregisters a <c>LevelElement</c> if it is placed on this <c>Tile</c>
	/// </summary>
	/// <param name="element"></param>
	public void RemoveLevelElement(ILevelElement element) {
        if(levelElements == null) {
            return;
        }
		levelElements.Remove(element);
	}

	/// <summary>
	/// Determines whether a person can move on this <c>ILevelTile</c>
	/// </summary>
	/// <returns></returns>
	public bool IsTraversable() {
		if (levelElements == null) {
			return true;
		}
		foreach (ILevelElement element in levelElements) {
			if (element.IsSolid()) {
				return false;
			}
		}
		return true;
	}
}
