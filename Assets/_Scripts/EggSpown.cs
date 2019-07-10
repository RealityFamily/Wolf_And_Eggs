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
    AudioClip HeartAdd;
    [SerializeField]
    AudioClip HeartCrush;

    [SerializeField]
    int Round = 1;
    [SerializeField]
    int score = 0;
    [SerializeField]
    int Health = 5;
    bool comboFail = false;

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
    List<AudioClip> TreaningDialog;
    [SerializeField]
    List<AudioClip> TouchPart;
    [SerializeField]
    List<AudioClip> LeapPart;

    void Start()
    {
        StartCoroutine(GameTimeLine());
    }

    IEnumerator GameTimeLine ()
    {
        /***************************************************************/
        /******** Постоновка человека перед выбором чем играть *********/
        /***************************************************************/
        PutToChoose();

        /***************************************************************/
        /********* ожидание выбора игрока, чем играть он хочет *********/
        /***************************************************************/
        float time = Time.time;
        yield return new WaitUntil(() => difficulty != Difficulty.none || Time.time - time >= 10);
        if (difficulty == Difficulty.none)
        {
            Countdown.SetActive(true);
            Text intCountdown = GameObject.FindGameObjectWithTag("Countdown").transform.GetChild(1).GetComponent<Text>();
            for (int t = 5; t > -1; t--)
            {
                intCountdown.text = t.ToString();
                yield return new WaitForSeconds(1);
            }
            if (difficulty == Difficulty.none)
            {
                difficulty = Difficulty.easy;
            }
        }

        /*****************************************************************/
        /* инициализация всех необходимых аппаратных средств внутри игры */
        /*****************************************************************/
        Initialize();

        /***************************************************************/
        /****************** начало обучения игрока *********************/
        /***************************************************************/
        int i = 0;
        // приветствие.
        i = ZayacTalk(i, Difficulty.none);
        // объяснение выбранного режима.
        i = ZayacTalk(i, Difficulty.none);
        if (difficulty == Difficulty.easy)
        {
            int j = 0;
            // окончание фразы о выбранном режиме
            j = ZayacTalk(j, difficulty);
            //проба сжатия кулака на левой руке с контроллером.
            j = ZayacTalk(j, difficulty);
            while (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) < 0.75f && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) < 0.75f) { yield return new WaitForSeconds(0.01f); }
            // теперь правой рукой с контроллером
            j = ZayacTalk(j, difficulty);
            while (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) < 0.75f && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) < 0.75f) { yield return new WaitForSeconds(0.01f); }
            // теперь сжатие обоих рук без контроллеров, чтобы игрок привык к анимации
            j = ZayacTalk(j, difficulty);
            VR.transform.GetChild(0).gameObject.SetActive(false);
            VR.transform.GetChild(1).gameObject.SetActive(true);
            while (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) < 0.75f && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) < 0.75f && OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) < 0.75f && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) < 0.75f) { yield return new WaitForSeconds(0.01f); }
        }
        else if (difficulty == Difficulty.hard)
        {
            int j = 0;
            // окончание фразы о выбранном режиме
            j = ZayacTalk(j, difficulty);
            //проба сжатия руками кулака.
            j = ZayacTalk(j, difficulty);
            while (true) { yield return new WaitForSeconds(0.01f); }
        }
        // объяснение смысла игры.
        i = ZayacTalk(i, Difficulty.none);
        // запуск яйцо по центру и просьба бросить его в корзину.
        i = ZayacTalk(i, Difficulty.none);
        int t_score = score;
        while (t_score == score)
        {
            Instantiate(egg, spawnPoint2.transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
            yield return new WaitForSeconds(5);
        }
        // объяснение часов со счетом
        i = ZayacTalk(i, Difficulty.none);
        // а теперь запускаем такое же яйцо, которое не возможно взять
        i = ZayacTalk(i, Difficulty.none);
        GameObject t_egg = egg;
        t_egg.GetComponent<OVRGrabbable>().enabled = false;
        Instantiate(t_egg, spawnPoint2.transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
        yield return new WaitForSeconds(5);
        // объясние часов с жизнью
        i = ZayacTalk(i, Difficulty.none);
        // комбо из 10 яиц => жизнь++
        i = ZayacTalk(i, Difficulty.none);
        // договариваемся о возврящении потеренной жизни
        i = ZayacTalk(i, Difficulty.none);
        ChangeHealth(1);
        // ввод игрока в игру по показу знака "класс"
        i = ZayacTalk(i, Difficulty.none);
        if (difficulty == Difficulty.easy)
        {
            time = Time.time;
            yield return new WaitUntil(() => (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) >= 0.75f && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) >= 0.75f && OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) >= 0.75f && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) >= 0.75f) || Time.time - time >= 5);
        } else if (difficulty == Difficulty.hard) {
            time = Time.time;
            yield return new WaitUntil(() => (true && true) || Time.time - time >= 5);
        }
        // уход зайца и начало игры
        i = ZayacTalk(i, Difficulty.none);
        yield return new WaitForSeconds(TreaningDialog[i-1].length);

        /***************************************************************/
        /************************ сама игра ****************************/
        /***************************************************************/
        while (Health > 0 && Round < 10)
        {
            Spawn();
            if (score >= 100) // каждые 10 яиц => ускорение времени их появления, или по-другому "новый уровень"
            {
                Round += 1;
                score -= 100;
                if (!comboFail) // если игрок не уронил ни одного яйца за раунд или другими словами собрал комбо из 10 яиц, то его жизнь++
                {
                    ChangeHealth(1);
                }
                comboFail = false;
            }
            yield return new WaitForSeconds(GiveTime());
        }

        /********************************************************************/
        /* демонстрация счета (пока под вопросом) и кнопка перезапуска игры */
        /********************************************************************/
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
        HealthWatch.text = Health.ToString();
        ScoreWatch = GameObject.FindGameObjectWithTag("Score Watch").GetComponentInChildren<Text>();
        ScoreWatch.text = score.ToString();

        zayac = GameObject.FindGameObjectWithTag("Zayac");
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

    private void ShowScore()
    {
        LH.isTrigger = false;
        RH.isTrigger = false;
    }

    private int ZayacTalk(int i, Difficulty GameDifficulty) // скрипт, чтобы не писать каждый отдельный взятый раз болтовню зайца. В массивах аудиозаписи должны лежать попорядку
    {
        if (difficulty == Difficulty.easy) { zayac.GetComponent<AudioSource>().PlayOneShot(TouchPart[i]); }
        else if (difficulty == Difficulty.hard) { zayac.GetComponent<AudioSource>().PlayOneShot(LeapPart[i]); }
        else { zayac.GetComponent<AudioSource>().PlayOneShot(TreaningDialog[i]); }

        return i++;
    }

    private void Spawn()
    {
        GameObject[] spawn_things = new GameObject[] {egg, bomb, egg};
        GameObject[] spawn_place = new GameObject[] {spawnPoint1, spawnPoint2, spawnPoint3};

        if (Round < 2) // первые два раунда падают только яйца с ускорением времени
        {
            Instantiate(egg, spawn_place[Random.Range(0, 3)].transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
        }
        else // а позже уже начинают появляться бомбы
        {
            Instantiate(spawn_things[Random.Range(0, 2)], spawn_place[Random.Range(0, 3)].transform.position,Quaternion.Euler(new Vector3(-90, 0, 0)));
        }
    }

    private float GiveTime()
    {
        float t = (10 - Round) * 0.5f; // расчет времени проиходит по формуле, зависящей от времени
        return t;
    }

    public void AddScore (int score_to_add)
    {
        score += score_to_add;
        ScoreWatch.text = (score + Round * 100).ToString(); // счет идет за каждый раунд свой, + 100 за каждый раунд
    }

    public void ChangeHealth(int delta)
    {
        if (delta == 0) { Health = 0; }
        else
        {
            Health += delta;
            HealthWatch.gameObject.GetComponent<AudioSource>().PlayOneShot( delta > 0 ? HeartAdd : HeartCrush);
            if (delta < 0) { comboFail = true; }
        }
        HealthWatch.text = Health.ToString();
    }
}