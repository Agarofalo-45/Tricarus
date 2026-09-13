using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using System.Collections; 
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField]
    private string _horizontalAxis = "Horizontal", _verticalAxis = "Vertical";
    [SerializeField] 
    private Rigidbody2D _rb2d;
    
    [Header("Movement Settings")]
    public float speed = 5f;
    
    [Header("Dash Settings")]
    public float dashSpeedMultiplier = 4f; 
    public float dashDuration = 0.15f;     
    public float dashCooldown = 1f;      
    
    private bool _canDash = true;
    public UnityEvent OnPlayerDie;
    private Vector2 _input;


    private void FixedUpdate()
    {
       
        _rb2d.linearVelocity = _input * speed;
    }

    void Update()
    {
        
        float horizontalInput = Input.GetAxisRaw(_horizontalAxis);
        float verticalInput = Input.GetAxisRaw(_verticalAxis);
        _input = new Vector2(horizontalInput, verticalInput);
        _input.Normalize();

        if (Input.GetKeyDown(KeyCode.LeftShift) && _canDash && _input != Vector2.zero)
        {
            StartCoroutine(DashRoutine());
        }
    }

    private IEnumerator DashRoutine()
    {
     
        _canDash = false;
        
       
        float originalSpeed = speed;
        speed = speed * dashSpeedMultiplier;

        yield return new WaitForSeconds(dashDuration);

   
        speed = originalSpeed;


        yield return new WaitForSeconds(dashCooldown);
        _canDash = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.GetComponentInParent<Enemy>() != null)
        {
            if(OnPlayerDie != null)
            {
                OnPlayerDie.Invoke();
            }
            Destroy(gameObject);
        }
    }
}