using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleManager : MonoBehaviour
{
    private VirualCameraController cameraController;
    private Cinemachine.CinemachineBrain cinemachineBrain;
    [HideInInspector] public bool isCarGrow;
    [HideInInspector] public int priorityBigCamera;
    [HideInInspector] public int priorityCurrentActiveCamera;
    [HideInInspector] public GameObject linnkToCratedPlayerCar;
    
    private void Start()
    {
        cameraController = FindObjectOfType<VirualCameraController>();
        cinemachineBrain = cameraController.transform.GetComponent<Cinemachine.CinemachineBrain>();
        isCarGrow = false;
    }
    public IEnumerator StartSlowMotion(Transform targetForCamera)
    {
        isCarGrow = true;
        int currentActiveCamera = cinemachineBrain.ActiveVirtualCamera.Priority;
        int destroyedCamera = cameraController.destroyedCarCamera.Priority;
        float originalFixedDeltaTime = Time.fixedDeltaTime;
        cameraController.destroyedCarCamera.LookAt = targetForCamera;
        cameraController.destroyedCarCamera.Follow = linnkToCratedPlayerCar.transform;
        cameraController.destroyedCarCamera.Priority = currentActiveCamera + 1;
        Time.timeScale = 0.05f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        yield return new WaitForSecondsRealtime(2.5f);
        cameraController.destroyedCarCamera.LookAt = null;
        //yield return new WaitForSecondsRealtime(0.1f);
        cameraController.destroyedCarCamera.Priority = -1;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = originalFixedDeltaTime;
        isCarGrow = false;
        yield return null;
    }

    public IEnumerator StartPlayerDeathCam()
    {
        cameraController.playerNormalScaleCamera.Follow = null;
        //cameraController.playerNormalScaleCamera.LookAt = null;
        float originalFixedDeltaTime = Time.fixedDeltaTime;
        Time.timeScale = 0.05f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = originalFixedDeltaTime;
        yield return null;
    }

    public void SetBigCamera(GameObject Link)
    {
        priorityBigCamera = cameraController.whenPlayerBigCamera.Priority;
        priorityCurrentActiveCamera =  cinemachineBrain.ActiveVirtualCamera.Priority;
        cameraController.whenPlayerBigCamera.Priority = priorityCurrentActiveCamera + 1;
        cameraController.whenPlayerBigCamera.LookAt = Link.transform;
        cameraController.whenPlayerBigCamera.Follow = Link.transform;

    }

    public void UnsetBigCamera()
    {
        cameraController.whenPlayerBigCamera.Priority = priorityBigCamera;
    }


}
