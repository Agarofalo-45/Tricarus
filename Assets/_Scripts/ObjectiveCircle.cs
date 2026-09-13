using UnityEngine;

public class ObjectiveCircle : MonoBehaviour
{
    [Header("Settings")]
    public float timeToComplete = 3f;        
    public float timingWindow = 0.3f;       
    
    [Header("Visuals")]
    public Transform outerCircle;            
    public float outerStartScale = 3f;    

    private float _timer;
    private bool _isPlayerInside = false;
    private bool _hasClicked = false;

    void Start()
    {
        _timer = timeToComplete;
        
      
        if (outerCircle != null)
        {
            outerCircle.localScale = Vector3.one * outerStartScale;
        }
    }

    void Update()
    {
        _timer -= Time.deltaTime;
        

        if (outerCircle != null)
        {
          
            float progress = 1f - (_timer / timeToComplete); 
            
           
            float currentScale = Mathf.Lerp(outerStartScale, 1f, progress);
            outerCircle.localScale = Vector3.one * currentScale;
        }


        if (Input.GetMouseButtonDown(0) && _isPlayerInside && !_hasClicked)
        {
            _hasClicked = true;
            
       
            if (_timer <= timingWindow && _timer >= -timingWindow)
            {
                Debug.Log("Perfect timing! Player survived.");
                
             
                CameraShake shaker = FindAnyObjectByType<CameraShake>();
                if (shaker != null)
                {
                  
                    shaker.TriggerShake(0.15f, 0.3f); 
                }
              
                GameObject enemyArt = GameObject.Find("EnemyArt");
                if (enemyArt != null)
                {
                    enemyArt.GetComponent<Animator>().SetTrigger("PlayHit");
                                    
                BossManager boss = FindAnyObjectByType<BossManager>();
                if (boss != null)
                {
                    boss.TakeDamage();
                }
              
                }
             

                Destroy(gameObject);
                return;
            }
            else
            {
               
                Debug.Log("Clicked too early! Game Over.");
                KillPlayer();
                return;
            }
        }

      
        if (_timer <= -timingWindow && !_hasClicked)
        {
            Debug.Log("Ran out of time! Game Over.");
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        Player player = FindAnyObjectByType<Player>();
        if (player != null)
        {
            player.OnPlayerDie?.Invoke();
            Destroy(player.gameObject);
        }
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<Player>() != null)
        {
            _isPlayerInside = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<Player>() != null)
        {
            _isPlayerInside = false;
        }
    }
}