using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDisplayTimer : MonoBehaviour
{
    public GameObject text; //Show the rules of the game
    public float existTime;

    private void Start()
    {
        text.SetActive(true);
        StartCoroutine(ExecuteAfterTime(existTime));
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        text.SetActive(false);
    }
}
