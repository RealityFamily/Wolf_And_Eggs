using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingScript : MonoBehaviour
{
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
            rb.AddForce(-transform.right);
        if (Input.GetKey(KeyCode.D))
            rb.AddForce(transform.right);
        if (Input.GetKey(KeyCode.W))
            rb.AddForce(transform.forward);
        if (Input.GetKey(KeyCode.S))
            rb.AddForce(-transform.forward);
        if (Input.GetKey(KeyCode.E))
            rb.AddForce(transform.up);
        if (Input.GetKey(KeyCode.Q))
            rb.AddForce(-transform.up);

    }
}
