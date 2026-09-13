using System.Collections;
using UnityEngine;

public class ObjectiveSpawner : MonoBehaviour
{
    public GameObject objectiveCirclePrefab;
    public Vector2 spawnAreaSize = new Vector2(10f, 10f); // Size of the spawn area
    public float timeBetweenSpawns = 5f;                  // How often a new circle appears

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenSpawns);
            SpawnCircle();
        }
    }

    private void SpawnCircle()
    {
        // Calculate a random X and Y inside the spawn area
        float halfWidth = spawnAreaSize.x / 2f;
        float halfHeight = spawnAreaSize.y / 2f;

        float randomX = Random.Range(-halfWidth, halfWidth);
        float randomY = Random.Range(-halfHeight, halfHeight);

        Vector3 spawnPosition = transform.position + new Vector3(randomX, randomY, 0f);

        // Spawn the circle!
        Instantiate(objectiveCirclePrefab, spawnPosition, Quaternion.identity);
    }

    // This draws a green box in the Unity Editor so you can visually align it with your map!
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.2f); // Transparent green
        Gizmos.DrawCube(transform.position, new Vector3(spawnAreaSize.x, spawnAreaSize.y, 0f));
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnAreaSize.x, spawnAreaSize.y, 0f));
    }
}