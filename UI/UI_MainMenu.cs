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

    //������Ӧ�ĳ���ʱ����һ��������Ч��
    IEnumerator loadSceneWithFadeEffect(float _delay)
    {
        Debug.Log("1111");
        //fadeScreen.fadeOut();
        SceneManager.LoadScene(sceneName);

        yield return new WaitForSeconds(_delay);

    }
}
