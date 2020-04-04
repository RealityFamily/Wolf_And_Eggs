using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EggSpown : MonoBehaviour
{
    public enum Difficulty { none, easy, hard }

    GameObject spawnPoint1;
    GameObject spawnPoint2;
    GameObject spawnPoint3;
    GameObject chicken1;
    GameObject chicken2;
    GameObject chicken3;
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

    bool LeftLeap = false;
    bool RightLeap = false;

    [SerializeField]
    Transform ScoreTableTeleportPlace;

    void Start()
    {
        StartCoroutine(GameTimeLine());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Reload();
        }
    }

    IEnumerator GameTimeLine ()
    {
        difficulty = Difficulty.none;
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

        //ShowScore();

        /***************************************************************/
        /****************** начало обучения игрока *********************/
        /***************************************************************/
        // возможность начать игру без обучения
        time = Time.time;
        bool skip = false;
        while (Time.time - time <= 5)
        {
            if ((OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) >= 0.75f && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) >= 0.75f && OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) >= 0.75f && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) >= 0.75f) || (LeftLeap && RightLeap))
            {
                skip = true;
            }
            yield return new WaitForSeconds(0.5f);
        }

        if (!skip)
        {
            int i = 0;
            // приветствие.
            yield return new WaitForSeconds(ZayacTalk(i, Difficulty.none));
            i++;
            // объяснение выбранного режима.
            yield return new WaitForSeconds(ZayacTalk(i, Difficulty.none));
            i++;
            if (difficulty == Difficulty.easy)
            {
                int j = 0;
                // окончание фразы о выбранном режиме
                yield return new WaitForSeconds(ZayacTalk(j, difficulty));
                j++;
                //проба сжатия кулака на левой руке с контроллером.
                yield return new WaitForSeconds(ZayacTalk(j, difficulty));
                j++;
                while (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) < 0.75f && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) < 0.75f) { yield return new WaitForSeconds(0.01f); }
                // теперь правой рукой с контроллером
                yield return new WaitForSeconds(ZayacTalk(j, difficulty));
                j++;
                while (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) < 0.75f && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) < 0.75f) { yield return new WaitForSeconds(0.01f); }
                // теперь сжатие обоих рук без контроллеров, чтобы игрок привык к анимации
                yield return new WaitForSeconds(ZayacTalk(j, difficulty));
                j++;
                VR.GetComponent<OvrAvatar>().ShowControllers(false);
                while (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) < 0.75f && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) < 0.75f && OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) < 0.75f && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) < 0.75f) { yield return new WaitForSeconds(0.01f); }
            }
            else if (difficulty == Difficulty.hard)
            {
                int j = 0;
                // окончание фразы о выбранном режиме
                yield return new WaitForSeconds(ZayacTalk(j, difficulty));
                j++;
                //проба сжатия руками кулака.
                yield return new WaitForSeconds(ZayacTalk(j, difficulty));
                j++;
                while (!LeftLeap && !RightLeap) { yield return new WaitForSeconds(0.01f); }
                LeftLeap = false;
                RightLeap = false;
            }
            // объяснение смысла игры.
            yield return new WaitForSeconds(ZayacTalk(i, Difficulty.none));
            i++;            
            // запуск яйцо по центру и просьба бросить его в корзину.
            yield return new WaitForSeconds(ZayacTalk(i, Difficulty.none));
            i++; int t_score = score;
            while (t_score == score)
            {
                Instantiate(egg, spawnPoint2.transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
                yield return new WaitForSeconds(5);
            }
            // объяснение часов со счетом
            yield return new WaitForSeconds(ZayacTalk(i, Difficulty.none));
            i++;            
            // а теперь запускаем такое же яйцо, которое не возможно взять
            yield return new WaitForSeconds(ZayacTalk(i, Difficulty.none));
            i++;
            GameObject t_egg = Instantiate(egg, spawnPoint2.transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
            Destroy(t_egg.GetComponent<OVRGrabbable>());
            
            yield return new WaitForSeconds(5);
            // объясние часов с жизнью
            yield return new WaitForSeconds(ZayacTalk(i, Difficulty.none));
            i++;            
            // комбо из 10 яиц => жизнь++. Договариваемся о возврате жизни
            yield return new WaitForSeconds(ZayacTalk(i, Difficulty.none));
            i++; ChangeHealth(1);
            // ввод игрока в игру по показу знака "класс"
            yield return new WaitForSeconds(ZayacTalk(i, Difficulty.none));
            i++;
            if (difficulty == Difficulty.easy)
            {
                time = Time.time;
                yield return new WaitUntil(() => (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) >= 0.75f && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) >= 0.75f && OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) >= 0.75f && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) >= 0.75f) || Time.time - time >= 5);
            }
            else if (difficulty == Difficulty.hard)
            {
                time = Time.time;
                yield return new WaitUntil(() => (LeftLeap && RightLeap) || Time.time - time >= 5);
            }
            // уход зайца и начало игры
            yield return new WaitForSeconds(ZayacTalk(i, Difficulty.none));
            i++;
        } else { VR.GetComponent<OvrAvatar>().ShowControllers(false); }

        /***************************************************************/
        /************************ сама игра ****************************/
        /***************************************************************/

        // востановление значений жизней и счета после обучения
        score = 0;
        Health = 5;
        HealthWatch.text = Health.ToString();
        ScoreWatch.text = score.ToString();

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
        Round = 1;
        Health = 5;
        score = 0;
        comboFail = false;

        Teacher.SetActive(false);

        if (difficulty == Difficulty.easy) { VR.SetActive(true); }
        else if (difficulty == Difficulty.hard) { Leap.SetActive(true); }
        player.transform.position = gamePlace.position;
        player.transform.rotation = gamePlace.rotation;

        spawnPoint1 = transform.GetChild(0).gameObject;
        spawnPoint2 = transform.GetChild(1).gameObject;
        spawnPoint3 = transform.GetChild(2).gameObject;

        if (difficulty == Difficulty.easy)
        {
            LH = GameObject.FindGameObjectWithTag("Left Hand").GetComponent<BoxCollider>();
            RH = GameObject.FindGameObjectWithTag("Right Hand").GetComponent<BoxCollider>();
        }

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
        Leap.SetActive(false);
        Teacher.SetActive(true);

        player.transform.position = chooseDifficultyPlace.position;
        player.transform.rotation = chooseDifficultyPlace.rotation;
    }

    private void ShowScore()
    {
        player.transform.position = ScoreTableTeleportPlace.position;
        if (difficulty == Difficulty.easy)
        {
            LH.isTrigger = false;
            RH.isTrigger = false;
        } else if (difficulty == Difficulty.hard)
        {
            BoxCollider col = GameObject.FindGameObjectWithTag("Life Watch").transform.parent.gameObject.AddComponent<BoxCollider>();
            col.size = new Vector3(0.1f, 0.1f, 0.1f);
            col.center = new Vector3(-0.07f, 0, 0);
            col = GameObject.FindGameObjectWithTag("Score Watch").transform.parent.gameObject.AddComponent<BoxCollider>();
            col.size = new Vector3(0.1f, 0.1f, 0.1f);
            col.center = new Vector3(0.07f, 0, 0);

        }
    }

    private float ZayacTalk(int i, Difficulty GameDifficulty) // скрипт, чтобы не писать каждый отдельный взятый раз болтовню зайца. В массивах аудиозаписи должны лежать попорядку
    {
        if (GameDifficulty == Difficulty.easy) {
            zayac.GetComponent<AudioSource>().PlayOneShot(TouchPart[i]);
            return TouchPart[i].length;
        }
        else if (GameDifficulty == Difficulty.hard) {
            zayac.GetComponent<AudioSource>().PlayOneShot(LeapPart[i]);
            return LeapPart[i].length;
        }
        else {
            zayac.GetComponent<AudioSource>().PlayOneShot(TreaningDialog[i]);
            return TreaningDialog[i].length;
        }
    }

    private void Spawn()
    {
        GameObject[] spawn_things = new GameObject[] {egg, bomb, egg};
        GameObject[] spawn_place = new GameObject[] {spawnPoint1, spawnPoint2, spawnPoint3};
        GameObject[] chickens = new GameObject[] { chicken1, chicken2, chicken3 };

        int place = Random.Range(0, 3);

        if (Round < 2) // первые два раунда падают только яйца с ускорением времени
        {
            Instantiate(egg, spawn_place[place].transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
        }
        else // а позже уже начинают появляться бомбы
        {
            Instantiate(spawn_things[Random.Range(0, 2)], spawn_place[place].transform.position,Quaternion.Euler(new Vector3(-90, 0, 0)));
        }
        chickens[place].GetComponent<AudioSource>().Play();
    }

    private float GiveTime()
    {
        float t = (10 - Round) * 0.5f; // расчет времени проиходит по формуле, зависящей от раунда
        return t;
    }

    public void AddScore (int score_to_add)  // метод добавление счета игроку
    {
        score += score_to_add;
        try
        {
            ScoreWatch.text = (score + Round * 100).ToString(); // счет идет за каждый раунд свой, + 100 за каждый раунд
        }
        catch { }
    }

    public void ChangeHealth(int delta)  // метод для изменения числа оставшихся жизней у игрока
    {
        if (delta == 0) { Health = 0; }
        else
        {
            Health += delta;
            try
            {
                GameObject.FindGameObjectWithTag("Life Watch").GetComponent<AudioSource>().PlayOneShot(delta > 0 ? HeartAdd : HeartCrush);
            } catch { }
            if (delta < 0) { comboFail = true; }
        }
        HealthWatch.text = Health.ToString();
    }

    public void LeapCheck (string hand)  // Метод для вызова при сжатии рук с Leap
    {
        if (hand == "left") { LeftLeap = true; }
        else if (hand == "right") { RightLeap = true; }
    }

    public void LeapUncheck(string hand) // Метод для вызова при рассжатии рук с Leap
    {
        if (hand == "left") { LeftLeap = false; }
        else if (hand == "right") { RightLeap = false; }
    }

    public void RefreshHeath()  // метод обновления значения левых часов при появление руки с Leap
    {
        HealthWatch.text = Health.ToString();
    }

    public void RefreshScore()  // метод обновления значения правых часов при появление руки с Leap
    { 
        ScoreWatch.text = score.ToString();
    }

    public void Reload()
    {
        StopCoroutine(GameTimeLine());
        StartCoroutine(GameTimeLine());
    }
}