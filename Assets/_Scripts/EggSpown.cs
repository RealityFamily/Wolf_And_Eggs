using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggSpown : MonoBehaviour
{
    public GameObject spawnPoint1;
    public GameObject spawnPoint2;
    public GameObject spawnPoint3;
    private float kadr;
    private float kadr1;
    private int randomInt;
    public GameObject egg;
    public GameObject bomb;
    public bool isPaused = false;


    void Update()
    {
        if (!isPaused)
        {
            kadr1 += Time.deltaTime;
            if (kadr1 > 2f)
            {
                kadr1 = 0;
                randomInt = Random.Range(1, 15);
            }
            switch (randomInt)
            {
                case 1:
                    EggClone(spawnPoint1);
                    break;
                case 2:
                    EggClone(spawnPoint2);
                    break;
                case 3:
                    EggClone(spawnPoint3);
                    break;
                case 4:
                    EggClone(spawnPoint1);
                    break;
                case 5:
                    BombClone(spawnPoint3);
                    break;
                case 6:
                    EggClone(spawnPoint3);
                    break;
                case 7:
                    EggClone(spawnPoint2);
                    break;
                case 8:
                    EggClone(spawnPoint1);
                    break;
                case 9:
                    EggClone(spawnPoint1);
                    break;
                case 10:
                    EggClone(spawnPoint3);
                    break;
                case 11:
                    EggClone(spawnPoint2);
                    break;
                case 12:
                    EggClone(spawnPoint2);
                    break;
                case 13:
                    BombClone(spawnPoint1);
                    break;
                case 14:
                    BombClone(spawnPoint3);
                    break;
                case 15:
                    BombClone(spawnPoint2);
                    break;
            }
        }
    }
    void EggClone(GameObject spawnPoint)
    {
        float x, y, z;
        x = Random.rotation.x;
        y = Random.rotation.y;
        z = Random.rotation.z;
        kadr += Time.deltaTime;
        if (kadr > 2f)
        {
            kadr = 0;
            new WaitForSecondsRealtime(1.5f);
            var res = Instantiate(egg);
            res.transform.position = spawnPoint.transform.position;
            res.transform.rotation = new Quaternion(x, y, z, w: 0);
        }
    }
    void BombClone(GameObject spawnPoint)
    {
        kadr += Time.deltaTime;
        if (kadr > 2f)
        {
            kadr = 0;
            Instantiate(bomb).transform.position = spawnPoint.transform.position;
        }
    }
}