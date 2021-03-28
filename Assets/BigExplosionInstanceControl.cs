using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigExplosionInstanceControl : MonoBehaviour
{
    //[HideInInspector] public Vector3 targetToFollow;
    private PlayerSpeedMove playerSpeed;
    private void OnEnable()
    {
        playerSpeed = FindObjectOfType<PlayerSpeedMove>();
        //transform.position = targetToFollow;
    }

    void Update()
    {
        transform.position += new Vector3(0f, 0f,  -playerSpeed.multiplayerSpeed* Time.deltaTime);
    }
        
    /*public void MoveThisInstanceToTheObject(GameObject transformObj)
    {
        
        if (transformObj == null)
        {
            return;
        }
        else
        {
            Debug.Log(transform.position);
            transform.position = transformObj.transform.position;
        }
        
    }*/
}
