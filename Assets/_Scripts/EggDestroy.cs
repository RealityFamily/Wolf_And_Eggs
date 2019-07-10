using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EggDestroy : MonoBehaviour
{
    private int counter;
    private int index = 0;
    private EggSpown gamePipeline;

    private void Start()
    {
        gamePipeline = GameObject.FindGameObjectWithTag("Game Pipeline").GetComponent<EggSpown>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "egg")
        {
            AudioSource audio = collision.gameObject.GetComponent<AudioSource>();
            audio.Play();
            Destroy(collision.gameObject, 0.3f);
            gamePipeline.ChangeHealth(-1);
        }

        if (collision.gameObject.tag == "Bomb")
        {
            Destroy(collision.gameObject);
        }
    }
}
