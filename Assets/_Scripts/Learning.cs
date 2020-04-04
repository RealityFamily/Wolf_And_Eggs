using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GamePipeline;

public class Learning : MonoBehaviour
{
    public GamePipeline gamePipeline;
    public PlayerLogic playerLogic;
    public EggSpown eggSpawn;

    GameObject zayac;
    public bool Ready { get { return ready; } }
    bool ready = false;
    //аудиоклипы для обучения
    [SerializeField]
    List<AudioClip> TreaningDialog;
    [SerializeField]
    List<AudioClip> TouchPart;
    [SerializeField]
    List<AudioClip> LeapPart;

    public void Start_Learning()
    {
        StartCoroutine(Learn_Method());
    }

    IEnumerator Learn_Method()
    {
        // возможность начать игру без обучения
        float time = Time.time;
        bool skip = false;
        while (Time.time - time <= 5)
        {
            if ((OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) >= 0.75f && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) >= 0.75f && OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) >= 0.75f && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) >= 0.75f) || (playerLogic.LeftLeap && playerLogic.RightLeap))
            {
                skip = true;
            }
            yield return new WaitForSeconds(0.5f);
        }

        if (!skip)
        {
            int i = 0;
            // приветствие.
            yield return new WaitForSeconds(ZayacTalk(i, Difficulty_Type.none));
            i++;
            // объяснение выбранного режима.
            yield return new WaitForSeconds(ZayacTalk(i, Difficulty_Type.none));
            i++;
            if (gamePipeline.Difficulty == Difficulty_Type.easy)
            {
                int j = 0;
                // окончание фразы о выбранном режиме
                yield return new WaitForSeconds(ZayacTalk(j, gamePipeline.Difficulty));
                j++;
                //проба сжатия кулака на левой руке с контроллером.
                yield return new WaitForSeconds(ZayacTalk(j, gamePipeline.Difficulty));
                j++;
                while (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) < 0.75f && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) < 0.75f) { yield return new WaitForSeconds(0.01f); }
                // теперь правой рукой с контроллером
                yield return new WaitForSeconds(ZayacTalk(j, gamePipeline.Difficulty));
                j++;
                while (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) < 0.75f && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) < 0.75f) { yield return new WaitForSeconds(0.01f); }
                // теперь сжатие обоих рук без контроллеров, чтобы игрок привык к анимации
                yield return new WaitForSeconds(ZayacTalk(j, gamePipeline.Difficulty));
                j++;
                playerLogic.ControllersOff();
                while (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) < 0.75f && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) < 0.75f && OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) < 0.75f && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) < 0.75f) { yield return new WaitForSeconds(0.01f); }
            }
            else if (gamePipeline.Difficulty == Difficulty_Type.hard)
            {
                int j = 0;
                // окончание фразы о выбранном режиме
                yield return new WaitForSeconds(ZayacTalk(j, gamePipeline.Difficulty));
                j++;
                //проба сжатия руками кулака.
                yield return new WaitForSeconds(ZayacTalk(j, gamePipeline.Difficulty));
                j++;
                while (!playerLogic.LeftLeap && !playerLogic.RightLeap) { yield return new WaitForSeconds(0.01f); }
                playerLogic.LeftLeap = false;
                playerLogic.RightLeap = false;
            }
            // объяснение смысла игры.
            yield return new WaitForSeconds(ZayacTalk(i, Difficulty_Type.none));
            i++;
            // запуск яйцо по центру и просьба бросить его в корзину.
            yield return new WaitForSeconds(ZayacTalk(i, Difficulty_Type.none));
            i++; int t_score = playerLogic.Score;
            while (t_score == playerLogic.Score)
            {
                eggSpawn.SpawnOnPlace("egg", 1);
                yield return new WaitForSeconds(5);
            }
            // объяснение часов со счетом
            yield return new WaitForSeconds(ZayacTalk(i, Difficulty_Type.none));
            i++;
            // а теперь запускаем такое же яйцо, которое не возможно взять
            yield return new WaitForSeconds(ZayacTalk(i, Difficulty_Type.none));
            i++;
            GameObject t_egg = eggSpawn.SpawnOnPlace("egg", 1);
            Destroy(t_egg.GetComponent<OVRGrabbable>());

            yield return new WaitForSeconds(5);
            // объясние часов с жизнью
            yield return new WaitForSeconds(ZayacTalk(i, Difficulty_Type.none));
            i++;
            // комбо из 10 яиц => жизнь++. Договариваемся о возврате жизни
            yield return new WaitForSeconds(ZayacTalk(i, Difficulty_Type.none));
            i++; playerLogic.ChangeHealth(1);
            // ввод игрока в игру по показу знака "класс"
            yield return new WaitForSeconds(ZayacTalk(i, Difficulty_Type.none));
            i++;
            if (gamePipeline.Difficulty == Difficulty_Type.easy)
            {
                time = Time.time;
                yield return new WaitUntil(() => (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) >= 0.75f && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) >= 0.75f && OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) >= 0.75f && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) >= 0.75f) || Time.time - time >= 5);
            }
            else if (gamePipeline.Difficulty == Difficulty_Type.hard)
            {
                time = Time.time;
                yield return new WaitUntil(() => (playerLogic.LeftLeap && playerLogic.RightLeap) || Time.time - time >= 5);
            }
            // уход зайца и начало игры
            yield return new WaitForSeconds(ZayacTalk(i, Difficulty_Type.none));
            i++;
        }
        else { playerLogic.ControllersOff(); }
        ready = true;
    }

    private float ZayacTalk(int i, Difficulty_Type GameDifficulty) // скрипт, чтобы не писать каждый отдельный взятый раз болтовню зайца. В массивах аудиозаписи должны лежать попорядку
    {
        if (GameDifficulty == Difficulty_Type.easy)
        {
            GetComponent<AudioSource>().PlayOneShot(TouchPart[i]);
            return TouchPart[i].length;
        }
        else if (GameDifficulty == Difficulty_Type.hard)
        {
            GetComponent<AudioSource>().PlayOneShot(LeapPart[i]);
            return LeapPart[i].length;
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(TreaningDialog[i]);
            return TreaningDialog[i].length;
        }
    }
}
