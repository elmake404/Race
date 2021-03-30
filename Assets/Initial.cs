using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Initial : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        FacebookManager.Instance.GameStart();
        SceneManager.LoadScene(1,LoadSceneMode.Single);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
