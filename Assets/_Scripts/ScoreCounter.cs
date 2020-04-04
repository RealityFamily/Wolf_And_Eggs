using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    public GameObject coin;
    public AudioClip clip;

    private EggSpown gamePipeline;
    List<GameObject> gameObjects = new List<GameObject>();
    private float yDeg;
    private Quaternion fromRotation;
    private Quaternion toRotation;
    private float lerpSpeed = 100;

    private void Start()
    {
        gamePipeline = GameObject.FindGameObjectWithTag("Game Pipeline").GetComponent<EggSpown>();
    }

    private void Update()
    {
        if (GameObject.Find("CoinTen(Clone)") != null)
        {
            var ten = GameObject.Find("CoinTen(Clone)");
            yDeg += 1f;

            fromRotation = transform.rotation;
            toRotation = Quaternion.Euler(0, yDeg, 0);
            transform.rotation = Quaternion.Lerp(fromRotation, toRotation, Time.deltaTime * lerpSpeed);
        }

    }


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "egg")
        {
            GameObject created_coin = Instantiate(coin, transform.position, Quaternion.Euler(0f, 90f, 0f));
            created_coin.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 2f, 0f), ForceMode.Impulse);
            Destroy(created_coin, 3);
            gameObjects.Add(other.gameObject);
            gamePipeline.AddScore(10);
        }

        if (other.gameObject.tag == "Bomb")
        {
            other.gameObject.GetComponent<AudioSource>().PlayOneShot(clip);
            //here will be particle
            gamePipeline.ChangeHealth(0);
            Destroy(other.gameObject, 2.5f);
        }

        if (gameObjects.Count == 10)
        {
            while (gameObjects.Count > 0)
            {
                Destroy(gameObjects[0]);
                gameObjects.Remove(gameObjects[0]);
            }
        }
    }
}

