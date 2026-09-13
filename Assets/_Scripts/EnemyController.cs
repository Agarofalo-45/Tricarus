using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
   [SerializeField]
   private GameObject _enemyPrefab;
   
   [Header("Spawner Settings")]
   [SerializeField]
   private int _enemyCount = 5;
   [SerializeField]
   private Transform _spawnTopLeft, _spawnTopRight, _spawnBottomLeft, _spawnBottomRight;

   [Header("Warning Settings")]
   [SerializeField]
   private GameObject _warningPrefab; // The yellow circle prefab
   [SerializeField]
   private float _spawnDelay = 1.5f;  // Seconds the warning is shown before spawning

   // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Start()
   {
       for( int i = 0; i < _enemyCount; i++)
       {
           SpawnEnemy();
       }
   }

   private void SpawnEnemy()
   {
       // Start the Coroutine to handle the delay
       StartCoroutine(SpawnEnemyRoutine());
   }

   private IEnumerator SpawnEnemyRoutine()
   {
       // 1. Pick a random position
       Vector3 spawnPosition = SelectRandomPosition();
       
       // 2. Spawn the Warning circle to alert the player
       GameObject warning = Instantiate(_warningPrefab, spawnPosition, Quaternion.identity);
       
       // 3. Wait for the delay time
       yield return new WaitForSeconds(_spawnDelay);
       
       // 4. Remove the Warning circle
       Destroy(warning);
       
       // 5. Spawn the actual enemy
       GameObject enemyObject = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
       Enemy enemy = enemyObject.GetComponent<Enemy>();
       
       if(enemy != null)
       {
           enemy.OnDie += SpawnEnemy;
       }
   }

   private Vector3 SelectRandomPosition()
   {
       Transform selectedTransform = null;
       int randomValue = Random.Range(0,4);
       switch (randomValue)
       {
           case 0:
               selectedTransform = _spawnBottomLeft;
               break;
           case 1:
               selectedTransform = _spawnBottomRight;
               break;
           case 2:
               selectedTransform = _spawnTopLeft;
               break;
           case 3:
               selectedTransform = _spawnTopRight;
               break;
           default:
               selectedTransform = _spawnTopLeft;
               break;
       }
       return selectedTransform.position + (Vector3)Random.insideUnitCircle;  
   }

   // Update is called once per frame
   void Update()
   {
      
   }
}