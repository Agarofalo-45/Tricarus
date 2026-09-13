using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed_min = 4;
    [SerializeField]
    private float _speed_max = 7;
    private float _speed;
    private Rigidbody2D _rb2d;
    private Transform _playerTransform;
    public bool Stopped = false;
    [SerializeField]
    private GameObject _crabDead;

    public event Action OnDie = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _speed = UnityEngine.Random.Range(_speed_min, _speed_max);
        _rb2d = GetComponent<Rigidbody2D>();
        Player player = FindAnyObjectByType<Player>();

        if (player != null){
        _playerTransform = player.transform;
        }
        else
        {
            Stopped = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    private void Move()
    {
         
        if(Stopped || _playerTransform == null)
        {
            _rb2d.linearVelocity = Vector2.zero * _speed;
            return;
        }
        
        _rb2d.linearVelocity = Vector2.down * _speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Weapon"))
        {
            Instantiate(_crabDead,transform.position,Quaternion.identity);
            Destroy(gameObject);
            if(OnDie != null)
            {
                print("helo");
                OnDie();
            }
        }
    }
}
