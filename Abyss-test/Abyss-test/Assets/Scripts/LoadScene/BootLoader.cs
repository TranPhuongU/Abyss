using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootLoader : MonoBehaviour
{
    private void Start()
    {
        // Load scene thực sự
        StartCoroutine(LoadTargetScene());
    }

    IEnumerator LoadTargetScene()
    {
        yield return null; // có thể đợi vài frame nếu cần chuẩn bị gì thêm

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneLoader.targetSceneName);

        yield return asyncLoad;

    }
}
