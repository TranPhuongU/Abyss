using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UI_Options : MonoBehaviour
{
    public void SaveAndExit()
    {
        SaveManager.instance.SaveGame();

        SceneManager.LoadScene("MainMenu");
    }
}
