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
    [SerializeField]
    private GameObject heart;
    [SerializeField]
    private GameObject heart2;
    [SerializeField]
    private GameObject heart3;
    public Collider terrain;
    private GameObject egg;
    public List<GameObject> helths;

    void Start()
    {
        helths = new List<GameObject>
        {
            heart,
            heart2,
            heart3
        };
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "egg")
        {
            egg = GameObject.FindGameObjectWithTag("egg");
            AudioSource audio = egg.GetComponent<AudioSource>();
            audio.Play();
            Destroy(collision.gameObject, 0.3f);

            counter++;
            if (counter == 3)
            {
                if (helths[index].GetComponent<MeshRenderer>().enabled == false)
                {
                    AudioSource audio1 =  helths[index + 1].GetComponent<AudioSource>();
                    audio1.Play();
                    helths[index + 1].GetComponent<MeshRenderer>().enabled = false;
                }
                else
                {
                    helths[index].GetComponent<AudioSource>().Play();
                    helths[index].GetComponent<MeshRenderer>().enabled = false;
                }
                if (index < helths.Count)
                {
                    index++;
                }

                counter = 0;
            }
            if (index == helths.Count)
            {
                FindObjectOfType<EggSpown>().isPaused = true;
            }
        }

        if (collision.gameObject.tag == "Bomb")
        {
            Destroy(collision.gameObject);
        }
    }
}
