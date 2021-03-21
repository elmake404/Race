using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSpawnedTrafficCar : MonoBehaviour
{
    private PlayerSpeedMove playerSpeed;
    [HideInInspector] public int directionForwardOrBack;
    private float offsetSpeed = 5f;
    private void OnEnable()
    {
        playerSpeed = FindObjectOfType<PlayerSpeedMove>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        transform.position += directionForwardOrBack * (new Vector3(0f,0f,playerSpeed.multiplayerSpeed + offsetSpeed))*Time.deltaTime;
    }
}
