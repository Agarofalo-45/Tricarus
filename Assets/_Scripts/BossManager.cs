using System.Collections;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [Header("Boss Settings")]
    public float timeBetweenAttacks = 4f; 

    [Header("Boss Phases")]
    public int hitsToPhaseTwo = 3;
    private int _currentHits = 0;
    private bool _isPhaseTwo = false;

    [Header("Constant Rock Hazard")]
    public GameObject rockAttackPrefab;
    public float rockWarningDelay = 1.0f; 
    public Vector2 rockSpawnAreaSize = new Vector2(10f, 10f); 
    public float minTimeBetweenRocks = 2f;
    public float maxTimeBetweenRocks = 5f;

    [Header("Attack 1: Axe Volley")]
    public GameObject axeAttackPrefab;
    public int axesPerAttack = 3;         
    public float minAxeDelay = 2f;        
    public float maxAxeDelay = 3f;        

    [Header("Attack 2: Crab Swarm")]
    public GameObject crabPrefab;
    public GameObject warningPrefab; 
    public float crabWarningDelay = 1.5f;
    public Transform spawnTopLeft, spawnTopRight, spawnBottomLeft, spawnBottomRight;

    void Start()
    {
        StartCoroutine(BossAttackLoop());
        StartCoroutine(ConstantRockFallingRoutine());
    }

    // --- PHASE 2 if you guys want to change the timing and stuff ---
    public void TakeDamage()
    {
        _currentHits++;
        
        
        if (_currentHits >= hitsToPhaseTwo && !_isPhaseTwo)
        {
            EnterPhaseTwo();
        }
    }

    private void EnterPhaseTwo()
    {
        _isPhaseTwo = true;
        Debug.Log("BOSS PHASE TWO STARTED!");

    
        GameObject enemyArt = GameObject.Find("EnemyArt");
        if (enemyArt != null)
        {
            enemyArt.GetComponent<Animator>().SetTrigger("PhaseTwo");
        }

        timeBetweenAttacks = 2f;       
        
        minTimeBetweenRocks = 0.5f;      
        maxTimeBetweenRocks = 2.0f;      
        
        axesPerAttack = 5;             
        minAxeDelay = 0.5f;              
        maxAxeDelay = 1.5f;              
        
        crabWarningDelay = 0.8f;       
    }

    // --- constant rock
    private IEnumerator ConstantRockFallingRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minTimeBetweenRocks, maxTimeBetweenRocks);
            yield return new WaitForSeconds(waitTime);
            StartCoroutine(SpawnSingleRockRoutine());
        }
    }

    private IEnumerator SpawnSingleRockRoutine()
    {
        float halfWidth = rockSpawnAreaSize.x / 2f;
        float halfHeight = rockSpawnAreaSize.y / 2f;
        float randomX = Random.Range(-halfWidth, halfWidth);
        float randomY = Random.Range(-halfHeight, halfHeight);
        Vector3 randomSpawnPosition = transform.position + new Vector3(randomX, randomY, 0f);

        GameObject warning = Instantiate(warningPrefab, randomSpawnPosition, Quaternion.identity);
        yield return new WaitForSeconds(rockWarningDelay);
        if (warning != null) Destroy(warning);
        Instantiate(rockAttackPrefab, randomSpawnPosition, Quaternion.identity);
    }

    
    private IEnumerator BossAttackLoop()
    {
        yield return new WaitForSeconds(2f);
        while (true)
        {
            int randomAttack = Random.Range(0, 2);
            if (randomAttack == 0) yield return StartCoroutine(AxeVolleyRoutine());
            else if (randomAttack == 1)
            {
                for (int i = 0; i < 5; i++) StartCoroutine(SpawnSingleCrabRoutine());
            }
            yield return new WaitForSeconds(timeBetweenAttacks);
        }
    }

    private IEnumerator AxeVolleyRoutine()
    {
        for (int i = 0; i < axesPerAttack; i++)
        {
            Instantiate(axeAttackPrefab, transform.position, Quaternion.identity);
            float delay = Random.Range(minAxeDelay, maxAxeDelay);
            yield return new WaitForSeconds(delay);
        }
    }

    
    private IEnumerator SpawnSingleCrabRoutine()
    {
        Vector3 spawnPosition = SelectRandomCorner();
        GameObject warning = Instantiate(warningPrefab, spawnPosition, Quaternion.identity);
        yield return new WaitForSeconds(crabWarningDelay);
        if (warning != null) Destroy(warning);
        Instantiate(crabPrefab, spawnPosition, Quaternion.identity);
    }

    private Vector3 SelectRandomCorner()
    {
        Transform selectedTransform = spawnTopLeft; 
        int randomValue = Random.Range(0, 4);
        switch (randomValue)
        {
            case 0: selectedTransform = spawnBottomLeft; break;
            case 1: selectedTransform = spawnBottomRight; break;
            case 2: selectedTransform = spawnTopLeft; break;
            case 3: selectedTransform = spawnTopRight; break;
        }
        return selectedTransform.position + (Vector3)Random.insideUnitCircle;  
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.2f); 
        Gizmos.DrawCube(transform.position, new Vector3(rockSpawnAreaSize.x, rockSpawnAreaSize.y, 0f));
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(rockSpawnAreaSize.x, rockSpawnAreaSize.y, 0f));
    }
}