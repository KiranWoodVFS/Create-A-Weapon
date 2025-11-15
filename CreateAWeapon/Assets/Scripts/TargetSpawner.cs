using System.Collections;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [Header("Spawn Area")]
    public Collider spawnArea;

    [Header("Spawn Objects")]
    public GameObject cubePrefab;

    [Header("Spawn Properties")]
    [Min(0f)] public float minDelay = 0.5f;
    [Min(0f)] public float maxDelay = 2.0f;

    public bool randomRotation = true;
    public Transform parentForSpawned;

    const int MaxAmountOfTries = 3;

    private void Awake()
    {
        if (!spawnArea) spawnArea = gameObject.GetComponent<Collider>();
        if (spawnArea && !spawnArea.isTrigger)
        {
            Debug.LogWarning("Spawn area should be a trigger");
        }
    }

    private void OnEnable()
    {
        StartCoroutine(SpawnLoop());
    }
    
    // Spawns Cube
    IEnumerator SpawnLoop()
    {
        while (enabled)
        {
            if (cubePrefab && spawnArea)
            {
                if (TryGetRandomPoint(spawnArea, out Vector3 pos))
                {
                    Quaternion rot = randomRotation ? Random.rotation : Quaternion.identity;
                    Transform parent = parentForSpawned ? parentForSpawned : transform;
                    GameObject go = Instantiate(cubePrefab, pos, rot, parent);
                }
            }

            float wait = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(wait);

        }
    }

    static bool TryGetRandomPoint(Collider objectCollider, out Vector3 position)
    {
        Bounds bounds = objectCollider.bounds;

        for (int i=0; i < MaxAmountOfTries; i++)
        {
            Vector3 point = new Vector3
                (
                   Random.Range(bounds.min.x, bounds.max.x),
                   Random.Range(bounds.min.y, bounds.max.y),
                   Random.Range(bounds.min.z, bounds.max.z)
                );

            if (objectCollider.ClosestPoint(point) == point)
            {
                position = point;
                return true;
            }
        }
        position = Vector3.zero;
        return false;
    }

}
