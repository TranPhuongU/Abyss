using UnityEngine;
using UnityEngine.SceneManagement;

public class CutScene : MonoBehaviour
{
    private void Start()
    {
        SceneLoader.targetSceneName = "Map1";

        SceneManager.LoadScene("BootScene");
    }
}
