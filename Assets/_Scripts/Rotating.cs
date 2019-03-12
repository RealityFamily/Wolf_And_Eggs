using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotating : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float lerpSpeed;
    private float yDeg;
    public GameObject pig;
    private Quaternion fromRotation;
    private Quaternion toRotation;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        yDeg += speed;

        fromRotation = transform.rotation;
        toRotation = Quaternion.Euler(0, yDeg, 0);
        transform.rotation = Quaternion.Lerp(fromRotation, toRotation, Time.deltaTime * lerpSpeed);
    }
}
