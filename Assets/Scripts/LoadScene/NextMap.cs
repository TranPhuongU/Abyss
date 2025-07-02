using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextMap : MonoBehaviour
{
    [SerializeField] private string sceneName;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            StartCoroutine(TransitionToScene());
        }
    }

    IEnumerator TransitionToScene()
    {
        GameManager.instance.UnlockMap(sceneName);
        SaveManager.instance.SaveGame();

        UI_FadeScreen fade = GameObject.Find("DarkScreen")?.GetComponent<UI_FadeScreen>();

        // B1: FadeOut để làm tối màn hình
        if (fade != null)
        {
            fade.FadeOut(); // alpha từ 0 → 255
            yield return new WaitForSeconds(1.5f); // đợi màn hình tối xong
        }

        // B2: Gọi FadeIn NGAY trước khi load scene mới
        if (fade != null)
        {
            fade.FadeIn(); // alpha từ 255 → 0 (mờ dần)
        }

        // B3: Load scene mới NGAY LẬP TỨC
        SceneManager.LoadScene(sceneName);
    }
}
