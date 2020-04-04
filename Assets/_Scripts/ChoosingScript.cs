using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoosingScript : MonoBehaviour
{
    EggSpown gamePipeline;

    // Start is called before the first frame update
    void Start()
    {
        gamePipeline = GameObject.FindGameObjectWithTag("Game Pipeline").GetComponent<EggSpown>();
    }

    private void Update()
    {
        //print("Works");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Easy") { gamePipeline.difficulty = EggSpown.Difficulty.easy; }
        else if (other.name == "Hard") { gamePipeline.difficulty = EggSpown.Difficulty.hard; }
    }
}
