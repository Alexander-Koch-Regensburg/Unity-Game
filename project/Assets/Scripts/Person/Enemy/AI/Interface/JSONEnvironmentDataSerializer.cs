using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class JsonEnvironmentDataSerializer {

    /// <summary>
    /// Serialize environment data for an enemy as json using Json.NET
    /// </summary>
    /// <param name="enemy"></param>
    public static string SerializeEnemyEnvironmentData(Enemy enemy) {
        JsonEnemyEnvironmentStructure enemyNode = GetEnemyData(enemy);
        return JsonConvert.SerializeObject(enemyNode);
    }

    /// <summary>
    /// Serialize environment data for multiple enemies as json using Json.NET
    /// </summary>
    public static string SerializeEnemiesEnvironmentData(ISet<Enemy> enemies) {
        List<JsonEnemyEnvironmentStructure> baseNode = new List<JsonEnemyEnvironmentStructure>();
        foreach (Enemy enemy in enemies) {
            baseNode.Add(GetEnemyData(enemy));
        }
        return JsonConvert.SerializeObject(baseNode.ToArray());
    }

    private static JsonEnemyEnvironmentStructure GetEnemyData(Enemy enemy) {
        Vector2 position = enemy.getActualPosition();
        Weapon weapon = enemy.Weapon;

        JsonEnemyEnvironmentStructure structure =  new JsonEnemyEnvironmentStructure {
            ID = enemy.ID,
            Type = GetEnemyType(enemy),
            Health = enemy.health,
            MaxVelocity = enemy.maxVelocity,
            TurnVelocity = enemy.turnVelocity,
            PatrolVelocity = enemy.patrolVelocity,
            CurrentVelocity = enemy.navAgent.velocity,
            Acceleration = enemy.acceleration,
            VisionAngle = enemy.visionAngle,
            VisionRange = enemy.visionRange,
            Position = new JsonFloatPositionStructure(position.x, position.y),
            Rotation = enemy.transform.rotation.z,
            Awareness = enemy.awarenessSystem.Awareness,
            InciteEnemiesRange = enemy.inciteEnemiesRange,
            VisibleElements = GetVisibleElementsData(enemy)
        };

        if (weapon) {
            structure.WeaponType = GetWeaponType(weapon);
            structure.Ammunition = weapon.GetAmmo();
            structure.ProjectileDamage = weapon.GetProjectileDamage();
            structure.ProjectileSpeed = weapon.GetProjectileVelocity();
        }

        return structure;
    }

    private static JsonEnvironmentVisibleElementsStructure GetVisibleElementsData(Enemy enemy) {
        IList<GameObject> visibleGameObjects = enemy.GetComponent<FieldOfViewHelper>().GetObjectsInFieldOfView();

        IList<ILevelElement> levelElements = new List<ILevelElement>();
        IList<Item> levelItems = new List<Item>();
        IList<Enemy> enemies = new List<Enemy>();
        IList<ProjectileBase> projectiles = new List<ProjectileBase>();
        Player player = null;

        foreach (GameObject gameObject in visibleGameObjects) {
            if (gameObject.GetComponent<ILevelElement>() != null) {
                levelElements.Add(gameObject.GetComponent<ILevelElement>());
            } else if (gameObject.GetComponent<Item>() != null) {
                levelItems.Add(gameObject.GetComponent<Item>());
            } else if (gameObject.GetComponent<Enemy>() != null) {
                enemies.Add(gameObject.GetComponent<Enemy>());
            } else if (gameObject.GetComponent<ProjectileBase>() != null) {
                projectiles.Add(gameObject.GetComponent<ProjectileBase>());
            } else if (gameObject.GetComponent<Player>() != null) {
                player = gameObject.GetComponent<Player>();
            }
        }
        
        JsonEnvironmentVisibleElementsStructure baseNode = new JsonEnvironmentVisibleElementsStructure {
            LevelElements = GetLevelElementsData(levelElements),
            LevelItems = GetLevelItemsData(levelItems, enemy),
            Enemies = GetVisibleEnemiesData(enemies),
            Projectiles = GetProjectilesData(projectiles)
        };

        if (player != null) {
            baseNode.Player = GetPlayerData(player);
        }

        return baseNode;
    }

    private static JsonLevelElementStructure GetLevelElementsData(IList<ILevelElement> elements) {
        List<JsonWallStructure> walls = new List<JsonWallStructure>();
        List<JsonDoorStructure> doors = new List<JsonDoorStructure>();
        List<JsonEndZoneStructure> endzones = new List<JsonEndZoneStructure>();

        foreach (ILevelElement element in elements) {
            if (element is Wall) {
                walls.Add(GetWallData((Wall) element));
            } else if (element is Door) {
                doors.Add(GetDoorData((Door) element));
            } else if (element is EndZone) {
                endzones.Add(GetEndZoneData((EndZone) element));
            }
        }

        return new JsonLevelElementStructure();
    }

    private static JsonEnvironmentLevelItemsStructure GetLevelItemsData(IList<Item> items, Enemy enemy) {
        IList<Weapon> weapons = new List<Weapon>();
        foreach (Item item in items) {
            if (item is Weapon) {
                weapons.Add((Weapon) item);
            }
        }

        return new JsonEnvironmentLevelItemsStructure {
            Weapons = GetWeaponsData(weapons, enemy)
        };
    }

    private static JsonEnvironmentProjectileStructure[] GetProjectilesData(IList<ProjectileBase> projectiles) {
        List<JsonEnvironmentProjectileStructure> projectileData = new List<JsonEnvironmentProjectileStructure>();
        
        foreach (ProjectileBase projectile in projectiles) {
            Vector2 position = projectile.transform.position;
            Vector2 direction = projectile.Direction;
            string origin = GetProjectileOrigin(projectile);

            if (origin != null) {
                projectileData.Add(new JsonEnvironmentProjectileStructure {
                    Position = new JsonFloatPositionStructure(position.x, position.y),
                    Direction = new JsonFloatPositionStructure(direction.x, direction.y),
                    Velocity = projectile.velocity,
                    Damage = projectile.damage,
                    Origin = origin
                });
            }
        }

        return projectileData.ToArray();
    }

    private static JsonEnvironmentEnemyStructure[] GetVisibleEnemiesData(IList<Enemy> enemies) {
        List<JsonEnvironmentEnemyStructure> enemyData = new List<JsonEnvironmentEnemyStructure>();

        foreach (Enemy enemy in enemies) {
            Vector2 position = enemy.getActualPosition();
            JsonEnvironmentEnemyStructure structure = new JsonEnvironmentEnemyStructure {
                ID = enemy.ID,
                Position = new JsonFloatPositionStructure(position.x, position.y),
                Type = GetEnemyType(enemy),
                Health = enemy.health,
                Awareness = enemy.awarenessSystem.Awareness
            };

            if (enemy.Weapon != null) {
                structure.WeaponType = GetWeaponType(enemy.Weapon);
                structure.Ammunition = enemy.Weapon.GetAmmo();
            }

            enemyData.Add(structure);
        }

        return enemyData.ToArray();
    }

    private static JsonEnvironmentPlayerStructure GetPlayerData(Player player) {
        Vector2 position = player.transform.position;
        Quaternion rotation = player.transform.rotation;

        JsonEnvironmentPlayerStructure structure = new JsonEnvironmentPlayerStructure {
            Position = new JsonFloatPositionStructure(position.x, position.y),
            Rotation = rotation.z,
            Health = player.health
        };

        if (player.Weapon != null) {
            structure.WeaponType = GetWeaponType(player.Weapon);
            structure.Ammunition = player.Weapon.GetAmmo();
        }

        return structure;
    }

    private static JsonEnvironmentWeaponStructure[] GetWeaponsData(IList<Weapon> weapons, Enemy enemy) {
        List<JsonEnvironmentWeaponStructure> weaponData = new List<JsonEnvironmentWeaponStructure>();
        foreach (Weapon weapon in weapons) {
            if (weapon.GetPerson() != null) {
                continue;
            }

            Vector2 position = weapon.getActualPosition();
            Quaternion rotation = weapon.transform.rotation;
            weaponData.Add(new JsonEnvironmentWeaponStructure {
                Position = new JsonFloatPositionStructure(position.x, position.y),
                Rotation = rotation.z,
                Type = GetWeaponType(weapon),
                Ammunition = weapon.GetAmmo(),
                CanInteract = weapon.CanInteract(enemy.transform.position),
                ID = weapon.GetID()
            });
        }
        return weaponData.ToArray();
    }

    private static string GetWeaponType(Weapon weapon) {
        if (weapon is Bat) {
            return "BAT";
        }
        if (weapon is Pistol) {
            return "PISTOL";
        }
        if (weapon is Shotgun) {
            return "SHOTGUN";
        }
        if (weapon is MachineGun) {
            return "MACHINEGUN";
        }
        return null;
    }

    private static string GetEnemyType(Enemy enemy) {
        switch (enemy.Type) {
            case EnemyType.EASY:
                return "EASY";
            case EnemyType.HARD:
                return "HARD";
            default:
                return "BASIC";
        }
    }

    private static string GetProjectileOrigin(ProjectileBase projectile) {
        if (projectile.origin != null) {
            Person person = projectile.origin.GetComponent<Person>();
            if (person is Enemy) {
                return projectile.origin.GetComponent<Enemy>().ID;
            } else if (person is Player) {
                return "Player";
            }
        }
        return null;
    }

    private static JsonWallStructure GetWallData(Wall element) {
        Vector2 position = element.transform.position;
        return new JsonWallStructure {
            Position = new JsonIntPositionStructure((int) position.x, (int) position.y)
        };
    }

    private static JsonDoorStructure GetDoorData(Door element) {
        Vector2 position = element.transform.position;
        return new JsonDoorStructure {
            Position = new JsonIntPositionStructure((int) position.x, (int) position.y)
        };
    }

    private static JsonEndZoneStructure GetEndZoneData(EndZone element) {
        Vector2 position = element.transform.position;
        return new JsonEndZoneStructure {
            Position = new JsonIntPositionStructure((int) position.x, (int) position.y)
        };
    }
}