using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{

    [SerializeField] private GameObject selectMapUI;

    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject buttonMap1;
    [SerializeField] private GameObject buttonMap2;
    [SerializeField] private GameObject buttonMap3;


    [SerializeField] private string sceneName = "MainScene";
    [SerializeField] private GameObject continueButton;
    [SerializeField] UI_FadeScreen fadeScreen;

    void Start()
    {
        if (!SaveManager.instance.HasSavedData())
            continueButton.SetActive(false);
        else
            continueButton.SetActive(true);

        selectMapUI.SetActive(false); // ẩn lúc đầu
    }


    public void ContinueGame()
    {
        var data = SaveManager.instance.gameData;

        if (data.unlockedMaps.Count <= 1)
        {
            //// chỉ có map1 → vào thẳng
            SceneLoader.targetSceneName = "Map1";
            SceneManager.LoadScene("BootScene");
        }
        else
        {
            mainMenuUI.SetActive(false);

            selectMapUI.SetActive(true);
            buttonMap1.SetActive(data.unlockedMaps.Contains("Map1"));
            buttonMap2.SetActive(data.unlockedMaps.Contains("Map2"));
            //buttonMap3.SetActive(data.unlockedMaps.Contains("Map3"));
        }
    }


    public void NewGame()
    {
        SaveManager.instance.DeleteSavedData();

        SceneManager.LoadScene("CutScene");
        //LoadMap1();
    }

    public void ExitGame()
    {
        Debug.Log("Exit game");
        //Application.Quit();
    }

    IEnumerator LoadSceneWithFadeEffect(float _delay)
    {
        fadeScreen.FadeOut();

        yield return new WaitForSeconds(_delay);

        SceneManager.LoadScene(sceneName);


    }

    public void LoadMap1()
    {
        SceneLoader.targetSceneName = "Map1";
        StartCoroutine(LoadSceneWithFadeEffect2(1.5f, "BootScene"));
    }

    public void LoadMap2()
    {
        SceneLoader.targetSceneName = "Map2";
        StartCoroutine(LoadSceneWithFadeEffect2(1.5f, "BootScene"));
    }

    IEnumerator LoadSceneWithFadeEffect2(float delay, string sceneToLoad)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneToLoad);
    }

    public void BackToMainMenu()
    {
        selectMapUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }


}
