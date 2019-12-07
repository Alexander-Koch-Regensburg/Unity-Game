using UnityEngine.Audio;
using UnityEngine;
using System.Collections.Generic;


public class AudioComponent : MonoBehaviour
{
    protected AudioSource audioSource;
    private List<DynamicSound> sounds = new List<DynamicSound>();

    protected virtual void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public virtual void Play(string name)
    {
        SetupAudioSource(name);
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        else if (!audioSource.loop)
        {
            audioSource.Play();
        }
    }

    public void StopPlaying()
    {
        audioSource.Stop();
    }

    public void SetupAudioSource(string name)
    {
        DynamicSound s = GetDynamicSound(name);
        s.ConfigureAudioSource(audioSource);
    }

    public void AddSound(DynamicSound s)
    {
        sounds.Add(s);
    }

    protected DynamicSound GetDynamicSound(string name)
    {
        foreach (DynamicSound s in sounds)
        {
            if (s.name == name)
                return s;
        }
        return null;
    }

  
    public void CheckIfEnemyIsInRange(DynamicSound s)
    {
        List<Enemy> enemies = GetNearbyEnemies(s.maxEnemyDistance);
        foreach (Enemy enemy in enemies)
        {
            EnemyAwarenessSystem awarenessSystem = enemy.awarenessSystem;
			if (awarenessSystem.Awareness == awarenessSystem.MaxAwareness) {
                continue;
			}

            IList<Vector2> soundPath = NavMesh2D.instance.GetPathTo(enemy.transform.position, transform.position);
			if (soundPath == null) {
                return;
			}
            float distance = 0f;
			for (int i = 0; i < (soundPath.Count - 1); ++i) {
				distance += Vector2.Distance(soundPath[i], soundPath[i + 1]);
			}
            float awarenessIncrease = GetAwarenessIncrease(s.maxEnemyDistance, distance, awarenessSystem.soundReactionMultiplier, s.noise);
            awarenessSystem.Awareness += awarenessIncrease;

			if (awarenessSystem.Awareness > awarenessSystem.MaxAwareness) {
                awarenessSystem.Awareness = awarenessSystem.MaxAwareness;
			}
        }
    }


    public float GetAwarenessIncrease(float maxEnemyDistance, float distance, float soundReactionMultiplier, float noise)
    {
        if (distance > maxEnemyDistance)
        {
            return 0f;
        }

        return (maxEnemyDistance - distance) / maxEnemyDistance * soundReactionMultiplier * noise;
    }

    // TODO: Move to helper class as the exact same implementation exists in Enemy.cs
    public List<Enemy> GetNearbyEnemies(float maxEnemyDistance)
    {
        List<Enemy> enemies = new List<Enemy>();
        Vector2 overlapPos = (Vector2)transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(overlapPos, maxEnemyDistance);

        if (colliders == null)
        {
            return enemies;
        }
        foreach (Collider2D collider in colliders)
        {
            Enemy enemy = collider.gameObject.GetComponentInChildren<Enemy>();
            if (enemy != null)
            {
                enemies.Add(enemy);
            }
        }
        return enemies;
    }

}
