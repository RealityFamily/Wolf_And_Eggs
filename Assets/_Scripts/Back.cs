using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Back : MonoBehaviour
{
    public Text text;
    public void Clicker()
    {
        text.text = Random.Range(0, 10).ToString();
        StartCoroutine(Invoke(Random.Range(0, 10)));
    }

    private IEnumerator Invoke(int score)
    {
        var www1 = UnityWebRequest.Post("http://localhost:59665/api/scores/hard/score", score.ToString());


        using (var www = UnityWebRequest.Get("http://localhost:59665/api/scores/hard/" + score))
        {
            yield return www;
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }
}
