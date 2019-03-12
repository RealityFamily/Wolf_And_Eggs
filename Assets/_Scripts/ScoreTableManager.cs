using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTableManager : MonoBehaviour
{
    public GameObject playerScore;
    public GameObject playerScore2;
    public GameObject playerScore3;
    public GameObject playerScore4;
    public GameObject playerScore5;
    private void Start()
    {
        playerScore.transform.Find("UsernameTitle").GetComponent<Text>().text = "ValiKEK";
        playerScore.transform.Find("ScoreTitle").GetComponent<Text>().text = "5000";

        playerScore2.transform.Find("UsernameTitle").GetComponent<Text>().text = "Maximka";
        playerScore2.transform.Find("ScoreTitle").GetComponent<Text>().text = "1340";

        playerScore3.transform.Find("UsernameTitle").GetComponent<Text>().text = "Meshh";
        playerScore3.transform.Find("ScoreTitle").GetComponent<Text>().text = "790";

        playerScore4.transform.Find("UsernameTitle").GetComponent<Text>().text = "Eblanizzz";
        playerScore4.transform.Find("ScoreTitle").GetComponent<Text>().text = "560";

        playerScore5.transform.Find("UsernameTitle").GetComponent<Text>().text = "KEKAR";
        playerScore5.transform.Find("ScoreTitle").GetComponent<Text>().text = "100";
    }
}
