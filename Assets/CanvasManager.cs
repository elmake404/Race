using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    public Image normalStatusBar;
    public Image completeImageBar;
    public Image whiteImageBar;
    public Text scoreText;
    public Button restartButton;
    [HideInInspector] public PlayerCarController playerCarController;
    public delegate void OnDestructOtherCars();
    public event OnDestructOtherCars onActionDestructOtherCars;
    [HideInInspector] public bool isBonusIsStart;
    private PlayerSpeedMove playerSpeedMove;
    private int totalDistance;


    private void Start()
    {
        playerSpeedMove = FindObjectOfType<PlayerSpeedMove>();
        
        //playerCarController = FindObjectOfType<PlayerCarController>();
        isBonusIsStart = false;
        normalStatusBar.fillAmount = 0.1f;
        completeImageBar.fillAmount = 0f;
        whiteImageBar.fillAmount = 0f;
        restartButton.onClick.AddListener(RestartScene);

        onActionDestructOtherCars += StartCoroutineGrowNormalStatusBar;
        StartCoroutine(WaitFilledStatusBar());
        StartCoroutine(CalcDistance());
    }

    private IEnumerator CalcDistance()
    {
        
        while (true)
        {
            int addDistance = 0;
            addDistance =  Mathf.RoundToInt(10f*playerSpeedMove.multiplayerSpeed*Time.deltaTime);
            totalDistance += addDistance; 
            scoreText.text = totalDistance.ToString() + "m";
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

    public IEnumerator GrowNormalStatusBar()
    {
        yield return new WaitForSeconds(1f);
        if (normalStatusBar.fillAmount >= 0.9f)
        {
            yield return null;
        }
        float offset = 0f;
        float currentFillValue = normalStatusBar.fillAmount;
        for (int i = 0; i <= 200; i++)
        {
            if (offset > 0.2f)
            {
                break;
            }
            if (isBonusIsStart == true)
            {
                normalStatusBar.fillAmount = 0.1f;
                break;
            }
            offset += Time.deltaTime/2f;
            normalStatusBar.fillAmount += Time.deltaTime/2f;
            yield return new WaitForEndOfFrame();
        }
        
        yield return null;
    }

    public IEnumerator WaitFilledStatusBar()
    {
        yield return new WaitWhile(()=>normalStatusBar.fillAmount<0.9f);
        isBonusIsStart = true;
        normalStatusBar.fillAmount = 0.1f;
        playerCarController.StartCoroutineInitiAlBonusWork();
        StartCoroutine(WaitFilledStatusBar());
        StartCoroutine(SubstractCompleteLine());
        yield return null;
    }

    private IEnumerator SubstractCompleteLine()
    {
        Color currentColor = completeImageBar.color;
        currentColor.a = 1f;
        completeImageBar.color = currentColor;
        for (float i = 0f; i < 10f; i += Time.deltaTime)
        {
            completeImageBar.fillAmount = Mathf.Lerp(0.9f, 0.1f, i/10f);
            yield return new WaitForEndOfFrame();
        }
        currentColor.a = 0f;
        completeImageBar.color = currentColor;
        isBonusIsStart = false;
        yield return null;
    }

    private void StartCoroutineGrowNormalStatusBar()
    {
        StartCoroutine(GrowNormalStatusBar());
    }

    public void InvokeOnActionDestructOtherCars()
    {
        onActionDestructOtherCars?.Invoke();
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(1,LoadSceneMode.Single);
    }
}
