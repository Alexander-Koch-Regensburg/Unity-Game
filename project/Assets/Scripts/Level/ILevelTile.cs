using System.Collections.Generic;
using UnityEngine;

public interface ILevelTile {
   
	/// <summary>
	/// Registers a <c>LevelElement</c> on this <c>ILevelTile</c>
	/// </summary>
	/// <param name="element"></param>
	void AddLevelElement(ILevelElement element);

	/// <summary>
	/// Unregisters a <c>LevelElement</c> if it is placed on this <c>ILevelTile</c>
	/// </summary>
	/// <param name="element"></param>
	void RemoveLevelElement(ILevelElement element);

	/// <summary>
	/// Determines whether a person can move on this <c>ILevelTile</c>
	/// </summary>
	/// <returns></returns>
	bool IsTraversable();

	/// <summary>
	/// The lower left position of the <c>ILevelTile</c> within the <c>Level</c>
	/// </summary>
	Vector2Int Pos { get; }

	/// <summary>
	/// The center position of the <c>ILevelTile</c> within the <c>Level</c>
	/// </summary>
	Vector2 CenterPos { get; }

    ISet<ILevelElement> LevelElements { get; }

}