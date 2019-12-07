using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelLoader {
	/// <summary>
	/// Loads and returns the <c>Level</c>
	/// </summary>
	/// <returns></returns>
	Level LoadLevel();

	/// <summary>
	/// Loads all <c>ILevelElement</c>s of the Level
	/// </summary>
	/// <returns></returns>
	IList<ILevelElement> LoadLevelElements(Transform parent);

    /// <summary>
    /// Loads all item data in the level. 
    /// </summary>
    /// <returns></returns>
    IList<ItemData> LoadItemsData();

    /// <summary>
    /// Loads all items in the level
    /// </summary>
    /// <param name="parent"></param>
    IList<Item> LoadItems(Transform parent, IList<ItemData> itemData);
}
