using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTiledRoad : MonoBehaviour
{

    private SpawnManager spawnManager;


    private void OnEnable()
    {
        spawnManager = FindObjectOfType<SpawnManager>();;
    }
    void Update()
    {
        transform.position -= new Vector3(0,0,10f)*Time.deltaTime;
    }

    private void OnBecameInvisible()
    {
        spawnManager.SpawnNewRoadTile(gameObject);
        
    }




}
