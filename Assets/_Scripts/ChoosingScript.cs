using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoosingScript : MonoBehaviour
{
    GamePipeline gamePipeline;

    // Start is called before the first frame update
    void Start()
    {
        gamePipeline = GameObject.FindGameObjectWithTag("Game Pipeline").GetComponent<GamePipeline>();
    }

    private void Update()
    {
        //print("Works");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Easy") { gamePipeline.Difficulty = GamePipeline.Difficulty_Type.easy; }
        else if (other.name == "Hard") { gamePipeline.Difficulty = GamePipeline.Difficulty_Type.hard; }
    }
}
