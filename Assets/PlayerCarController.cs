using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlayerCarControllerState
{
    PlayerIsAlive = 0,
    PlayerIsDead = 1,
    PlayerCatchBonus = 2
}

public class PlayerCarController : MonoBehaviour
{
    private PointsRailControll pointsRail;
    private AccesToObjectsLinks accesToObjectsLinks;
    private CarPropereties playerCarPropereties;
    private ScaleUpUnderCars scaleUpUnderCars;
    [HideInInspector]public GameObject LinkToCreatedPlayerCar;
    [HideInInspector] public int currentPlayerRailIndex;
    [HideInInspector] public int currentPlayerRailBonusIndex;
    [HideInInspector] public int currentPlayerCarControllerState;
    private bool lastMoveLeftRightIenumerator;
    private bool lastScaleDownIsAlready;
    private bool lastScaleUpIsAlready;
    private bool lastMoveLeftRightWhenBigIsAlready;
    private GameObject chassisPlayerCar;
    private GameObject wheelsCar;
    private GameObject springBodyPlayerCar;
    private Vector3 saveScaleCar;
    private SpringBody springBody;
    private VirualCameraController playerCamera;
    private PlayerSpeedMove playerSpeed;
    private Vector3 defaultQuaternionCar;
    [HideInInspector] public bool isCarSmallScale;
    private Vector3 sizeCastBox;
    private Vector3 centerCastBox;
    private CarPropereties carPropereties;
    private TimeScaleManager timeScale;
    private BonusPointsRailControl bonusPointsRail;
    private PlayerDeath playerDeath;
    private PlayerCarColider playerCarColider;
    public LayerMask layerMaskOnBonus;
    private HashSet<Collider> usedColliderInBonus;
    private Cinemachine.CinemachineBrain cinemachineBrain;
    private ParticlesSpawnManger particlesSpawn;
    private CanvasManager canvasManager;
    //private Vector3 saveNormalRotation;
    
    void Start()
    {

        usedColliderInBonus = new HashSet<Collider>();
        currentPlayerCarControllerState = (int)PlayerCarControllerState.PlayerIsAlive;

        playerCamera = FindObjectOfType<VirualCameraController>();
        cinemachineBrain = FindObjectOfType<Cinemachine.CinemachineBrain>();
        playerSpeed = FindObjectOfType<PlayerSpeedMove>();
        bonusPointsRail = FindObjectOfType<BonusPointsRailControl>();
        pointsRail = FindObjectOfType<PointsRailControll>();
        accesToObjectsLinks = FindObjectOfType<AccesToObjectsLinks>();
        timeScale = FindObjectOfType<TimeScaleManager>();
        particlesSpawn = FindObjectOfType<ParticlesSpawnManger>();
        canvasManager = FindObjectOfType<CanvasManager>();

        SpawnPlayerCar(accesToObjectsLinks.carSpaawnManager.carsList[1], out LinkToCreatedPlayerCar);

        isCarSmallScale = false;

        //saveNormalRotation = new Vector3(90f,0f,0f);
        timeScale.linnkToCratedPlayerCar = LinkToCreatedPlayerCar;
        canvasManager.playerCarController = this;
        playerCarPropereties = LinkToCreatedPlayerCar.GetComponent<CarPropereties>();
        playerCarColider = LinkToCreatedPlayerCar.GetComponent<PlayerCarColider>();
        playerDeath = playerCarPropereties.carBoxTrigger.transform.GetComponent<PlayerDeath>();
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
        SwitchUpdatePlayerCarController(currentPlayerCarControllerState);
        
        //Debug.Log(currentPlayerCarControllerState);
    }

    private void FixedUpdate()
    {
        if (currentPlayerCarControllerState == (int)PlayerCarControllerState.PlayerCatchBonus)
        {
            MakeTrafficCarsIsRigidbody();
        }
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
                    //Debug.Log("Left");
                    StartCoroutine(CarMoveLefRight(pointsRail.GetNearRail(currentPlayerRailIndex,-1),-1)) ;
                }
                
                break;
            case Swipes.Right:

                if (lastMoveLeftRightIenumerator == false & isCarSmallScale == false)
                {
                    //Debug.Log("Right");
                    StartCoroutine(CarMoveLefRight(pointsRail.GetNearRail(currentPlayerRailIndex, 1), 1));
                }

                break;

            case Swipes.Up:
                if (lastScaleUpIsAlready == false)
                {
                    StartCoroutine(CarScaleUp());
                    scaleUpUnderCars.CheckCarOnTop(LinkToCreatedPlayerCar.transform, centerCastBox, sizeCastBox);
                    //isCarSmallScale = false;
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

    public void SwitchActionSwipePlayerCarWhenBig(Swipes swipe)
    {
        switch (swipe)
        {
            case Swipes.Left:
                if(lastMoveLeftRightWhenBigIsAlready == false)
                {
                    StartCoroutine(CarMoveLefRightWhenBig(bonusPointsRail.GetNearPoint(1, currentPlayerRailBonusIndex), 1));
                }
                    
                break;
            case Swipes.Right:
                if (lastMoveLeftRightWhenBigIsAlready == false)
                {
                    StartCoroutine(CarMoveLefRightWhenBig(bonusPointsRail.GetNearPoint(-1, currentPlayerRailBonusIndex), -1));
                }
                    
                break;
        }
    }

        private void SwitchUpdatePlayerCarController(int playerCarControllerState)
    {
        switch ((PlayerCarControllerState)playerCarControllerState)
        {
            case PlayerCarControllerState.PlayerIsAlive:
                SwitchActionSwipePlayerCar(SwipeController.direction);
                wheelsCar.transform.rotation = Quaternion.Euler(0f, springBody.localEulerAngles.z, -springBody.localEulerAngles.x);
                break;

            case PlayerCarControllerState.PlayerIsDead:
                //wheelsCar.transform.rotation = Quaternion.Euler(saveNormalRotation);
                break;
            case PlayerCarControllerState.PlayerCatchBonus:
                wheelsCar.transform.rotation = Quaternion.Euler(0f, springBody.localEulerAngles.z, -springBody.localEulerAngles.x);
                SwitchActionSwipePlayerCarWhenBig(SwipeController.direction);
                break;
        }
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
                if (playerDeath.isPlayerDead == true)
                {
                    yield return null;
                    break;
                }
                LinkToCreatedPlayerCar.transform.position = Vector3.Lerp(startPos, endPos, i / 10f);
                yield return new WaitForEndOfFrame();
            }

            lastMoveLeftRightIenumerator = false;
        }
        
        yield return null;
    }

    private IEnumerator CarMoveLefRightWhenBig(Transform destinationPoint, int sideLeftRight)
    {
        
        if (destinationPoint == null)
        {
            yield return null;
        }

        else
        {
            lastMoveLeftRightWhenBigIsAlready = true;
            currentPlayerRailBonusIndex += sideLeftRight;
            Vector3 startPos = LinkToCreatedPlayerCar.transform.position;
            Vector3 endPos = destinationPoint.position;
            lastMoveLeftRightIenumerator = true;
            for (float i = 0f; i <= 10f; i += 50f * Time.deltaTime)
            {
                LinkToCreatedPlayerCar.transform.position = Vector3.Lerp(startPos, endPos, i / 10f);
                yield return new WaitForEndOfFrame();
            }
            lastMoveLeftRightWhenBigIsAlready = false;
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
        isCarSmallScale = false;
        yield return null;
    }


    public IEnumerator StartInitialBonusWork()
    {
        //yield return new WaitForSecondsRealtime(10f);
        yield return new WaitWhile(()=> lastMoveLeftRightIenumerator == true);
        yield return new WaitWhile(()=> isCarSmallScale == true);
        playerDeath.currentPlayerTriggerResponsibilities = (int)PlayerTriggerResponsibilities.playerCatchBonus;
        timeScale.SetBigCamera(LinkToCreatedPlayerCar);
        carPropereties.wheelsContainer.transform.localRotation = Quaternion.Euler(90f, 90f, 90f);
        playerCarColider.setColiderPlayerCarState = (int)ColiderPlayerCarState.playerIsBig;
        playerCarPropereties.carBoxColider.enabled = true;
        Vector3 targetPos = bonusPointsRail.GetInitialStartBonusPoint(pointsRail.listRailPoints[currentPlayerRailIndex], out currentPlayerRailBonusIndex).position;
        Vector3 targetScale = LinkToCreatedPlayerCar.transform.localScale * 2f;
        Vector3 currentPos = LinkToCreatedPlayerCar.transform.position;
        Vector3 currentScale = LinkToCreatedPlayerCar.transform.localScale;
        currentPlayerCarControllerState = (int)PlayerCarControllerState.PlayerCatchBonus;
        for (float i = 0f; i <= 10f; i += 50f * Time.deltaTime)
        {
            LinkToCreatedPlayerCar.transform.position = Vector3.Lerp(currentPos, targetPos, i/10f);
            LinkToCreatedPlayerCar.transform.localScale = Vector3.Lerp(currentScale, targetScale, i / 10f);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(10f);
        StartCoroutine(EndOfActionBonusWork(currentScale));
        yield return null;
    }

    public IEnumerator EndOfActionBonusWork(Vector3 normalScale)
    {
        Vector3 currentScale = LinkToCreatedPlayerCar.transform.localScale;
        Vector3 currentPos = LinkToCreatedPlayerCar.transform.position;
        Vector3 targetPos = bonusPointsRail.GetOutNormalRailPoint(currentPlayerRailBonusIndex, out currentPlayerRailIndex).position;

        for (float i = 0f; i <= 10f; i += 50f * Time.deltaTime)
        {
            LinkToCreatedPlayerCar.transform.position = Vector3.Lerp(currentPos, targetPos, i / 10f);
            LinkToCreatedPlayerCar.transform.localScale = Vector3.Lerp(currentScale, normalScale, i / 10f);
            yield return new WaitForEndOfFrame();
        }
        timeScale.UnsetBigCamera();
        lastMoveLeftRightIenumerator = false;
        isCarSmallScale = false;
        playerCarColider.setColiderPlayerCarState = (int)ColiderPlayerCarState.playerIsAlive;
        currentPlayerCarControllerState = (int)PlayerCarControllerState.PlayerIsAlive;
        playerCarPropereties.carBoxColider.enabled = false;
        playerDeath.currentPlayerTriggerResponsibilities = (int)PlayerTriggerResponsibilities.playerWaitForDie;
        yield return null;
    }

    private void MakeTrafficCarsIsRigidbody()
    {
        Collider[] hitColliders = Physics.OverlapBox(LinkToCreatedPlayerCar.transform.position, new Vector3(sizeCastBox.x, sizeCastBox.y, sizeCastBox.z*1.5f), Quaternion.identity, layerMaskOnBonus);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (usedColliderInBonus.Add(hitColliders[i]))
            {
                Rigidbody currentRigidbody = hitColliders[i].attachedRigidbody;
                ControlSpawnedTrafficCar controlSpawnedTrafficCar = hitColliders[i].transform.parent.GetComponent<ControlSpawnedTrafficCar>();
                particlesSpawn.SpawnBigExplosion(hitColliders[i].transform.parent.gameObject);
                controlSpawnedTrafficCar.trafficCarState = (int)TrafficCarState.playerIsDead;
                currentRigidbody.isKinematic = false;
                currentRigidbody.useGravity = true;
                currentRigidbody.AddExplosionForce(20f, LinkToCreatedPlayerCar.transform.position, 0, 20f, ForceMode.Impulse);
            }
        }
    }

    public void StartCoroutineInitiAlBonusWork()
    { StartCoroutine(StartInitialBonusWork()); }
}
