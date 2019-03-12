using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigPath : MonoBehaviour
{
    public Color lineColor;
    private List<Transform> nodes;
    public Transform[] path;
    public float reachDist = 0.5f;
    public float speed = 3f;
    public int currentPoint = 0;


    void Update()
    {
        if (path.Length == 0)
        {
            return;
        }

        float dist = Vector3.Distance(path[currentPoint].position, transform.position);
        transform.position = Vector3.MoveTowards(transform.position, path[currentPoint].position, Time.deltaTime * speed);

        if (dist <= reachDist)
        {
            currentPoint++;
        }
        if (currentPoint >= path.Length)
        {
            currentPoint = 0;
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = lineColor;
        Transform[] pathTransforms = GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != transform)
            {
                nodes.Add(pathTransforms[i]);
            }
        }
        for (int i = 0; i < nodes.Count; i++)
        {
            Vector3 currentPath = nodes[i].position;
            Vector3 previousPath = Vector3.zero;
            if (i > 0)
            {
                previousPath = nodes[i - 1].position;
            }
            else if (i == 0 && nodes.Count > 1)
            {
                previousPath = nodes[nodes.Count - 1].position;
            }
            Gizmos.DrawLine(previousPath, currentPath);
            Gizmos.DrawWireSphere(currentPath, 0.3f);
        }
    }
}
