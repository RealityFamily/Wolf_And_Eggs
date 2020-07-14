using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject.FindGameObjectWithTag("Game Pipeline").GetComponent<GamePipeline>().chooseDificulty.PutToChoose();
    }

    private void Update()
    {

    }
}
