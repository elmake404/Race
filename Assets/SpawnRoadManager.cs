using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoadManager : MonoBehaviour
{
    public Transform spawnPoint;
    public Transform swapTilePoint;
    private AccesToObjectsLinks accesToObjectsLinks;
    [HideInInspector] public Vector3 boundsRoadTile;
    [HideInInspector] public GameObject lastSpawnedRoadTiles;
    [HideInInspector] public Queue<GameObject> queueRoadTiles;
    [HideInInspector] public Vector3 posSpawnRoad;
    [HideInInspector] public float travelPath;

    private void Start()
    {
        queueRoadTiles = new Queue<GameObject>();
        accesToObjectsLinks = FindObjectOfType<AccesToObjectsLinks>();
        lastSpawnedRoadTiles = new GameObject();
        boundsRoadTile = accesToObjectsLinks.bigRoadTile.GetComponent<Renderer>().bounds.size;
        Debug.Log(boundsRoadTile);
        GetInitialLastRoadTile();
        
    }


    private void GetInitialLastRoadTile()
    {
        int childNumber = accesToObjectsLinks.CollectBigRoadTiled.transform.childCount;
        
        for (int i = 0; i<childNumber; i++)
        {
            queueRoadTiles.Enqueue(accesToObjectsLinks.CollectBigRoadTiled.transform.GetChild(i).gameObject);
            
        }
        posSpawnRoad = accesToObjectsLinks.CollectBigRoadTiled.transform.GetChild(childNumber-1).transform.position;
    }

}
