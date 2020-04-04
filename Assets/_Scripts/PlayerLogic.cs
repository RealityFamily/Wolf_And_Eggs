using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GamePipeline;

public class PlayerLogic : MonoBehaviour
{
    public GamePipeline gamePipeline;
    public EggSpown eggSpawn;
    public int Score { get { return score; } set { score = value; } }
    public int Health { get { return health; } set { health = value; } }
    public bool LeftLeap { get { return leftLeap; } set { leftLeap = value; } }
    public bool RightLeap { get { return rightLeap; } set { rightLeap = value; } }

    OVRPlugin.SystemHeadset headset;
    GameObject Quest;
    GameObject Touch;
    GameObject Leap;

    GameObject player;
    BoxCollider LH;
    BoxCollider RH;

    Text HealthWatch;
    Text ScoreWatch;

    [SerializeField]
    AudioClip HeartAdd;
    [SerializeField]
    AudioClip HeartCrush;
    int score;
    int health;

    [SerializeField]
    Transform ScoreTableTeleportPlace;
    [SerializeField]
    Transform gamePlace;

    bool leftLeap = false;
    bool rightLeap = false;

    // Start is called before the first frame update
    void Start()
    {
        health = 5;
        score = 0;
        headset = OVRPlugin.GetSystemHeadsetType();

        HealthWatch = GameObject.FindGameObjectWithTag("Life Watch").GetComponentInChildren<Text>();
        ScoreWatch = GameObject.FindGameObjectWithTag("Score Watch").GetComponentInChildren<Text>();

        switch (headset)
        {
            case OVRPlugin.SystemHeadset.None:
            case OVRPlugin.SystemHeadset.GearVR_R320:
            case OVRPlugin.SystemHeadset.GearVR_R321:
            case OVRPlugin.SystemHeadset.GearVR_R322:
            case OVRPlugin.SystemHeadset.GearVR_R323:
            case OVRPlugin.SystemHeadset.GearVR_R324:
            case OVRPlugin.SystemHeadset.GearVR_R325:
            case OVRPlugin.SystemHeadset.Oculus_Go:
            case OVRPlugin.SystemHeadset.Rift_CB:
            case OVRPlugin.SystemHeadset.Rift_DK2:
            case OVRPlugin.SystemHeadset.Rift_DK1:
                Debug.Log("Unsurported HMD version");
                break;

            case OVRPlugin.SystemHeadset.Oculus_Quest:
                player = Quest;
                Quest.SetActive(true);
                Leap.SetActive(false);
                Touch.SetActive(false);
                break;

            case OVRPlugin.SystemHeadset.Rift_CV1:
            case OVRPlugin.SystemHeadset.Rift_S:
                if (gamePipeline.Difficulty == GamePipeline.Difficulty_Type.easy)
                {
                    player = Touch;
                    Leap.SetActive(false);
                    Touch.SetActive(true);
                    LH = GameObject.FindGameObjectWithTag("Left Hand").GetComponent<BoxCollider>();
                    RH = GameObject.FindGameObjectWithTag("Right Hand").GetComponent<BoxCollider>();
                } else if (gamePipeline.Difficulty == GamePipeline.Difficulty_Type.hard)
                {
                    player = Leap;
                    Leap.SetActive(true);
                    Touch.SetActive(false);
                }
                Quest.SetActive(false);
                break;
        }
    }

    public void putToGamePlace()
    {
        player.transform.position = gamePlace.position;
        player.transform.rotation = gamePlace.rotation;
    }

    public void AddScore(int score_to_add)  // метод добавление счета игроку
    {
        score += score_to_add;
        try
        {
            ScoreWatch.text = (score + eggSpawn.Round * 100).ToString(); // счет идет за каждый раунд свой, + 100 за каждый раунд
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
            }
            catch { }
            if (delta < 0) { eggSpawn.ComboFail = true; }
        }
        HealthWatch.text = Health.ToString();
    }

    public void ShowScore()
    {
        transform.position = ScoreTableTeleportPlace.position;
        if (gamePipeline.Difficulty == Difficulty_Type.easy)
        {
            LH.isTrigger = false;
            RH.isTrigger = false;
        }
        else if (gamePipeline.Difficulty == Difficulty_Type.hard)
        {
            BoxCollider col = GameObject.FindGameObjectWithTag("Life Watch").transform.parent.gameObject.AddComponent<BoxCollider>();
            col.size = new Vector3(0.1f, 0.1f, 0.1f);
            col.center = new Vector3(-0.07f, 0, 0);
            col = GameObject.FindGameObjectWithTag("Score Watch").transform.parent.gameObject.AddComponent<BoxCollider>();
            col.size = new Vector3(0.1f, 0.1f, 0.1f);
            col.center = new Vector3(0.07f, 0, 0);

        }
    }

    public void RefreshHeath()  // метод обновления значения левых часов при появление руки с Leap
    {
        HealthWatch.text = Health.ToString();
    }

    public void RefreshScore()  // метод обновления значения правых часов при появление руки с Leap
    {
        ScoreWatch.text = score.ToString();
    }

    public void ReloadWatchValues()
    {
        HealthWatch.text = Health.ToString();
        ScoreWatch.text = score.ToString();
    }

    public void LeapCheck(string hand)  // Метод для вызова при сжатии рук с Leap
    {
        if (hand == "left") { LeftLeap = true; }
        else if (hand == "right") { RightLeap = true; }
    }

    public void LeapUncheck(string hand) // Метод для вызова при рассжатии рук с Leap
    {
        if (hand == "left") { LeftLeap = false; }
        else if (hand == "right") { RightLeap = false; }
    }

    public void ControllersOff()
    {
        if (player == Quest && Quest.GetComponentInChildren<OVRTrackedRemote>().Quest_Work_Type == OVRTrackedRemote.Quest_Type.Controller)
        {
            // здесь нужна логика переключения контроллеров на руки
        } else if (player == Touch)
        {
            Touch.GetComponent<OvrAvatar>().ShowControllers(false);
        }
    }
}
