using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using static GamePipeline;
using Random = UnityEngine.Random;

public class EggSpown : MonoBehaviour
{
    public GamePipeline gamePipeline;

    GameObject spawnPoint1;
    GameObject spawnPoint2;
    GameObject spawnPoint3;
    GameObject chicken1;
    GameObject chicken2;
    GameObject chicken3;
    public GameObject egg;
    public GameObject bomb;

    public int Round { get { return round; } set { round = value; } }
    public bool ComboFail { get { return comboFail; } set { comboFail = value; } }
    public bool Playing { get { return playing; } }
    int round = 1;
    bool comboFail = false;
    bool playing;

    void Start()
    {
        round = 1;
        comboFail = false;

        spawnPoint1 = transform.GetChild(0).gameObject;
        spawnPoint2 = transform.GetChild(1).gameObject;
        spawnPoint3 = transform.GetChild(2).gameObject;

        chicken1 = GameObject.Find("ChickenBrown1");
        chicken2 = GameObject.Find("ChickenBrown2");
        chicken3 = GameObject.Find("ChickenBrown3");
    }

    public void StartPlay()
    {
        StartCoroutine(GameTimeLine());
    }

    IEnumerator GameTimeLine ()
    {
        // востановление значений жизней и счета после обучения
        gamePipeline.playerLogic.Score = 0;
        gamePipeline.playerLogic.Health = 5;
        gamePipeline.playerLogic.ReloadWatchValues();
        playing = true;

        while (playing)
        {
            Spawn();
            if (gamePipeline.playerLogic.Score >= 100) // каждые 10 яиц => ускорение времени их появления, или по-другому "новый уровень"
            {
                round += 1;
                gamePipeline.playerLogic.Score -= 100;
                if (!comboFail) // если игрок не уронил ни одного яйца за раунд или другими словами собрал комбо из 10 яиц, то его жизнь++
                {
                    gamePipeline.playerLogic.ChangeHealth(1);
                }
                comboFail = false;
            }
            playing = gamePipeline.playerLogic.Health > 0 && round < 10;
            yield return new WaitForSeconds(GiveTime());
        }

        /********************************************************************/
        /* демонстрация счета (пока под вопросом) и кнопка перезапуска игры */
        /********************************************************************/
        gamePipeline.playerLogic.ShowScore();

        //yield return null; //заглушка для проверки
    }    

    private void Spawn()
    {
        string[] spawn_things = new string[] {"egg", "bomb", "egg"};
        GameObject[] chickens = new GameObject[] { chicken1, chicken2, chicken3 };

        int place = Random.Range(0, 3);

        if (Round < 2) // первые два раунда падают только яйца с ускорением времени
        {
            SpawnOnPlace("egg", place);
        }
        else // а позже уже начинают появляться бомбы
        {
            SpawnOnPlace(spawn_things[Random.Range(0, 2)], place);
        }
        chickens[place].GetComponent<AudioSource>().Play();
    }

    private float GiveTime()
    {
        float t = (10 - Round) * 0.5f; // расчет времени проиходит по формуле, зависящей от раунда
        return t;
    }

    public GameObject SpawnOnPlace(string item, int index_or_point)
    {
        GameObject[] spawn_place = new GameObject[] { spawnPoint1, spawnPoint2, spawnPoint3 };

        return Instantiate((item == "egg") ? egg : bomb, spawn_place[index_or_point].transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
    }
}