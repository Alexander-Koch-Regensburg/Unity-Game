using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class JsonLevelSerializer : ILevelSerializer {
    
    /// <summary>
    /// Serialize the given level and levelElements into a json file. 
    /// Therefore, the JSON framework Json.NET (https://www.newtonsoft.com/json) will be used. 
    /// </summary>
    /// <param name="level"></param>
    /// <param name="levelElements"></param>
    /// <param name="levelItems"></param>
    /// <param name="enemyData"></param>
    public void SerializeLevel(string pathToSave, Level level, IList<ILevelElement> levelElements, IList<Item> levelItems, IList<Enemy> enemies) {

        ////////////////////////////////////////////////////////////////////////
        //////////////////////////// Level Elements ////////////////////////////
        ////////////////////////////////////////////////////////////////////////
        
        IList<ILevelElement> relevantLevelElements = new List<ILevelElement>();

        // Filter only the LevelElements that are necessary for serialization.
        // Remove all floor elements because they will not be serialized (and lead to an error if they get casted to LevelElement). 
        foreach (ILevelElement levelElement in levelElements) {
            if(levelElement is Wall || levelElement is Door || levelElement is EndZone) {
                relevantLevelElements.Add(levelElement);
            }
        }


        // Count how many elements of each type are in the given list. 
        int numberWallElements = 0;
        int numberDoorElements = 0;
		int numberEndZones = 0;

        foreach (ILevelElement levelElement in relevantLevelElements) {
			if (levelElement is Wall) {
				numberWallElements++;
			} else if (levelElement is Door) {
				numberDoorElements++;
			} else if (levelElement is EndZone) {
				numberEndZones++;
			}
        }

        // In case the serialize method is called at the start of the level and before the player can be spawned, use the spawn position as the players actual position. 
        JsonFloatPositionStructure spawnPosition;
        if(Player.instance is null) {
            spawnPosition = new JsonFloatPositionStructure(level.getSpawnPosition().x, level.getSpawnPosition().y); 
        }
        else {
            float playerPosX = (float)Math.Round(Player.instance.transform.position.x, 2);
            float playerPosY = (float)Math.Round(Player.instance.transform.position.y, 2);
            spawnPosition = new JsonFloatPositionStructure(playerPosX, playerPosY);
        }

        JsonLevelStructure levelNode = new JsonLevelStructure {
            Size = new JsonIntPositionStructure(level.GetLevelSize().x, level.GetLevelSize().y)
        };

        JsonPlayerStructure playerNode = new JsonPlayerStructure {
            SpawnPosition = spawnPosition
        };

        JsonLevelElementStructure levelElementsNode = new JsonLevelElementStructure {
			Walls = new JsonWallStructure[numberWallElements],
			Doors = new JsonDoorStructure[numberDoorElements],
			EndZones = new JsonEndZoneStructure[numberEndZones]
        };

        // Loop through the levelElements and add them to the corresponding array. 
        int wallArrayCounter = 0;
        int doorArrayCounter = 0;
		int endZoneArrayCounter = 0;
        foreach (LevelElement levelElement in relevantLevelElements) {
			// Wall
			if (levelElement is Wall) {
				JsonWallStructure tmpWallStructure = new JsonWallStructure {
					Position = new JsonIntPositionStructure(levelElement.getActualPosition().x, levelElement.getActualPosition().y)
				};
				levelElementsNode.Walls[wallArrayCounter] = tmpWallStructure;
				wallArrayCounter++;

			// Door
			} else if (levelElement is Door) {

				// We have to use a string here, because the serialization framework converts the enum entries to integer numbers otherwise.
				// A door can only have two states: horizontal or vertical.
				string doorRotation;
				if (levelElement.transform.localEulerAngles.z == 90.0F) {
					doorRotation = "HORIZONTAL";
				} else {
					doorRotation = "VERTICAL";
				}

				JsonDoorStructure tmpDoorStructure = new JsonDoorStructure {
					Position = new JsonIntPositionStructure(levelElement.Pos.x, levelElement.Pos.y),
					Rotation = doorRotation
				};
				levelElementsNode.Doors[doorArrayCounter] = tmpDoorStructure;
				doorArrayCounter++;

			// EndZone
			} else if (levelElement is EndZone) {
				JsonEndZoneStructure endZone = new JsonEndZoneStructure {
					Position = new JsonIntPositionStructure(levelElement.getActualPosition().x - 1, levelElement.getActualPosition().y - 1)
				};
				levelElementsNode.EndZones[endZoneArrayCounter] = endZone;
				endZoneArrayCounter++;
			}
        }
        

        ////////////////////////////////////////////////////////////////////////
        //////////////////////////// Level Items ///////////////////////////////
        ////////////////////////////////////////////////////////////////////////
       
        // Filter weapons. 
        IList<Weapon> weapons = new List<Weapon>();
        foreach (Item item in levelItems) {
            if (item is Weapon) {
                weapons.Add((Weapon)item);
            }
        }

        
        IList<Weapon> weaponsToSerialize = new List<Weapon>();
        for (int i = 0; i < weapons.Count; i++) {
            Weapon weapon = weapons[i];

            if (weapon != null) {
                string weaponType = getWeaponTypeByObject(weapon);
                weaponsToSerialize.Add(weapon);
            }
        }

        if(Player.instance.Weapon != null) {
            string weaponType = getWeaponTypeByObject(Player.instance.Weapon);
            playerNode.WeaponType = weaponType;
            playerNode.Amount = Player.instance.Weapon.GetAmmo();
        }

        // Create new node for level items.
        JsonLevelItemStructure levelItemsNode = new JsonLevelItemStructure {
            Weapons = new JsonWeaponStructure[weaponsToSerialize.Count]
        };

        for (int i = 0; i < weaponsToSerialize.Count; ++i) {
            Weapon weapon = weaponsToSerialize[i];
            string weaponType = getWeaponTypeByObject(weapon);

            JsonWeaponStructure jsonWeaponStructure = new JsonWeaponStructure {
                Position = new JsonFloatPositionStructure(weapon.getActualPosition().x, weapon.getActualPosition().y),
                Rotation = weapon.transform.rotation.x,
                Type = weaponType,
                Ammount = weapon.GetAmmo()
            };

            levelItemsNode.Weapons[i] = jsonWeaponStructure;
        }

        ////////////////////////////////////////////////////////////////////////
        //////////////////////////// Enemies ///////////////////////////////////
        ////////////////////////////////////////////////////////////////////////

        JsonEnemyStructure[] enemyDataArr = new JsonEnemyStructure[enemies.Count];
        
        for(int i = 0; i < enemies.Count; ++i) {
            Enemy tmpEnemy = enemies[i];

            // Create a list of JsonFloatPositionStructure for the patrol points. 
            int patrolPointCounter = 0;
            JsonFloatPositionStructure[] patrolPoints = new JsonFloatPositionStructure[tmpEnemy.PatrolPoints.Count];
            foreach (Vector2 patrolPoint in tmpEnemy.PatrolPoints) {
                JsonFloatPositionStructure jsonPatrolPoint = new JsonFloatPositionStructure(patrolPoint.x, patrolPoint.y);
                patrolPoints[patrolPointCounter] = jsonPatrolPoint;
                patrolPointCounter++;
            };


            string weaponType = null;
            int amount = 0;
            if(tmpEnemy.Weapon != null) {
                weaponType = getWeaponTypeByObject(tmpEnemy.Weapon);
                amount = tmpEnemy.Weapon.GetAmmo();
            }

            JsonEnemyStructure enemyNode = new JsonEnemyStructure {
                Position = new JsonFloatPositionStructure((float)Math.Round(tmpEnemy.getActualPosition().x, 2), (float)Math.Round(tmpEnemy.getActualPosition().y, 2)),
                Type = tmpEnemy.Type.ToString(),
                PatrolPoints = patrolPoints,
                WeaponType = weaponType,
                Amount = amount
            };

            enemyDataArr[i] = enemyNode;
        }



        levelNode.LevelElements = levelElementsNode;
        levelNode.LevelItems = levelItemsNode;
        levelNode.Enemies = enemyDataArr;
        levelNode.Player = playerNode;

        string jsonString = JsonConvert.SerializeObject(levelNode);

        System.IO.File.WriteAllText(pathToSave, jsonString);
    }

    public string getWeaponTypeByObject(Weapon weapon) {
        if (weapon is Pistol)
            return "PISTOL";
        else if (weapon is MachineGun)
            return "MACHINEGUN";
        else if (weapon is Bat)
            return "BAT";
        else if (weapon is Shotgun)
            return "SHOTGUN";
        else
            return "INVALID";
    }
}