using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleUpUnderCars : MonoBehaviour
{
    

    private void Start()
    {
        
    }
    public void CheckCarOnTop(Transform playerCar, Vector3 centerBox, Vector3 sizeBox)
    {
        RaycastHit hit;
        Vector3 direction = new Vector3(playerCar.position.x, Vector3.up.y, playerCar.position.z) - playerCar.position;
        if (Physics.BoxCast(new Vector3(playerCar.position.x, centerBox.y-10f, playerCar.position.z), sizeBox, direction, out hit, Quaternion.identity, 10f))
        {

                Debug.Log("Explos");
                TrafficCarExplosion(hit.transform.GetComponent<Rigidbody>());

            
        }
        Debug.DrawRay(playerCar.position, direction, Color.green, Mathf.Infinity);
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3 (transform.position.x, centerCastBox.y, transform.position.z), sizeCastBox);
    }*/
    public void TrafficCarExplosion(Rigidbody rigidbody)
    {
        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;
        rigidbody.AddExplosionForce(20f, this.transform.position, 0, 20f, ForceMode.Impulse);
    }
}
