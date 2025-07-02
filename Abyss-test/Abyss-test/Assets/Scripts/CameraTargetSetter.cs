using UnityEngine;
using Unity.Cinemachine;

public class CameraTargetSetter : MonoBehaviour
{
    private void Start()
    {
        var cam = GetComponent<CinemachineCamera>();
        Transform target = FindPlayer();

        if (target != null)
            cam.Follow = target;
        else
            Debug.LogWarning("Không tìm thấy Player (component) để gán vào Follow.");
    }

    private Transform FindPlayer()
    {
        Player playerComponent = Object.FindAnyObjectByType<Player>();

        if (playerComponent != null)
            return playerComponent.transform;

        return null;
    }
}
