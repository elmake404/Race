using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesSpawnManger : MonoBehaviour
{
    public GameObject bigExplosion;


    public void SpawnBigExplosion(GameObject targetTransform)
    {
        GameObject newInstance = Instantiate(bigExplosion);
        newInstance.transform.position = targetTransform.transform.position;
        newInstance.SetActive(true);
        
        //newInstance.GetComponent<BigExplosionInstanceControl>().targetToFollow = targetTransform.transform.position;
        
    }

    private IEnumerator DeleteObjectAfterSpawn(GameObject gameObject)
    {
        yield return new WaitForSecondsRealtime(2f);
        Destroy(gameObject);
        
        yield return null;
    }
}


