using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlayerTriggerState
{
    frontTriggerCollision = 0,
    sideTriggerCollision = 1

}
public class PlayerDeath : MonoBehaviour
{
    private CarPropereties carPropereties;
    private Rigidbody thisPlayerCarRigidbody;
    private BoxCollider thisPlayerCarColider;
    private PlayerSpeedMove playerSpeed;
    private PlayerCarController playerCarController;
    private TimeScaleManager timeScaleManager;
    private HashSet<Collider> crashedColiders;
    
    
    private void OnEnable()
    {
        //isPlayerScaled = false;
        timeScaleManager = FindObjectOfType<TimeScaleManager>();
        playerSpeed = FindObjectOfType<PlayerSpeedMove>();
        playerCarController = FindObjectOfType<PlayerCarController>();
        carPropereties = transform.parent.GetComponent<CarPropereties>();
        thisPlayerCarRigidbody = carPropereties.carRigidbody;
        thisPlayerCarColider = carPropereties.carBoxColider;
        //thisPlayerCarTrigger = GetComponent<BoxCollider>();
        crashedColiders = new HashSet<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (playerCarController.isCarSmallScale == true | timeScaleManager.isCarGrow == true)
        {
            return;
        }
        if (other.tag == "trafficCarColider")
        {
            if (crashedColiders.Add(other))
            {
                float angle = Mathf.Abs(Vector3.Angle(transform.forward, other.transform.position - transform.position));
                if (angle > 20)
                {
                    SwitchFrontOrSideTrigger(PlayerTriggerState.sideTriggerCollision, other);
                }
                else
                {
                    SwitchFrontOrSideTrigger(PlayerTriggerState.frontTriggerCollision, other);
                }
            }
            
        }
    }
    private void SwitchFrontOrSideTrigger(PlayerTriggerState state, Collider other)
    {
        switch (state)
        {
            case PlayerTriggerState.frontTriggerCollision:
                other.transform.parent.GetComponent<ControlSpawnedTrafficCar>().MakeCarDestroyedWhenPlayerIsDead();
                carPropereties.chassis.transform.SetParent(carPropereties.transform, true);
                playerSpeed.currentPlayerSpeedMoveState = (int)PlayerSpeedMoveState.playerIsDead;
                playerCarController.currentPlayerCarControllerState = (int)PlayerCarControllerState.PlayerIsDead;
                carPropereties.playerCarColider.setColiderPlayerCarState = (int)ColiderPlayerCarState.playerIsDead;
                thisPlayerCarColider.enabled = true;
                //carPropereties.gameObjWithPlayerColider.SetActive(true);
                thisPlayerCarRigidbody.isKinematic = false;
                thisPlayerCarRigidbody.useGravity = true;
                transform.GetComponent<BoxCollider>().enabled = false;
                thisPlayerCarRigidbody.AddForce(transform.forward*50f, ForceMode.Impulse);
                StartCoroutine(timeScaleManager.StartPlayerDeathCam());
                break;

            case PlayerTriggerState.sideTriggerCollision:
                Rigidbody otherRigidbody = other.transform.parent.GetComponent<Rigidbody>();
                otherRigidbody.isKinematic = false;
                otherRigidbody.useGravity = true;
                StartCoroutine(timeScaleManager.StartSlowMotion(other.transform));
                otherRigidbody.AddExplosionForce(20f, this.transform.position, 0, 20f, ForceMode.Impulse);
                break;
        }
    }

}
