using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPropereties : MonoBehaviour
{
    public GameObject springBodyObject;
    public GameObject chassis;
    public GameObject wheelsContainer;
    public GameObject gameObjWithPlayerColider;
    public BoxCollider carBoxTrigger;
    public BoxCollider carBoxColider;
    public Rigidbody carRigidbody;
    private Renderer carMeshFilter;
    [HideInInspector] public bool isPlayerScaled;
    [HideInInspector] public Vector3 playerCarMeshBounds;
    [HideInInspector] public Vector3 playerCarMeshCenter;
    public PlayerCarColider playerCarColider;

    private void Awake()
    {
        carMeshFilter = chassis.GetComponent<Renderer>();
        playerCarMeshBounds = carMeshFilter.bounds.size;
        playerCarMeshCenter = carMeshFilter.bounds.center;
        
    }
}
