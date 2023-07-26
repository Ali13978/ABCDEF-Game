using UnityEngine;

public class RandomPrefabSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public float spawnRadius = 10f;
    public int numberOfPrefabsToSpawn = 10;
    public float maxHeightOffset = 5f;
    public bool randomRotation = true;

    private void Start()
    {
        SpawnPrefabsRandomly();
    }

    public void SpawnPrefabsRandomly()
    {
        for (int i = 0; i < numberOfPrefabsToSpawn; i++)
        {
            // Calculate a random position within the spawn radius
            Vector3 randomPosition = Random.insideUnitSphere * spawnRadius;

            // Ensure the spawned prefab is not elevated
            randomPosition.y = 0f;

            // Calculate a random height offset
            float randomHeightOffset = Random.Range(0f, maxHeightOffset);

            // Set the new Y position with the height offset
            randomPosition.y = randomHeightOffset;

            // Create the prefab at the random position
            GameObject spawnedPrefab = Instantiate(prefabToSpawn, transform.position + randomPosition, Quaternion.identity);

            // Apply random rotation if required
            if (randomRotation)
            {
                spawnedPrefab.transform.rotation = Random.rotation;
            }

            // You can also set the parent to the empty GameObject created in step 1 (optional)
            spawnedPrefab.transform.parent = transform;
        }
    }
}
