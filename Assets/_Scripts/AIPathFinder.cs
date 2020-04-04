using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPathFinder : MonoBehaviour
{

    [SerializeField]
    GameObject AIPoints;
    NavMeshAgent agent;
    Animator pig;

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        pig = GetComponentInChildren<Animator>();
        agent.Warp(GetNewPoint());
        agent.SetDestination(GetNewPoint());
        pig.Play("Walk", 0, Random.Range(0.0f, 9.9f));
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance) { agent.SetDestination(GetNewPoint()); }

        AnimatorStateInfo state = pig.GetCurrentAnimatorStateInfo(0);
        AnimatorClipInfo[] clip = pig.GetCurrentAnimatorClipInfo(0);
        float myTime = clip[0].clip.length * state.normalizedTime;
        agent.speed = myTime % 7 > 4.18f ? 0 : 1.5f;
    }

    Vector3 GetNewPoint()
    {
        int len = AIPoints.transform.childCount;
        int i = Random.Range(0, len - 1);
        Vector3 target = AIPoints.transform.GetChild(i).transform.position;

        while (target == agent.destination)
        {
            i = Random.Range(0, len-1);
            target = AIPoints.transform.GetChild(i).position;
        }
        return target;
    }
}
