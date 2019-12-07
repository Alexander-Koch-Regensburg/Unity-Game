using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicLevelLoader : ILevelLoader {

	private ILevelDataProvider dataProvider;

	private Level currentLevel;

	public BasicLevelLoader(ILevelDataProvider dataProvider) {
		this.dataProvider = dataProvider;
	}

	public Level LoadLevel() {
        LevelData levelData = LoadLevelData();
        currentLevel = new Level(levelData.size, levelData.spawnPosition);

		return currentLevel;
	}

    public LevelData LoadLevelData()
    {
        return dataProvider.GetLevelData();
    }

    public IList<ILevelElement> LoadLevelElements(Transform parent) {
		IList<ILevelElement> levelElements = new List<ILevelElement>();

        IList<LevelElementData> elementData = loadLevelElementData();
		foreach (LevelElementData data in elementData) {
			ILevelElement element = LevelElementFactory.InstantiateLevelElement(data, parent);
			if (element != null) {
				levelElements.Add(element);
			}
		}

		FillEmptyTilesWithFloor(levelElements, parent);

		return levelElements;
	}

    public IList<LevelElementData> loadLevelElementData()
    {
        return dataProvider.GetLevelElementsData();
    }

    public IList<ItemData> LoadItemsData() {
        return dataProvider.GetItemData();
    }

	public IList<Item> LoadItems(Transform parent, IList<ItemData> itemData) {
        IList<Item> items = new List<Item>();
		foreach (ItemData data in itemData) {
			if (data is WeaponData) {
				Weapon weapon = ItemFactory.InstantiateWeapon((WeaponData)data, parent);
                items.Add(weapon);
			}
		}
        return items;
	}

	private void FillEmptyTilesWithFloor(IList<ILevelElement> levelElements, Transform parent) {
		if (currentLevel == null) {
			return;
		}
		Vector2Int levelSize = currentLevel.GetLevelSize();

		for (int x = 0; x < levelSize.x; ++x) {
			for (int y = 0; y < levelSize.y; ++y) {
				Vector2Int pos = new Vector2Int(x, y);
                ILevelTile tile = currentLevel.GetTileAtPos(pos);
				if (tile.LevelElements != null) {
					bool containsSolidElement = false;
					foreach (ILevelElement element in tile.LevelElements) {
						if (element.IsSolid()) {
							containsSolidElement = true;
							break;
						}
					}
					if (containsSolidElement) {
						continue;
					}
				}
				FloorData floorData = new FloorData(pos);

				ILevelElement floor = LevelElementFactory.InstantiateLevelElement(floorData, parent);
				levelElements.Add(floor);
			}
		}
	}
}
