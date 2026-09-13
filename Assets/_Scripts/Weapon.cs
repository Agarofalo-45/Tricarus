using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private int rotationSpeed = 200;
    [SerializeField]
    private Vector3 rotationPoint = Vector3.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        float rotationAmount = CalculateRotationAmount(Time.deltaTime);
        transform.RotateAround(rotationPoint,Vector3.forward, rotationAmount);
    }

    private float CalculateRotationAmount(float delta)
    {
        return rotationSpeed * delta;
    }
}