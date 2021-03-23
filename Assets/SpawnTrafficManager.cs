using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrafficManager : MonoBehaviour
{
    private PointsRailControll pointsRail;
    private TrafficCarContainer trafficCars;
    private PlayerSpeedMove playerSpeed;
    [HideInInspector] public List<Transform> forwardDirectionPoints;
    [HideInInspector] public List<Transform> backWardDirectionPoints;
    private List<GameObject> trafficCarsGameObjects;
    private Queue<GameObject> usedQueueTrafficCars;
    private Transform[] forGetRandomForwardDirectionPoints;
    private Queue<Transform> forQueueGetForwardDirectionPoints;
    private Transform[] forGetRandomBackWardDirectionPoints;
    private Queue<Transform> forQueueGetBackWardDirectionPoints;
    public float zOffsetSpawnTraffic;
    
    
    /*private int usedNumOfForwardDirectionPoints;
    private int usedNumOfBackWardDirectionPoints;*/

    void Start()
    {
        playerSpeed = FindObjectOfType<PlayerSpeedMove>();
        pointsRail = FindObjectOfType<PointsRailControll>();
        forwardDirectionPoints = new List<Transform>() {pointsRail.listRailPoints[3], pointsRail.listRailPoints[4], pointsRail.listRailPoints[5] };
        backWardDirectionPoints = new List<Transform>() {pointsRail.listRailPoints[0], pointsRail.listRailPoints[1], pointsRail.listRailPoints[2] };

        trafficCars = FindObjectOfType<TrafficCarContainer>();
        trafficCarsGameObjects = trafficCars.TrafficCars;

        usedQueueTrafficCars = new Queue<GameObject>();
        InitUsedQueueTrafficCars();

        forGetRandomBackWardDirectionPoints = backWardDirectionPoints.ToArray();
        forGetRandomForwardDirectionPoints = forwardDirectionPoints.ToArray();
        forGetRandomBackWardDirectionPoints = WorkWithArray.GetMixedArray(forGetRandomBackWardDirectionPoints) as Transform[];
        forGetRandomForwardDirectionPoints = WorkWithArray.GetMixedArray(forGetRandomForwardDirectionPoints) as Transform[];

        //Object newObject1 = forGetRandomBackWardDirectionPoints as Object;
        forQueueGetBackWardDirectionPoints = new Queue<Transform>(forGetRandomBackWardDirectionPoints);
        forQueueGetForwardDirectionPoints = new Queue<Transform>(forGetRandomForwardDirectionPoints);

        StartCoroutine(ForwardSpawnObject());
        StartCoroutine(BackwardSpawnObject());
    }

    void Update()
    {
        
    }



    private GameObject GetNextCarFromList()
    {
        GameObject NewCar = usedQueueTrafficCars.Dequeue();
        usedQueueTrafficCars.Enqueue(NewCar);
        return NewCar;
    }

    private Transform GetRandomUnusedRailPoint(int directionForwardBack)
    {
        Transform newTransform;
        if (directionForwardBack == -1)
        {
            newTransform = forQueueGetBackWardDirectionPoints.Dequeue();
            
            if (forQueueGetBackWardDirectionPoints.Count == 0)
            {
                //Debug.Log("jjjj");
                forGetRandomBackWardDirectionPoints = WorkWithArray.GetMixedArray(forGetRandomBackWardDirectionPoints) as Transform[];
                forQueueGetBackWardDirectionPoints = new Queue<Transform>(forGetRandomBackWardDirectionPoints);
            }
            return newTransform;
        }
        else
        {
            newTransform = forQueueGetForwardDirectionPoints.Dequeue();

            if (forQueueGetForwardDirectionPoints.Count == 0)
            {
                forGetRandomForwardDirectionPoints = WorkWithArray.GetMixedArray(forGetRandomForwardDirectionPoints) as Transform[];
                forQueueGetForwardDirectionPoints = new Queue<Transform>(forGetRandomForwardDirectionPoints);
            }

            return newTransform;
        }
    }

    private void InitUsedQueueTrafficCars()
    {
        for (int i = 0; i < trafficCarsGameObjects.Count; i++)
        {
            usedQueueTrafficCars.Enqueue(trafficCarsGameObjects[i]);
        }
    }

    

    private IEnumerator ForwardSpawnObject()
    {
        
        while (true)
        {
            GameObject newInstance = Instantiate(GetNextCarFromList(), GetRandomUnusedRailPoint(1).position + new Vector3(0f,0f,zOffsetSpawnTraffic), Quaternion.identity);
            newInstance.GetComponent<ControlSpawnedTrafficCar>().directionForwardOrBack = 1;
            newInstance.SetActive(true);
            float clampedSpeed = Mathf.Clamp(playerSpeed.multiplayerSpeed / (playerSpeed.maxSpeed / 10f), 1f, 10f);
            yield return new WaitForSeconds(5f/clampedSpeed);
        }
        yield return null;
    }

    private IEnumerator BackwardSpawnObject()
    {

        while (true)
        {
            GameObject newInstance = Instantiate(GetNextCarFromList(), GetRandomUnusedRailPoint(-1).position + new Vector3(0f, 0f, zOffsetSpawnTraffic), Quaternion.identity);
            newInstance.transform.rotation = Quaternion.Euler(0f,-180f,0f);
            newInstance.GetComponent<ControlSpawnedTrafficCar>().directionForwardOrBack = -1;
            newInstance.SetActive(true);
            
            float clampedSpeed = Mathf.Clamp(playerSpeed.multiplayerSpeed/(playerSpeed.maxSpeed/10f), 1f, 10f);
            //Debug.Log(clampedSpeed);
            yield return new WaitForSeconds(5f/clampedSpeed);
        }
        yield return null;
    }
}
