using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataCollection
{
    /// <summary>
    /// The <c>ILevelElement</c>s placed on the <c>Level</c>
    /// </summary>
    public IList<ILevelElement> levelElements;

    /// <summary>
    /// The <c>Items</c> placed on the <c>Level</c>
    /// </summary>
    public IList<ItemData> levelItemsData;

    /// <summary>
    /// The <c>Items</c> placed on the <c>Level</c>
    /// </summary>
    public IList<Item> levelItems;

    /// <summary>
    /// The <c>Enemies</c> placed on the <c>Level</c>
    /// </summary>
    public IList<EnemyData> enemyData;

    /// <summary>
    /// The <c>Enemies</c> placed on the <c>Level</c>
    /// </summary>
    public IList<Enemy> enemies;

    /// <summary>
    /// The data for the player. 
    /// </summary>
    public PlayerData playerData;

    public LevelDataCollection()
    {
        levelElements = new List<ILevelElement>();
        levelItemsData = new List<ItemData>();
        levelItems = new List<Item>();
        enemyData = new List<EnemyData>();
        enemies = new List<Enemy>();
        playerData = new PlayerData();
    }
}
