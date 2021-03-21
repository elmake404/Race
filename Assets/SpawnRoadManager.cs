using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoadManager : MonoBehaviour
{
    public Transform spawnPoint;
    private AccesToObjectsLinks accesToObjectsLinks;
    [HideInInspector] public Vector3 boundsRoadTile;
    [HideInInspector] public GameObject lastSpawnedRoadTiles;
    

    private void Start()
    {
        accesToObjectsLinks = FindObjectOfType<AccesToObjectsLinks>();
        lastSpawnedRoadTiles = new GameObject();
        boundsRoadTile = accesToObjectsLinks.bigRoadTile.GetComponent<Renderer>().bounds.size;
        Debug.Log(boundsRoadTile);
        GetInitialLastRoadTile();
        //SpawnNewRoadTile();
    }

    public void SpawnNewRoadTile(GameObject outInGameObject)
    {
        Vector3 posLastSpawnedRoadTile = lastSpawnedRoadTiles.transform.position;
        Vector3 newPos = new Vector3 (0f,0f, posLastSpawnedRoadTile.z + boundsRoadTile.z);

        outInGameObject.transform.position = newPos;
        lastSpawnedRoadTiles = outInGameObject;
    }

    private void GetInitialLastRoadTile()
    {
        int childNumber = accesToObjectsLinks.CollectBigRoadTiled.transform.childCount;
        lastSpawnedRoadTiles = accesToObjectsLinks.CollectBigRoadTiled.transform.GetChild(childNumber-1).gameObject;
        //Debug.Log(lastSpawnedRoadTiles.name);
    }

}
