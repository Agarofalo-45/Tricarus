using Microsoft.Extensions.DependencyInjection;
using UnityEngine;

public class CloudsController : MonoBehaviour
{ 
   
    [SerializeField]
    private Transform[] _clouds = new Transform[6];
    [SerializeField]
    private float _speed = 1.0f;
    [SerializeField]
    private float _xLimit = 12.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
            
        for (int i = 0; i < _clouds.Length; i ++)
        {
            _clouds[i].position = _clouds[i].position + Vector3.right * Time.deltaTime*_speed;
            if (_clouds[i].position.x > _xLimit)
                {
                    _clouds[i].position -= new Vector3(2 * _xLimit,0,0);
                }
        }
        }
}

