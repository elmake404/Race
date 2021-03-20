using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccesToObjectsLinks : MonoBehaviour
{
    public GameObject bigRoadTile;
    public GameObject CollectBigRoadTiled;
    [HideInInspector]public CarSpawnManager carSpaawnManager;

    private void Awake()
    {
        carSpaawnManager = FindObjectOfType<CarSpawnManager>();
    }
}
