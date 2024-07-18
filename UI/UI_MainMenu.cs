using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainScene";
    [SerializeField] private GameObject continueButton;
    [SerializeField] UI_FadeScreen fadeScreen;

    private void Start()
    {
        if(SaveManager.instance.hasNoSaveData() == false)
            continueButton.SetActive(false);
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
        Debug.Log("1111");
        //fadeScreen.fadeOut();
        SceneManager.LoadScene(sceneName);

        yield return new WaitForSeconds(_delay);

    }
}
