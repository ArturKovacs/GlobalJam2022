using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //TEST
        if (Input.GetKeyDown(KeyCode.F2))
        {
            ReloadGame();
        }
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(0);
    }
}
