using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EggSpown : MonoBehaviour
{
    public enum Difficulty { none, easy, hard }

    GameObject spawnPoint1;
    GameObject spawnPoint2;
    GameObject spawnPoint3;
    public GameObject egg;
    public GameObject bomb;

    BoxCollider LH;
    BoxCollider RH;
    Text HealthWatch;
    Text ScoreWatch;

    [SerializeField]
    int Round = 1;
    [SerializeField]
    int score = 0;
    [SerializeField]
    int Health = 5;

    GameObject player;
    GameObject Leap;
    GameObject VR;
    GameObject Teacher;

    [SerializeField]
    Transform chooseDifficultyPlace;
    [SerializeField]
    Transform gamePlace;
    public Difficulty difficulty = Difficulty.none;
    [SerializeField]
    GameObject Countdown;

    GameObject zayac;
    bool ready = false;
    //аудиоклипы для обучения
    [SerializeField]
    List<AudioClip> TreaningWithControllers;
    [SerializeField]
    List<AudioClip> TreaningWithLeap;

    void Start()
    {
        StartCoroutine(GameTimeLine());
    }

    IEnumerator GameTimeLine ()
    {
        // постанока человека перед выбором
        PutToChoose();

        // ожидание выбора игрока, чем играть он хочет
        float time = Time.time;
        yield return new WaitUntil(() => difficulty != Difficulty.none || Time.time - time >= 10);
        if (difficulty == Difficulty.none)
        {
            Countdown.SetActive(true);
            Text intCountdown = GameObject.FindGameObjectWithTag("Countdown").transform.GetChild(1).GetComponent<Text>();
            for (int i = 5; i > -1; i--)
            {
                intCountdown.text = i.ToString();
                yield return new WaitForSeconds(1);
            }
            if (difficulty == Difficulty.none)
            {
                difficulty = Difficulty.easy;
            }
        }

        print("init");
        // инициализация всех необходимых аппаратных средств внутри игры
        Initialize();

        print("learn");
        // начало обучения игрока
        zayac = GameObject.FindGameObjectWithTag("Zayac");

        if (difficulty == Difficulty.easy)
        {
            int i = 0;
            // приветствие. объяснение выбранного режима. проба сжатия кулака на левой руке с контроллером.
            i = ZayacTalk(i);
            while (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) < 0.75f && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) < 0.75f) { yield return new WaitForSeconds(0.01f); }
            print("right");
            // теперь правой рукой с контроллером
            i = ZayacTalk(i);
            while (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) < 0.75f && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) < 0.75f) { yield return new WaitForSeconds(0.01f); }
            // теперь сжатие обоих рук без контроллеров, чтобы игрок привык к анимации
            i = ZayacTalk(i);
            VR.GetComponent<OvrAvatar>().StartWithControllers = false;
            VR.transform.GetChild(0).gameObject.SetActive(false);
            VR.transform.GetChild(1).gameObject.SetActive(true);
            while (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) < 0.75f && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) < 0.75f && OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) < 0.75f && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) < 0.75f) { yield return new WaitForSeconds(0.01f); }


        }
        else if (difficulty == Difficulty.hard)
        {

        }

        // инициализация часов игрока
        SetWatch();

        // сама игра
        while (Health > 0 && Round < 10)
        {
            Spawn();
            if (score >= 100) { Round += 1; score -= 100; }
            yield return new WaitForSeconds(GiveTime());
        }

        // демонстрация счета (пока под вопросом) и кнопка перезапуска игры
        ShowScore();

        //yield return null; //заглушка для проверки
    }

    private void Initialize()
    {
        Teacher.SetActive(false);

        if (difficulty == Difficulty.easy) { VR.SetActive(true); }
        else if (difficulty == Difficulty.hard) { Leap.SetActive(true); }
        player.transform.position = gamePlace.position;
        player.transform.rotation = gamePlace.rotation;

        spawnPoint1 = transform.GetChild(0).gameObject;
        spawnPoint2 = transform.GetChild(1).gameObject;
        spawnPoint3 = transform.GetChild(2).gameObject;

        LH = GameObject.FindGameObjectWithTag("Left Hand").GetComponent<BoxCollider>();
        RH = GameObject.FindGameObjectWithTag("Right Hand").GetComponent<BoxCollider>();

        HealthWatch = GameObject.FindGameObjectWithTag("Life Watch").GetComponentInChildren<Text>();
        ScoreWatch = GameObject.FindGameObjectWithTag("Score Watch").GetComponentInChildren<Text>();
    }

    private void PutToChoose()
    {
        player = GameObject.FindGameObjectWithTag("player");
        Leap = player.transform.GetChild(0).gameObject;
        VR = player.transform.GetChild(1).gameObject;
        Teacher = player.transform.GetChild(2).gameObject;

        VR.SetActive(false);
        Teacher.SetActive(true);

        player.transform.position = chooseDifficultyPlace.position;
        player.transform.rotation = chooseDifficultyPlace.rotation;
    }

    private void SetWatch()
    {
        HealthWatch.text = Health.ToString();
        ScoreWatch.text = score.ToString();
    }

    private void ShowScore()
    {
        LH.isTrigger = false;
        RH.isTrigger = false;
    }

    private int ZayacTalk(int i) // скрипт, чтобы не писать каждый отдельный взятый раз болтовню зайца. В массивах аудиозаписи должны лежать попорядку
    {
        //if (difficulty == Difficulty.easy) { zayac.GetComponent<AudioSource>().PlayOneShot(TreaningWithControllers[i]); }
        //else if (difficulty == Difficulty.hard) { zayac.GetComponent<AudioSource>().PlayOneShot(TreaningWithLeap[i]); }

        return i++;
    }

    private void Spawn()
    {
        GameObject[] spawn_things = new GameObject[] { egg, bomb, egg };
        GameObject[] spawn_place = new GameObject[] { spawnPoint1, spawnPoint2, spawnPoint3 };

        Instantiate(spawn_things[Random.Range(0,2)], spawn_place[Random.Range(0, 3)].transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
    }

    private float GiveTime()
    {
        float t = (10 - Round) * 0.5f;
        return t;
    }

    public void AddScore (int score_to_add)
    {
        score += score_to_add;
        ScoreWatch.text = (score + Round * 100).ToString();
    }

    public void ChangeHealth(int delta)
    {
        if (delta == 0) { Health = 0; }
        else { Health += delta; }
        HealthWatch.text = Health.ToString();
    }
}