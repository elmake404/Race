using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPropereties : MonoBehaviour
{
    public GameObject springBodyObject;
    public GameObject chassis;
    public GameObject wheelsContainer;
    private Renderer carMeshFilter;
    [HideInInspector] public Vector3 playerCarMeshBounds;
    [HideInInspector] public Vector3 playerCarMeshCenter;

    private void Awake()
    {
        carMeshFilter = chassis.GetComponent<Renderer>();
        playerCarMeshBounds = carMeshFilter.bounds.size;
        playerCarMeshCenter = carMeshFilter.bounds.center;

    }
}
