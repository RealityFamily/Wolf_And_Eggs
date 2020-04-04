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
                GameObject.FindGameObjectWithTag("Game Pipeline").GetComponent<EggSpown>().RefreshHeath();
            }
            else if (hand == Hand.right)
            {
                GameObject.FindGameObjectWithTag("Game Pipeline").GetComponent<EggSpown>().RefreshHeath();
            }
        }
        catch { }
        yield return null;
    }
}
