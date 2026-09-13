using System.Collections;
using UnityEngine;

public class CurvedAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float warningTime = 2f;        
    public float travelTime = 1f;       
    public float rightCurveAmount = 10f;  
    public float spinSpeed = 500f;

    [Header("Random Targeting")]
    [Tooltip("Percentage chance to pick a random spot (0 = always player, 100 = always random)")]
    public float randomTargetChance = 40f; // 40% chance to be random
    public Vector2 randomAreaCenter = new Vector2(0, -3f); // The center of the random box
    public Vector2 randomAreaSize = new Vector2(15f, 10f); // The size of the random box

    [Header("Visuals")]
    public GameObject warningDotPrefab;   
    public int numberOfDots = 15;         
    
    private Vector3 _startPoint;
    private Vector3 _controlPoint;
    private Vector3 _endPoint;
    private GameObject[] _warningDots;
    private bool _isMoving = false;
    private float _moveTimer = 0f;


    void Start()
    {
        _startPoint = transform.position;
        
        Player player = FindAnyObjectByType<Player>();
        if (player == null)
        {
            Destroy(gameObject);
            return;
        }

        // --- NEW TARGETING LOGIC ---
        // Roll a 100-sided dice. If it's less than our random chance, pick a random spot!
        if (Random.Range(0f, 100f) < randomTargetChance)
        {
            // Pick a random X and Y inside our defined area
            float randomX = randomAreaCenter.x + Random.Range(-randomAreaSize.x / 2f, randomAreaSize.x / 2f);
            float randomY = randomAreaCenter.y + Random.Range(-randomAreaSize.y / 2f, randomAreaSize.y / 2f);
            _endPoint = new Vector3(randomX, randomY, 0f);
        }
        else
        {
            // Target the player normally
            _endPoint = player.transform.position;
        }
        // ----------------------------

        Vector3 midPoint = (_startPoint + _endPoint) / 2f;
        float curveDirection = Random.Range(0, 2) == 0 ? rightCurveAmount : -rightCurveAmount;
        _controlPoint = midPoint + new Vector3(curveDirection, 0, 0);

        DrawDottedLine();
        StartCoroutine(AttackSequence());
    }

    private void DrawDottedLine()
    {
        if (warningDotPrefab == null) return;

        _warningDots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            float t = i / (float)(numberOfDots - 1);
            Vector3 pointOnCurve = CalculateBezierPoint(t, _startPoint, _controlPoint, _endPoint);
            _warningDots[i] = Instantiate(warningDotPrefab, pointOnCurve, Quaternion.identity);
        }
    }

    private IEnumerator AttackSequence()
    {
        yield return new WaitForSeconds(warningTime);

        if (_warningDots != null)
        {
            foreach (GameObject dot in _warningDots)
            {
                Destroy(dot);
            }
        }
        _isMoving = true;
    }

        void Update()
    {
        if (_isMoving)
        {
            _moveTimer += Time.deltaTime;
            float t = _moveTimer / travelTime;
            transform.position = CalculateBezierPoint(t, _startPoint, _controlPoint, _endPoint);
            // This rotates the sprite in 2D space. 
            transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
            if (t >= 2.5f)
            {
                Destroy(gameObject);
            }
        }
    }

    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0; 
        p += 2 * u * t * p1; 
        p += tt * p2; 
        return p;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isMoving && collision.GetComponentInParent<Player>() != null)
        {
            Player player = collision.GetComponentInParent<Player>();
            player.OnPlayerDie?.Invoke();
            Destroy(player.gameObject);
            Destroy(gameObject);
        }
    }

    // Draws a helpful red box in your Scene view when you click on the attack!
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f); // See-through red
        Gizmos.DrawCube(new Vector3(randomAreaCenter.x, randomAreaCenter.y, 0), new Vector3(randomAreaSize.x, randomAreaSize.y, 0));
        Gizmos.color = Color.red; // Solid red outline
        Gizmos.DrawWireCube(new Vector3(randomAreaCenter.x, randomAreaCenter.y, 0), new Vector3(randomAreaSize.x, randomAreaSize.y, 0));
    }
}