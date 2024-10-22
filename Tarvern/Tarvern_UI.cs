using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tarvern_UI : MonoBehaviour
{
    public GameObject eButton;

    private void Update()
    {
        if(eButton.activeSelf && Input.GetKeyDown(KeyCode.E)) LeaveTavern();
    }

    //Àë¿ª¾Æ¹Ý
    public void LeaveTavern()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        eButton.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        eButton.SetActive(false);
    }
}
