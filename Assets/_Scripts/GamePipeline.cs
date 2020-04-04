﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePipeline : MonoBehaviour
{
    public enum Difficulty_Type { none, easy, hard }
    public Difficulty_Type Difficulty { get { return difficulty; } set { difficulty = value; } }

    [SerializeField]
    PlayerLogic playerLogic;
    [SerializeField]
    Choose_Difficulty chooseDificulty;
    [SerializeField]
    Learning learning;
    [SerializeField]
    EggSpown eggSpawn;

    Difficulty_Type difficulty = Difficulty_Type.none;

    // Start is called before the first frame update
    void Start()
    {
        difficulty = Difficulty_Type.none;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Reload();
        }
    }

    public void Reload()
    {
        StopCoroutine(GameTimeLine());
        StartCoroutine(GameTimeLine());
    }

    IEnumerator GameTimeLine()
    {
        chooseDificulty.PutToChoose();
        yield return new WaitWhile(() => difficulty == Difficulty_Type.none);
        playerLogic.putToGamePlace();
        learning.Start_Learning();
        yield return new WaitWhile(() => learning.Ready == false);
        eggSpawn.StartPlay();
        yield return new WaitWhile(() => eggSpawn.Playing == true);
        playerLogic.ShowScore();
    }
}
