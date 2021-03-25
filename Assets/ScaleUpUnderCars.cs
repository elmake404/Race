using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScaleUpUnderCars : MonoBehaviour
{

    private TimeScaleManager timeScale;
    public LayerMask layerMask;
    private void Start()
    {
        timeScale = FindObjectOfType<TimeScaleManager>();
    }
    public void CheckCarOnTop(Transform playerCar, Vector3 centerBox, Vector3 sizeBox)
    {
        RaycastHit hit;
        Vector3 direction = new Vector3(playerCar.position.x, Vector3.up.y, playerCar.position.z) - playerCar.position;
        if (Physics.BoxCast(new Vector3(playerCar.position.x, centerBox.y-10f, playerCar.position.z), new Vector3(sizeBox.x/2f, sizeBox.y, sizeBox.z) , direction, out hit, Quaternion.identity, 10f, layerMask))
        {
                
                TrafficCarExplosion(hit.transform.GetComponent<Rigidbody>());
                StartCoroutine(timeScale.StartSlowMotion(hit.transform));
        }
        
    }


    public void TrafficCarExplosion(Rigidbody rigidbody)
    {
        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;
        rigidbody.AddExplosionForce(20f, this.transform.position, 0, 20f, ForceMode.Impulse);
    }
}
