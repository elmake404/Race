using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeedMove : MonoBehaviour
{
    [HideInInspector] public float multiplayerSpeed;
    [HideInInspector] public float smallSpeed = 4f;
    [HideInInspector] public float speedOfSpeed;
    [HideInInspector] public float speedIncreased;
    [HideInInspector] public float targetSpeed;
    [HideInInspector] public float damping;
    [HideInInspector] public float velocity;
    [HideInInspector] public float maxSpeed = 50f;
    [HideInInspector] public float normalDamping = 50f;
    private bool isSmallSpeed;

    void Start()
    {
        damping = normalDamping;
        targetSpeed = maxSpeed;
    }

    void Update()
    {
        
        speedUp(targetSpeed);
        multiplayerSpeed = speedIncreased;
        //Debug.Log(speedIncreased);
    }

    public void speedUp(float targetSpeed)
    {
        //velocity = Mathf.Clamp(velocity, 0f, 50f);
        float n1 = velocity - (speedIncreased - targetSpeed) * damping * Time.deltaTime;
        float n2 = 1 + damping * Time.deltaTime;
        velocity = n1 /(n2*n2);

        speedIncreased += velocity * Time.deltaTime;
    }

   
}
