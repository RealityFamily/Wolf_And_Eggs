using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BombExplosion : MonoBehaviour
{
    public AudioClip clip;
    GameObject eggDestroy;
    private void Awake()
    {
        eggDestroy = GameObject.Find("Terrain");
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bomb")
        {
            //AudioSource audio = eggDestroy
            //    .GetComponent<EggDestroy>()
            //    .helths
            //    .FirstOrDefault(s => s.GetComponent<MeshRenderer>().enabled == true).GetComponent<AudioSource>();
            //audio.Play();

            //eggDestroy
            //    .GetComponent<EggDestroy>()
            //    .helths
            //    .FirstOrDefault(s => s.GetComponent<MeshRenderer>().enabled == true)
            //    .GetComponent<MeshRenderer>()
            //    .enabled = false;

            collision.gameObject.GetComponent<AudioSource>().PlayOneShot(clip);
            //here will be particle
            Destroy(collision.gameObject, 0.5f);
            Debug.Log("ALLAH AKBAAAAAR BOOOOM");
        }
    }
}
