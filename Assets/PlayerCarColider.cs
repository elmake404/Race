using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ColiderPlayerCarState
{
    playerIsAlive = 0,
    playerIsDead = 1,
    playerIsBig = 2
}

public class PlayerCarColider : MonoBehaviour
{
    private HashSet<Collider> usedColider;
    private HashSet<Collider> usedColiderIsBigPlayer;
    [HideInInspector] public int setColiderPlayerCarState;
    private void OnEnable()
    {
        usedColider = new HashSet<Collider>();
        usedColiderIsBigPlayer = new HashSet<Collider>();
        setColiderPlayerCarState = (int)ColiderPlayerCarState.playerIsAlive;
    }
    private void OnCollisionEnter(Collision collision)
    {
        /*foreach (ContactPoint contact in collision.contacts)
        {
            Debug.Log( contact.otherCollider.transform.tag);
        }*/
        SwitchActionOnCollision((ColiderPlayerCarState)setColiderPlayerCarState, collision);
    }

    private void SwitchActionOnCollision(ColiderPlayerCarState state, Collision collision)
    {
        switch (state)
        {
            case ColiderPlayerCarState.playerIsAlive:
                break;
            case ColiderPlayerCarState.playerIsDead:
                MakeColidedTafficCarsDestroyed(collision);
                break;
            case ColiderPlayerCarState.playerIsBig:
                
                break;
        }
    }

    private void MakeColidedTafficCarsDestroyed(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.otherCollider.transform.tag == "trafficCarColider")
            {
                if (usedColider.Add(contact.otherCollider))
                {
                    Rigidbody rigidbody = contact.otherCollider.transform.parent.GetComponent<Rigidbody>();
                    ControlSpawnedTrafficCar controlSpawnedTrafficCar = contact.otherCollider.transform.parent.GetComponent<ControlSpawnedTrafficCar>();
                    controlSpawnedTrafficCar.trafficCarState = (int)TrafficCarState.playerIsDead;
                    rigidbody.isKinematic = false;
                    rigidbody.useGravity = true;
                    rigidbody.AddExplosionForce(5f, this.transform.position, 0, 5f, ForceMode.Impulse);
                }
            }
        }
    }
    
}
