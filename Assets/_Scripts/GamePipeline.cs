using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GamePipeline : MonoBehaviour
{
    public enum Difficulty_Type { none, easy, hard }
    public Difficulty_Type Difficulty { get { return difficulty; } set { difficulty = value; } }

    public PlayerLogic playerLogic;
    public Choose_Difficulty chooseDificulty;
    public Learning learning;
    public EggSpown eggSpawn;


    [Header("For debugging:")]
    public bool NeedTimeCount = true;
    public bool WithoutLearning = false;
    [SerializeField]
    Difficulty_Type difficulty = Difficulty_Type.none;
    

    // Start is called before the first frame update
    void Start()
    {
        difficulty = Difficulty_Type.none;
        chooseDificulty.PutToChoose();

        #region StateControl Logic
        WebConnection webConnection = GameObject.FindGameObjectWithTag("States").GetComponent<WebConnection>();
        webConnection.ConnectToDelegate("Reload", () => {
            StopAllCoroutines();
            chooseDificulty.PutToChoose(); 
        });
        webConnection.ConnectToDelegate("\"Controlers\" mode", () => {
            StopAllCoroutines();
            difficulty = Difficulty_Type.easy;
            playerLogic.putToGamePlace();
        });
        webConnection.ConnectToDelegate("\"Hand\" mode", () => {
            StopAllCoroutines();
            difficulty = Difficulty_Type.hard;
            playerLogic.putToGamePlace();
        });
        webConnection.ConnectToDelegate("Game Start", () => {
            StopAllCoroutines();
            eggSpawn.StartPlay();
        });
        webConnection.ConnectToDelegate("Game End", () => {
            StopAllCoroutines();
            playerLogic.ShowScore();
        });
        #endregion

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            chooseDificulty.PutToChoose();
        }
    }

    public new void StopAllCoroutines()
    {
        playerLogic.StopAllCoroutines();
        chooseDificulty.StopAllCoroutines();
        eggSpawn.StopAllCoroutines();
        learning.ShutUp();
        learning.StopAllCoroutines();
    }
}
