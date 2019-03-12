using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class ScoreCounter : MonoBehaviour
{
    public GameObject spown;
    public GameObject coin;
    public Text score;
    private int count = 10;
    List<GameObject> gameObjects = new List<GameObject>();
    private float yDeg;
    private Quaternion fromRotation;
    private Quaternion toRotation;
    private float lerpSpeed = 100;

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
            score.text = count.ToString();
            Instantiate(coin, spown.transform.position, Quaternion.Euler(0f, 90f, 0f)).GetComponent<Rigidbody>().AddForce(new Vector3(0f, 2f, 0f), ForceMode.Impulse);
            gameObjects.Add(other.gameObject);
        }
        count += 10;
        if (gameObjects.Count == 30)
        {
            foreach (var gameobject in gameObjects)
            {
                Destroy(gameobject);
                gameObjects.Remove(gameobject);
            }
        }
    }
}

