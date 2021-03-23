using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarController : MonoBehaviour
{
    private PointsRailControll pointsRail;
    private AccesToObjectsLinks accesToObjectsLinks;
    private CarPropereties playerCarPropereties;
    private ScaleUpUnderCars scaleUpUnderCars;
    public GameObject LinkToCreatedPlayerCar;
    [HideInInspector] public int currentPlayerRailIndex;
    private bool lastMoveLeftRightIenumerator;
    private bool lastScaleDownIsAlready;
    private bool lastScaleUpIsAlready;
    private GameObject chassisPlayerCar;
    private GameObject wheelsCar;
    private GameObject springBodyPlayerCar;
    private Vector3 saveScaleCar;
    private SpringBody springBody;
    private VirualCameraController playerCamera;
    private PlayerSpeedMove playerSpeed;
    private Vector3 defaultQuaternionCar;
    private bool isCarSmallScale;
    private Vector3 sizeCastBox;
    private Vector3 centerCastBox;
    private CarPropereties carPropereties;


    void Start()
    {
        playerCamera = FindObjectOfType<VirualCameraController>();

        playerSpeed = FindObjectOfType<PlayerSpeedMove>();

        pointsRail = FindObjectOfType<PointsRailControll>();
        accesToObjectsLinks = FindObjectOfType<AccesToObjectsLinks>();


        SpawnPlayerCar(accesToObjectsLinks.carSpaawnManager.carsList[1], out LinkToCreatedPlayerCar);

        isCarSmallScale = false;
        playerCarPropereties = LinkToCreatedPlayerCar.GetComponent<CarPropereties>();
        scaleUpUnderCars = LinkToCreatedPlayerCar.GetComponent<ScaleUpUnderCars>();
        chassisPlayerCar = playerCarPropereties.chassis;
        wheelsCar = playerCarPropereties.wheelsContainer;
        springBodyPlayerCar = playerCarPropereties.springBodyObject;
        springBody = springBodyPlayerCar.GetComponent<SpringBody>();
        defaultQuaternionCar = chassisPlayerCar.transform.rotation.eulerAngles;
        saveScaleCar = LinkToCreatedPlayerCar.transform.localScale;

        carPropereties = LinkToCreatedPlayerCar.GetComponent<CarPropereties>();
        sizeCastBox = carPropereties.playerCarMeshBounds;
        centerCastBox = carPropereties.playerCarMeshCenter;

        playerCamera.playerNormalScaleCamera.Follow = LinkToCreatedPlayerCar.transform;
        playerCamera.playerNormalScaleCamera.LookAt = LinkToCreatedPlayerCar.transform;
        playerCamera.playerSmallScaleCamera.Follow = LinkToCreatedPlayerCar.transform;
        playerCamera.playerSmallScaleCamera.LookAt = LinkToCreatedPlayerCar.transform;
    }


    void Update()
    {
        SwitchActionSwipePlayerCar(SwipeController.direction);

        
        wheelsCar.transform.rotation = Quaternion.Euler(0f , springBody.localEulerAngles.z, -springBody.localEulerAngles.x) * Quaternion.Euler(new Vector3(0f, 0f, -playerSpeed.velocity/3f));
        
        
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

                if (lastMoveLeftRightIenumerator == false & isCarSmallScale == false)
                {
                    StartCoroutine(CarMoveLefRight(pointsRail.GetNearRail(currentPlayerRailIndex,-1),-1)) ;
                }
                
                break;
            case Swipes.Right:

                if (lastMoveLeftRightIenumerator == false & isCarSmallScale == false)
                {
                    StartCoroutine(CarMoveLefRight(pointsRail.GetNearRail(currentPlayerRailIndex, 1), 1));
                }

                break;

            case Swipes.Up:
                if (lastScaleUpIsAlready == false)
                {
                    StartCoroutine(CarScaleUp());
                    scaleUpUnderCars.CheckCarOnTop(LinkToCreatedPlayerCar.transform, centerCastBox, sizeCastBox);
                    isCarSmallScale = false;
                    playerSpeed.targetSpeed = playerSpeed.maxSpeed;
                    playerSpeed.damping = playerSpeed.normalDamping;
                    playerCamera.playerNormalScaleCamera.Priority = 1;
                    playerCamera.playerSmallScaleCamera.Priority = 0;
                }
                
                break;

            case Swipes.Down:
                if (lastScaleDownIsAlready == false)
                {
                    StartCoroutine(CarScaleDown());
                    isCarSmallScale = true;
                    playerSpeed.targetSpeed = 2f;
                    playerSpeed.damping = 1f;
                    playerCamera.playerNormalScaleCamera.Priority = 0;
                    playerCamera.playerSmallScaleCamera.Priority = 1;
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

    private IEnumerator CarScaleDown()
    {
        lastScaleDownIsAlready = true;
        
        Vector3 startScale = saveScaleCar;
        Vector3 newScale = new Vector3(0.05f, 0.05f, 0.05f);

        for (float i = 0f; i <= 10f; i += 50f * Time.deltaTime)
        {
            LinkToCreatedPlayerCar.transform.localScale = Vector3.Lerp(startScale, newScale, i/10f);
            yield return new WaitForEndOfFrame();
        }
        lastScaleUpIsAlready = false;
        yield return null;
    }

    private IEnumerator CarScaleUp()
    {
        lastScaleUpIsAlready = true;
        
        Vector3 startScale = LinkToCreatedPlayerCar.transform.localScale;
        Vector3 newScale = saveScaleCar;

        for (float i = 0f; i <= 10f; i += 50f * Time.deltaTime)
        {
            LinkToCreatedPlayerCar.transform.localScale = Vector3.Lerp(startScale, newScale, i / 10f);
            yield return new WaitForEndOfFrame();
        }
        lastScaleDownIsAlready = false;
        yield return null;
    }
}
