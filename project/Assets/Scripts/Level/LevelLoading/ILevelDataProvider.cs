using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelDataProvider {
	/// <summary>
	/// Gets all relevant data of the <c>Level</c> from the datasource
	/// </summary>
	/// <returns></returns>
	LevelData GetLevelData();

	/// <summary>
	/// Gets all relevant data for all <c>LevelElement</c>s from the datasource
	/// </summary>
	/// <returns></returns>
	IList<LevelElementData> GetLevelElementsData();

	/// <summary>
	/// Returns the data for all items in the level
	/// </summary>
	/// <returns></returns>
	IList<ItemData> GetItemData();

	/// <summary>
	/// Returns the data for all enemies in the level
	/// </summary>
	/// <returns></returns>
	IList<EnemyData> GetEnemyData();
}
