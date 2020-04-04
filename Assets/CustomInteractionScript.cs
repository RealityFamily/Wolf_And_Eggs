using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractionBehaviour))]
public class CustomInteractionScript : MonoBehaviour
{
    private InteractionBehaviour interactionBehaviour;

    private void Start()
    {
        if (GameObject.FindGameObjectWithTag("Game Pipeline").GetComponent<EggSpown>().difficulty == EggSpown.Difficulty.hard)
        {
            interactionBehaviour = GetComponent<InteractionBehaviour>();
            interactionBehaviour.manager = GameObject.FindGameObjectWithTag("player").GetComponentInChildren<InteractionManager>();
            interactionBehaviour.enabled = true;
        }
    }
    private void Update()
    {
        
    }
}
