using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TrafficCarState
{
    playerIsAlive = 0,
    playerIsDead = 1,

}
public class ControlSpawnedTrafficCar : MonoBehaviour
{
    private PlayerSpeedMove playerSpeed;
    [HideInInspector] public int directionForwardOrBack;
    private float offsetSpeed = 5f;
    [HideInInspector] public Rigidbody trafficCarRigidbody;
    [HideInInspector] public int trafficCarState;
    private void OnEnable()
    {
        trafficCarState = 0;
        playerSpeed = FindObjectOfType<PlayerSpeedMove>();
        trafficCarRigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        SwitchCarState((TrafficCarState)trafficCarState);
    }

    private void SwitchCarState(TrafficCarState state)
    {
        switch (state)
        {
            case TrafficCarState.playerIsAlive:
                transform.position += directionForwardOrBack * (new Vector3(0f, 0f, -directionForwardOrBack * playerSpeed.multiplayerSpeed + offsetSpeed)) * Time.deltaTime;
                break;
            case TrafficCarState.playerIsDead:
                transform.position += directionForwardOrBack * (new Vector3(0f, 0f, -directionForwardOrBack * playerSpeed.multiplayerSpeed)) * Time.deltaTime;
                break;
        }
    }

    public void MakeCarDestroyedWhenPlayerIsDead()
    {
        trafficCarRigidbody.isKinematic = false;
        trafficCarRigidbody.useGravity = true;
        trafficCarState = (int)TrafficCarState.playerIsDead;
    }
}
