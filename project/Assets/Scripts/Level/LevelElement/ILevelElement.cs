using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelElement {

    /// <summary>
	/// Inits this LevelElement with the given position and sets all corresponding tile-references
	/// </summary>
	/// <param name="pos"></param>
	void InitPos(Vector2Int pos, int sizeX = 1, int sizeY = 1);

	/// <summary>
	/// The tiles this level element is placed on
	/// </summary>
	ISet<ILevelTile> GetTiles();

	/// <summary>
	/// Returns, whether this <c>LevelElement</c> is solid
	/// </summary>
	/// <returns></returns>
	bool IsSolid();
}
