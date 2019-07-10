
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigLookAt : MonoBehaviour
{
    public Transform node1;
    public Transform node2;
    public Transform node3;
    public Transform node4;
    public Transform node5;
    public Transform node6;
    private List<Transform> nodes;

    private void Start()
    {
        nodes.Add(node1);
        nodes.Add(node2);
        nodes.Add(node3);
        nodes.Add(node4);
        nodes.Add(node5);
        nodes.Add(node6);
    }

    void Update()
    {
        foreach (var node in nodes)
        {
            transform.LookAt(node);
        }
    }
}
