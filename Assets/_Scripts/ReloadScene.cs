using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    private GameObject scoreCount;
    private GameObject eggSpown;

    private void Awake()
    {
        scoreCount = GameObject.Find("EggTrigger");
        eggSpown = GameObject.Find("EggSpown");
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Reload");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            scoreCount.GetComponent<ScoreCounter>().score.text = "0";
            eggSpown.GetComponent<EggSpown>().isPaused = true;
        }
    }
}
