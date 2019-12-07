using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyAwarenessSystem : MonoBehaviour {
    // configurable properties

    /// <summary>
	/// The threshold for when the enemy starts attacking
	/// </summary>
    [Range(0f, 100f)]
	public float attackThreshold = 80f;

    /// <summary>
	/// Time for cooldown in seconds
	/// </summary>
	public float awarenessCooldown = 5f;

    /// <summary>
    /// Determines how much the enemy reacts to sound
    /// </summary>
    public float soundReactionMultiplier = 0.5f;

	// end configurable properties

	private const float AWARENESS_UPDATE_RATE = 0.5f;

    private float maxAwareness = 100f;
	public float MaxAwareness {
		get {
			return maxAwareness;
		}
	}

	private float awareness = 0f;
    /// <summary>
	/// How aware the enemy is of the player
	/// </summary>
	public float Awareness {
		get {
			return awareness;
		}
		set {
			if (value >= 0f)
			{
				if (value > awareness) {
					OnAwarenessIncreased?.Invoke();
				}
				awareness = Math.Min(value, maxAwareness);
				StartCooldown();
			} else {
				awareness = 0f;
			}
		}
	}

	public delegate void AwarenessIncreased();
	public event AwarenessIncreased OnAwarenessIncreased;

    private bool isCoolingDown = false;
    public bool IsCoolingDown {
        get {
            return isCoolingDown;
        }
    }

    private Enemy enemy;

    void Awake() {
		enemy = transform.gameObject.GetComponent<Enemy>();
	}

    /// <summary>
	/// Reduces player awareness according to awarenessCooldown
	/// </summary>
	public void StartCooldown() {
		if (isCoolingDown == false) {
			isCoolingDown = true;
			StartCoroutine(AwarenessCooldown());
		}
	}

    public bool IsAboveThreshold() {
        return awareness > attackThreshold;
    }

	/// <summary>
	/// Routine for deacreasing player awareness, when play is out of sight
	/// </summary>
	private IEnumerator AwarenessCooldown() {
		float startAwareness = awareness;

		while (awareness > 0) {
            if (enemy.HasPlayerInSight) {
                isCoolingDown = false;
                yield break;
            }
            yield return new WaitForSeconds(AWARENESS_UPDATE_RATE);
			float reduction = maxAwareness * (1 / awarenessCooldown) * AWARENESS_UPDATE_RATE;
			awareness = Math.Max(awareness - reduction, 0);
		}
        isCoolingDown = false;
        yield break;
	}
}