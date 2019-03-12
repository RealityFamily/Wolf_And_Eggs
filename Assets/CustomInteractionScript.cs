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
        interactionBehaviour = GetComponent<InteractionBehaviour>();
    }
    private void Update()
    {
        
    }
}
