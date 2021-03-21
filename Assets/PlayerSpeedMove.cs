using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeedMove : MonoBehaviour
{
    [HideInInspector] public float multiplayerSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        multiplayerSpeed = 15f;
    }
}
