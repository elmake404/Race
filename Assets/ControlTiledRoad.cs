using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTiledRoad : MonoBehaviour
{

    private SpawnRoadManager spawnManager;
    private PlayerSpeedMove playerSpeed;


    private void OnEnable()
    {
        spawnManager = FindObjectOfType<SpawnRoadManager>();;
        playerSpeed = FindObjectOfType<PlayerSpeedMove>();;
    }
    void Update()
    {
        transform.position -= new Vector3(0f,0f,playerSpeed.multiplayerSpeed)*Time.deltaTime;
    }

    private void OnBecameInvisible()
    {
        spawnManager.SpawnNewRoadTile(gameObject);
        
    }




}
