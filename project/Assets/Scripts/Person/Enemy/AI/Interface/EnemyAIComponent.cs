using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIComponent : MonoBehaviour
{
    public Enemy enemy;
    private FieldOfViewHelper fovChecker;

    private void Awake() {
        enemy = GetComponent<Enemy>();
        fovChecker = GetComponent<FieldOfViewHelper>();
    }

    public string GetEnvironmentData() {
        return JsonEnvironmentDataSerializer.SerializeEnemyEnvironmentData(enemy);
    }

    public void SetDestination(Vector2? destination, float velocity) {
        enemy.IsOverridden = true;
        enemy.ChangeDestination(destination, velocity);
    }

    public void SetRotation(float rotation) {
        enemy.IsOverridden = true;
        enemy.RotateEnemyToAngle(rotation);
    }

    public void FireWeapon() {
        enemy.IsOverridden = true;
        enemy.Weapon.Fire();;
    }

    public void DropWeapon() {
        enemy.IsOverridden = true;
        enemy.DropWeapon();
    }

    public void CloseCombatAttack() {
        enemy.IsOverridden = true;
        enemy.CloseCombatAttack();
    }

    public void StopOverride() {
        enemy.IsOverridden = false;
    }

    public void Interact(string id) {
        enemy.IsOverridden = true;
        IInteractable interactable = fovChecker.GetInteractableById(id);
        if (interactable != null) {
            interactable.Interact(enemy);
        }
    }
}
