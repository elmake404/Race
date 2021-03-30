using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlayerTriggerResponsibilities
{
    playerWaitForDie = 0,
    playerCatchBonus = 1,

}
enum PlayerTriggerState
{
    frontTriggerCollision = 0,
    sideTriggerCollision = 1,
    

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
    private ParticlesSpawnManger particlesSpawn;
    private CanvasManager canvas;
    [HideInInspector] public int currentPlayerTriggerResponsibilities;
    [HideInInspector] public bool isPlayerDead;
    
    
    private void OnEnable()
    {
        particlesSpawn = FindObjectOfType<ParticlesSpawnManger>();
        canvas = FindObjectOfType<CanvasManager>();
        //isPlayerScaled = false;
        isPlayerDead = false;
        currentPlayerTriggerResponsibilities = (int)PlayerTriggerResponsibilities.playerWaitForDie;
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
        SwitchTriggerResponsibilities((PlayerTriggerResponsibilities)currentPlayerTriggerResponsibilities, other);
        
    }
    private void SwitchFrontOrSideTrigger(PlayerTriggerState state, Collider other)
    {
        switch (state)
        {
            case PlayerTriggerState.frontTriggerCollision:
                isPlayerDead = true;
                other.transform.parent.GetComponent<ControlSpawnedTrafficCar>().MakeCarDestroyedWhenPlayerIsDead();
                thisPlayerCarColider.enabled = true;
                carPropereties.wheelsContainer.transform.localRotation = Quaternion.Euler(90f,90f,90f);
                carPropereties.chassis.transform.SetParent(carPropereties.transform, true);
                playerSpeed.currentPlayerSpeedMoveState = (int)PlayerSpeedMoveState.playerIsDead;
                playerCarController.currentPlayerCarControllerState = (int)PlayerCarControllerState.PlayerIsDead;
                carPropereties.playerCarColider.setColiderPlayerCarState = (int)ColiderPlayerCarState.playerIsDead;
                //carPropereties.gameObjWithPlayerColider.SetActive(true);
                thisPlayerCarRigidbody.isKinematic = false;
                thisPlayerCarRigidbody.useGravity = true;
                transform.GetComponent<BoxCollider>().enabled = false;
                particlesSpawn.SpawnBigExplosion(other.gameObject);
                thisPlayerCarRigidbody.AddForce(transform.forward*50f, ForceMode.Impulse);
                FacebookManager.Instance.LevelFail(1);
                StartCoroutine(canvas.VisibleMenuScore());
                StartCoroutine(timeScaleManager.StartPlayerDeathCam());
                break;

            case PlayerTriggerState.sideTriggerCollision:
                Rigidbody otherRigidbody = other.transform.parent.GetComponent<Rigidbody>();
                canvas.InvokeOnActionDestructOtherCars();
                otherRigidbody.isKinematic = false;
                otherRigidbody.useGravity = true;
                StartCoroutine(timeScaleManager.StartSlowMotion(other.transform));
                particlesSpawn.SpawnBigExplosion(other.gameObject);
                otherRigidbody.AddExplosionForce(20f, this.transform.position, 0, 20f, ForceMode.Impulse);
                break;
        }
    }
    private void SwitchTriggerResponsibilities(PlayerTriggerResponsibilities state, Collider other)
    {
        switch (state)
        {
            case PlayerTriggerResponsibilities.playerWaitForDie:
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
                break;
            case PlayerTriggerResponsibilities.playerCatchBonus:
                break;
        }
    }
    
}
