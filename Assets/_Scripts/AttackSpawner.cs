using System.Collections;
using UnityEngine;

public class AttackSpawner : MonoBehaviour
{
    public GameObject attackPrefab;
    
    [Header("Random Timing")]
    public float minTimeBetweenAttacks = 3f;
    public float maxTimeBetweenAttacks = 7f;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float randomWaitTime = Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks);
            
            yield return new WaitForSeconds(randomWaitTime);

            Instantiate(attackPrefab, transform.position, Quaternion.identity);
        }
    }
}