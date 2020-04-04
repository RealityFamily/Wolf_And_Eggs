using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GamePipeline;

public class Choose_Difficulty : MonoBehaviour
{
    public GamePipeline gamePipeline;

    GameObject player;
    [SerializeField]
    Transform chooseDifficultyPlace;
    [SerializeField]
    GameObject Countdown;

    public void PutToChoose()
    {
        player = GameObject.FindGameObjectWithTag("player");

        player.transform.position = chooseDifficultyPlace.position;
        player.transform.rotation = chooseDifficultyPlace.rotation;

        if (gamePipeline.NeedTimeCount) {
            StartCoroutine(WaitForAnswer());
        }
    }

    IEnumerator WaitForAnswer()
    {
        
        float time = Time.time;
        yield return new WaitUntil(() => gamePipeline.Difficulty != Difficulty_Type.none || Time.time - time >= 10);
        if (gamePipeline.Difficulty == Difficulty_Type.none)
        {
            Countdown.SetActive(true);
            Text intCountdown = Countdown.transform.GetChild(1).GetComponent<Text>();
            for (int t = 5; t > -1; t--)
            {
                intCountdown.text = t.ToString();
                yield return new WaitForSeconds(1);
            }
            if (gamePipeline.Difficulty == Difficulty_Type.none)
            {
                gamePipeline.Difficulty = Difficulty_Type.easy;
            }
        }
        
    }
}
