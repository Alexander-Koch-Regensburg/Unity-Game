using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
public class AIInterfaceComponent : MonoBehaviour
{
    public static AIInterfaceComponent instance;

    void Awake() {
		instance = this;
    }

    /// <summary>
	/// Return serialized environment data for all enemies
	/// </summary>
    public string GetEnvironmentData () {
        ISet<Enemy> enemies = PersonController.instance.Enemies;
        return JsonEnvironmentDataSerializer.SerializeEnemiesEnvironmentData(enemies);
    }

    /// <summary>
	/// Interpret and execute command
	/// </summary>
    public string HandleCommand (string input) {

        JsonEnemyCommandStructure command = JsonConvert.DeserializeObject<JsonEnemyCommandStructure>(input);

        Enemy enemy = PersonController.instance.GetEnemyById(command.EnemyID);
        if (enemy == null) {
            return "Failed: No such enemy";
        }
        EnemyAIComponent aIComponent = enemy.GetComponent<EnemyAIComponent>();

        switch (command.Method) {
            case "GetEnvironmentData":
                return aIComponent.GetEnvironmentData();
            case "SetDestination":
                JsonFloatPositionStructure structure = command.Destination;
                Vector2 destination = new Vector2(structure.x, structure.y);
                aIComponent.SetDestination(destination, (float) command.Velocity);
                break;
            case "SetRotation":
                aIComponent.SetRotation((float) command.Rotation);
                break;
            case "Interact":
                aIComponent.Interact(command.InteractableID);
                break;
            case "FireWeapon":
                aIComponent.FireWeapon();
                break;
            case "DropWeapon":
                aIComponent.DropWeapon();
                break;
            case "CloseCombatAttack":
                aIComponent.CloseCombatAttack();
                break;
            case "StopOverride":
                aIComponent.StopOverride();
                break;
            default:
                return "Failed: No such command";
        }

        return null;
    }
}
