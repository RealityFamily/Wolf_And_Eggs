using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetInfo : MonoBehaviour
{
    enum Hand { right, left };
    [SerializeField]
    Hand hand;
    private void OnEnable()
    {
        StartCoroutine(Check());
    }

    IEnumerator Check()
    {
        try
        {
            if (hand == Hand.left)
            {
                GameObject.FindGameObjectWithTag("player").GetComponent<PlayerLogic>().RefreshHeath();
            }
            else if (hand == Hand.right)
            {
                GameObject.FindGameObjectWithTag("player").GetComponent<PlayerLogic>().RefreshHeath();
            }
        }
        catch { }
        yield return null;
    }
}
