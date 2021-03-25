using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlayerSpeedMoveState
{
    playerIsAlive = 0,
    playerIsDead = 1
}
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
    [HideInInspector] public int currentPlayerSpeedMoveState;
    private ControlTiledRoad[] controlTiledRoad;
    private SpawnRoadManager spawnManager;
    private bool isSmallSpeed;

    void Start()
    {
        currentPlayerSpeedMoveState = (int)PlayerSpeedMoveState.playerIsAlive;
        damping = normalDamping;
        targetSpeed = maxSpeed;
        controlTiledRoad = FindObjectsOfType<ControlTiledRoad>();
        spawnManager = FindObjectOfType<SpawnRoadManager>();
    }

    void Update()
    {
        SwitchUpdatePlayerSpeedMove(currentPlayerSpeedMoveState);
    }

    private void SwitchUpdatePlayerSpeedMove(int currentState)
    {
        switch ((PlayerSpeedMoveState)currentState)
        {
            case PlayerSpeedMoveState.playerIsAlive:
                speedUp(targetSpeed);
                multiplayerSpeed = speedIncreased;
                ChangeSpeedTraffic();
                break;
            case PlayerSpeedMoveState.playerIsDead:
                multiplayerSpeed = 0f;
                speedIncreased = 0f;
                break;
        }
    }

    public void speedUp(float targetSpeed)
    {
        //velocity = Mathf.Clamp(velocity, 0f, 50f);
        float n1 = velocity - (speedIncreased - targetSpeed) * damping * Time.deltaTime;
        float n2 = 1 + damping * Time.deltaTime;
        velocity = n1 /(n2*n2);

        speedIncreased += velocity * Time.deltaTime;
    }

    private void ChangeSpeedTraffic()
    {
        Vector3 offset = new Vector3(0f, 0f, multiplayerSpeed) * Time.deltaTime;
        for (int i = 0; i < controlTiledRoad.Length; i++)
        {
            controlTiledRoad[i].transform.position -= offset;
            if (controlTiledRoad[i].gameObject.GetHashCode() == spawnManager.queueRoadTiles.Peek().GetHashCode())
            {

                if (spawnManager.travelPath > Mathf.Abs(spawnManager.boundsRoadTile.z))
                {
                    GameObject currentObj = spawnManager.queueRoadTiles.Dequeue();
                    Vector3 newPos = Vector3.zero;
                    int k = 0;
                    foreach (GameObject currObj in spawnManager.queueRoadTiles)
                    {
                        if (k == 1)
                        {
                            newPos = new Vector3(currentObj.transform.position.x, currentObj.transform.position.y, currentObj.transform.position.z + controlTiledRoad.Length * spawnManager.boundsRoadTile.z);
                        }
                        k++;
                    }
                    controlTiledRoad[i].transform.position = newPos;
                    spawnManager.queueRoadTiles.Enqueue(controlTiledRoad[i].gameObject);
                    spawnManager.travelPath = 0f;
                }
            }
        }
        spawnManager.travelPath += offset.z;
        //Debug.Log(spawnManager.boundsRoadTile.z);
    }

   
}
