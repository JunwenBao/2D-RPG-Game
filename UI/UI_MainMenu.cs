//using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private GameObject continueButton;
    [SerializeField] UI_FadeScreen fadeScreen;

    private void Start()
    {
        if(SaveManager.instance.hasNoSaveData() == false) DisableButton();
    }

    public void continueGame()
    {
        StartCoroutine(loadSceneWithFadeEffect(1.5f));
    }

    public void newGame()
    {
        SaveManager.instance.deleteSaveData();

        StartCoroutine(loadSceneWithFadeEffect(1.5f));
    }

    public void exitGame()
    {
        Debug.Log("EXIT");
        //Application.Quit();
    }

    //进入相应的场景时，有一个渐进的效果
    IEnumerator loadSceneWithFadeEffect(float _delay)
    {
        //fadeScreen.fadeOut();
        SceneManager.LoadScene(sceneName);

        yield return new WaitForSeconds(_delay);

    }

    public void DisableButton()
    {
        Color disabledColor = new Color(1, 1, 1, 0.5f); //Disable button color
        continueButton.GetComponent<Button>().interactable = false;
        continueButton.GetComponent<Image>().color = disabledColor;
        continueButton.GetComponentInChildren<TextMeshProUGUI>().color = disabledColor;
    }
}
