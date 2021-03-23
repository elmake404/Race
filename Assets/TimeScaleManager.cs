using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleManager : MonoBehaviour
{
    private VirualCameraController cameraController;
    private Cinemachine.CinemachineBrain cinemachineBrain;
    private void Start()
    {
        cameraController = FindObjectOfType<VirualCameraController>();
        cinemachineBrain = cameraController.transform.GetComponent<Cinemachine.CinemachineBrain>();
        
    }
    public IEnumerator StartSlowMotion(Transform targetForCamera)
    {
        int currentActiveCamera = cinemachineBrain.ActiveVirtualCamera.Priority;
        int destroyedCamera = cameraController.destroyedCarCamera.Priority;
        float originalFixedDeltaTime = Time.fixedDeltaTime;
        cameraController.destroyedCarCamera.LookAt = targetForCamera;
        cameraController.destroyedCarCamera.Priority = currentActiveCamera + 1;
        Time.timeScale = 0.05f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        yield return new WaitForSecondsRealtime(2.5f);
        cameraController.destroyedCarCamera.LookAt = null;
        //yield return new WaitForSecondsRealtime(0.1f);
        cameraController.destroyedCarCamera.Priority = -1;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = originalFixedDeltaTime;

        yield return null;
    }
}
