using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarController : MonoBehaviour
{
    private PointsRailControll pointsRail;
    private AccesToObjectsLinks accesToObjectsLinks;
    private CarPropereties playerCarPropereties;
    public GameObject LinkToCreatedPlayerCar;
    [HideInInspector] public int currentPlayerRailIndex;
    private bool lastMoveLeftRightIenumerator;
    private GameObject chassisPlayerCar;
    private GameObject wheelsCar;
    private GameObject springBodyPlayerCar;
    private SpringBody springBody;
    private Vector3 defaultQuaternionCar;


    void Start()
    {
        pointsRail = FindObjectOfType<PointsRailControll>();
        accesToObjectsLinks = FindObjectOfType<AccesToObjectsLinks>();


        SpawnPlayerCar(accesToObjectsLinks.carSpaawnManager.carsList[1], out LinkToCreatedPlayerCar);
        

        playerCarPropereties = LinkToCreatedPlayerCar.GetComponent<CarPropereties>();
        chassisPlayerCar = playerCarPropereties.chassis;
        wheelsCar = playerCarPropereties.wheelsContainer;
        springBodyPlayerCar = playerCarPropereties.springBodyObject;
        springBody = springBodyPlayerCar.GetComponent<SpringBody>();
        defaultQuaternionCar = chassisPlayerCar.transform.rotation.eulerAngles;
    }


    void Update()
    {
        SwitchActionSwipePlayerCar(SwipeController.direction);

        //Debug.Log(springBody.localEulerAngles);
        //Debug.Log(defaultQuaternionCar);

        //chassisPlayerCar.transform.localEulerAngles = springBody.localEulerAngles + defaultQuaternionCar + new Vector3(springBody.localEulerAngles.z,0f, -springBody.localEulerAngles.z);

        wheelsCar.transform.rotation = Quaternion.Euler(0f, springBody.localEulerAngles.z, -springBody.localEulerAngles.x);
        
    }

    private void SpawnPlayerCar(GameObject carToSpawn, out GameObject newInstance)
    {
        newInstance = Instantiate(carToSpawn, pointsRail.listRailPoints[3].position, Quaternion.identity);
        newInstance.SetActive(true);
        currentPlayerRailIndex = 3;

        Debug.Log("dvs");
    }

    public void SwitchActionSwipePlayerCar(Swipes swipe)
    {
        switch (swipe)
        {
            case Swipes.Left:

                if (lastMoveLeftRightIenumerator == false)
                {
                    StartCoroutine(CarMoveLefRight(pointsRail.GetNearRail(currentPlayerRailIndex,-1),-1)) ;
                }
                
                break;
            case Swipes.Right:

                if (lastMoveLeftRightIenumerator == false)
                {
                    StartCoroutine(CarMoveLefRight(pointsRail.GetNearRail(currentPlayerRailIndex, 1), 1));
                }

                break;
        }

        //default: return null;
    }



    private IEnumerator CarMoveLefRight(Transform destinationPoint, int sideLeftRight)
    {

        if (destinationPoint == null)
        {
            yield return null;
        }

        else
        {
            currentPlayerRailIndex += sideLeftRight;
            Vector3 startPos = LinkToCreatedPlayerCar.transform.position;
            Vector3 endPos = destinationPoint.position;

            lastMoveLeftRightIenumerator = true;

            for (float i = 0f; i <= 10f; i += 50f * Time.deltaTime)
            {
                LinkToCreatedPlayerCar.transform.position = Vector3.Lerp(startPos, endPos, i / 10f);
                yield return new WaitForEndOfFrame();
            }

            lastMoveLeftRightIenumerator = false;
        }
        
        yield return null;
    }
}
